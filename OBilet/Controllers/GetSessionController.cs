using System.Net;
using Microsoft.AspNetCore.Mvc;
using OBilet.Extensions;
using OBilet.Model.Session;
using RestSharp;

namespace OBilet.Controllers;

[ApiController]
[Route("[controller]")]
public class GetSessionController : ControllerBase
{
    // Rest ve log için dependency injection.
    
    private readonly ILogger<GetSessionController> _logger;
    private const string HandleForLog = "{@GetSession}";
    private readonly RestClient _restClient;

    public GetSessionController(ILogger<GetSessionController> logger, RestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }
    
    //MVC view tarafını oluşturmadım. O yüzden web api olarak çalışmaktadır. Fakat MVC yapısına döneceği zaman IActionResult olarak değiştirilebilir
    
    [HttpPost(Name = "GetSession")]
    public async Task<SessionResponse> GetSession()
    {
        string url = $"{Const.Url}/client/getsession";
        
        var data = new SessionRequest(new Connection(), new Application());

        var request = new RestRequest(url, Method.Post)
            .AddHeader("Content-Type", "application/json")
            .AddHeader("Authorization", "Basic JEcYcEMyantZV095WVc3G2JtVjNZbWx1")
            .AddJsonBody(data);

        var response = await _restClient.ExecutePostAsync<SessionResponse>(request);

        //Exception'ları genellikle kendim Handle ediyorum, bir middleware yazılabilir
        if (response.ErrorException is InvalidOperationException)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Deserialization error"
            });

            return new SessionResponse(default);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Endpoint not found"
            });

            return new SessionResponse(default);
        }

        if (response.ResponseStatus == ResponseStatus.Error)
        {
            var message = response.StatusCode switch
            {
                HttpStatusCode.InternalServerError => "Endpoint down or unavailable",
                HttpStatusCode.BadRequest => "Endpoint rejected request. Check sent JSON",
                _ => "Unexpected error"
            };

            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = message
            });

            return new SessionResponse(default);
        }

        if (response.ResponseStatus == ResponseStatus.TimedOut)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Request timeout"
            });
            
            return new SessionResponse(default);
        }

        return response.Data!;
    }
}

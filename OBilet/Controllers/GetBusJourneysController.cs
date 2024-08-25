using System.Net;
using Microsoft.AspNetCore.Mvc;
using OBilet.Extensions;
using OBilet.Model;
using OBilet.Model.BusJourney;
using RestSharp;

namespace OBilet.Controllers;

[ApiController]
[Route("[controller]")]
public class GetBusJourneysController : ControllerBase
{
    // Rest ve log için dependency injection.
    
    private readonly ILogger<GetBusJourneysController> _logger;
    private const string HandleForLog = "{@GetBusJourneys}";
    private readonly RestClient _restClient;

    public GetBusJourneysController(ILogger<GetBusJourneysController> logger, RestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }

    //MVC view tarafını oluşturmadım. O yüzden web api olarak çalışmaktadır. Fakat MVC yapısına döneceği zaman IActionResult olarak değiştirilebilir

    [HttpPost(Name = "GetBusJourneys")]
    public async Task<BusJourneyResponse> GetBusLocations(string sessionId, string deviceId, int originId, int destinationId, DateTime departureDate)
    {
        string url = $"{Const.Url}/journey/getbusjourneys";

        var data = new BusJourneyRequest()
        {
            Data = new BusJourneyReqData(originId, destinationId, departureDate.ToString("yyyy-MM-ddTHH:mm:ss")),
            Date = DateTime.Now,
            DeviceSession = new DeviceSession(sessionId, deviceId)
        };

        var request = new RestRequest(url, Method.Post)
            .AddHeader("Content-Type", "application/json")
            .AddHeader("Authorization", "Basic JEcYcEMyantZV095WVc3G2JtVjNZbWx1")
            .AddJsonBody(data);

        var response = await _restClient.ExecutePostAsync<BusJourneyResponse>(request);

        //Exception'ları genellikle kendim Handle ediyorum, bir middleware yazılabilir
        if (response.ErrorException is InvalidOperationException)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Deserialization error"
            });

            return new BusJourneyResponse();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Endpoint not found"
            });

            return new BusJourneyResponse();
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

            return new BusJourneyResponse();
        }

        if (response.ResponseStatus == ResponseStatus.TimedOut)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Request timeout"
            });
            
            return new BusJourneyResponse();
        }

        return response.Data!;
    }
}

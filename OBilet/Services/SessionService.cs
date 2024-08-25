using System.Net;
using OBilet.Extensions;
using OBilet.Model.Session;
using RestSharp;

namespace OBilet.Services;

public class SessionService
{
    private readonly ILogger<SessionService> _logger;
    private const string HandleForLog = "{@GetSession}";
    private readonly RestClient _restClient;

    public SessionService(ILogger<SessionService> logger, RestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }

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
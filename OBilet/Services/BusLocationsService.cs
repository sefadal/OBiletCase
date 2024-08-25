using System.Net;
using OBilet.Extensions;
using OBilet.Model;
using OBilet.Model.BusLocation;
using OBilet.Model.Session;
using RestSharp;

namespace OBilet.Services;

public class BusLocationsService
{
    private readonly ILogger<BusLocationsService> _logger;
    private const string HandleForLog = "{@GetSession}";
    private readonly RestClient _restClient;

    public BusLocationsService(ILogger<BusLocationsService> logger, RestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }

    public async Task<BusLocationResponse> GetBusLocations()
    {
        SessionService sessionService = new SessionService(new Logger<SessionService>(new LoggerFactory()), _restClient);

        var session = await sessionService.GetSession();

        if (session.Data == null)
        {
            throw new Exception();
        }

        string url = $"{Const.Url}/location/getbuslocations";

        var data = new BusLocationRequest
        {
            Date = DateTime.Now,
            DeviceSession = new DeviceSession(session.Data.SessionId, session.Data.DeviceId)
        };

        var request = new RestRequest(url, Method.Post)
            .AddHeader("Content-Type", "application/json")
            .AddHeader("Authorization", "Basic JEcYcEMyantZV095WVc3G2JtVjNZbWx1")
            .AddJsonBody(data);

        var response = await _restClient.ExecutePostAsync<BusLocationResponse>(request);

        //Exception'ları genellikle kendim Handle ediyorum, bir middleware yazılabilir
        if (response.ErrorException is InvalidOperationException)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Deserialization error"
            });

            return new BusLocationResponse();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Endpoint not found"
            });

            return new BusLocationResponse();
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

            return new BusLocationResponse();
        }

        if (response.ResponseStatus == ResponseStatus.TimedOut)
        {
            _logger.LogInformation(HandleForLog, new
            {
                Request = new { Parameters = request.Parameters },
                Response = new { Data = response.Data },
                Message = "Request timeout"
            });
            
            return new BusLocationResponse();
        }

        return response.Data!;
    }
}
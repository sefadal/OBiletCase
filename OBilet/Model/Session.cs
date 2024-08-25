using System.Text.Json.Serialization;

namespace OBilet.Model.Session;

public sealed record SessionResponse(
    [property: JsonPropertyName("data")] SessionData? Data
);

public sealed record SessionData(
    [property: JsonPropertyName("session-id")] string SessionId,
    [property: JsonPropertyName("device-id")] string DeviceId
);

public sealed record SessionRequest(
    [property: JsonPropertyName("connection")] Connection Connection,
    [property: JsonPropertyName("application")] Application Application,
    [property: JsonPropertyName("type")] int Type = 2
    
    /*
     * Api dökümanına göre Type 7 belirtilmiş fakat aşağıdaki hatayı vermektedir. Bu yüzden 2 olarak tanımladım.
     
     * "Port can not be null for browsers.\r\nParameter name: Port\r\nBrowser can not be null for browsers.
     * \r\nParameter name: Browser\r\nTargetSite: Void ValidateSessionRequest(oBilet.Clients.DeviceSessionRequest)\r\nStackTrace:
     * at oBilet.Api.Controllers.ClientController.ValidateSessionRequest(DeviceSessionRequest request) in
     * C:\\Users\\mustafa.yildirim\\Repos\\obilet\\src\\api\\Controllers\\ClientController.cs:line 929\r\n
     * at oBilet.Api.Controllers.ClientController.GetSession(DeviceSessionRequest request) in
     * C:\\Users\\mustafa.yildirim\\Repos\\obilet\\src\\api\\Controllers\\ClientController.cs:line 113"
    */
);

public sealed record Connection(
    [property: JsonPropertyName("ip-address")] string IpAddress = "127.0.0.0"
);

public sealed record Application(
    [property: JsonPropertyName("version")] string Version = "1.0.0.0",
    [property: JsonPropertyName("equipment-id")] string EquipmentId = "distribusion"
);

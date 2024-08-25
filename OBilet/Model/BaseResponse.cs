using System.Text.Json.Serialization;
using RestSharp;

namespace OBilet.Model;

/*
 * Record tipinde abstract bir record tanımladım. Base olarak tanımlanan bu record dğer recordlar tarafından referans
 * alınmaktadır. Record'u immutable olmasından dolayı kullandım ve compile edilirken class'lardan daha performanslı
 * çalışmaktadır. Sealed ise başka sınıflardan miras alınamayacağını belirtmek için kullandım.
*/

public abstract record BaseResponse<T>
{
    [property: JsonPropertyName("status")] public string? Status { get; set; }
    [property: JsonPropertyName("data")] public IEnumerable<T>? Data { get; set; }
    [property: JsonPropertyName("message")] public string? Message { get; set; }
    [property: JsonPropertyName("user-message")] public string? UserMessage { get; set; }
    [property: JsonPropertyName("api-request-id")] public string? ApiRequestId { get; set; }
    [property: JsonPropertyName("controller")] public string? Controller { get; set; }
}


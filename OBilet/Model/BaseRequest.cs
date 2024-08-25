using System.Text.Json.Serialization;

namespace OBilet.Model;

/*
 * Record tipinde abstract bir record tanımladım. Base olarak tanımlanan bu record dğer recordlar tarafından referans
 * alınmaktadır. Record'u immutable olmasından dolayı kullandım ve compile edilirken class'lardan daha performanslı
 * çalışmaktadır. Sealed ise başka sınıflardan miras alınamayacağını belirtmek için kullandım.
*/

public abstract record BaseRequest<T>
{
    [property: JsonPropertyName("data")] public T? Data { get; set; }
    [property: JsonPropertyName("device-session")] public DeviceSession? DeviceSession { get; set; }
    [property: JsonPropertyName("dete")] public DateTime Date { get; set; }
    [property: JsonPropertyName("language")] public string Language => "en-EN";
}


public sealed record DeviceSession(
    [property: JsonPropertyName("session-id")] string SessionId,
    [property: JsonPropertyName("device-id")] string DeviceId
);



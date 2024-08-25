using System.Text.Json.Serialization;

namespace OBilet.Model.BusLocation;

public sealed record BusLocationRequest : BaseRequest<BusLocationReqData>;

public sealed record BusLocationReqData;

public sealed record BusLocationResponse : BaseResponse<BusLocationResData>;

public sealed record BusLocationResData(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("parent-id")] int? ParentId,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("geo-location")] GeoLocation GeoLocation,
    [property: JsonPropertyName("tz-code")] string TzCode,
    [property: JsonPropertyName("weather-code")] string? WeatherCode,
    [property: JsonPropertyName("rank")] int? Rank,
    [property: JsonPropertyName("reference-code")] string? ReferenceCode,
    [property: JsonPropertyName("keywords")] string KeyWords
);

public sealed record GeoLocation(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("zoom")] int Zoom //Api dokumanında string görünüyor fakat değişken int
);

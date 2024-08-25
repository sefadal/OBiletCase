using System.Text.Json.Serialization;

namespace OBilet.Model.BusJourney;

// OOP
public sealed record BusJourneyRequest : BaseRequest<BusJourneyReqData>;

public sealed record BusJourneyReqData(
    [property: JsonPropertyName("origin-id")] int OriginId,
    [property: JsonPropertyName("destination-id")] int DestinationId,
    [property: JsonPropertyName("departure-date")] string DepartureDate //Api dokumanına göre int fakat değer string tipinde
);

public sealed record BusJourneyResponse : BaseResponse<BusJourneyResData>;

public sealed record BusJourneyResData(
    [property: JsonPropertyName("id")] int Id, //Api dokumanına göre string fakat değer int tipinde
    [property: JsonPropertyName("partner-id")] int PartnerId, //Api dokumanına göre string fakat değer int tipinde
    [property: JsonPropertyName("partner-name")] string PartnerName,
    [property: JsonPropertyName("route-id")] int RouteId,
    [property: JsonPropertyName("bus-type")] string BusType,
    [property: JsonPropertyName("total-seats")] int TotalSeats,
    [property: JsonPropertyName("available-seats")] int AvailableSeats,
    [property: JsonPropertyName("journey")] Journey Journey,
    [property: JsonPropertyName("features")] IEnumerable<Features> Features,
    [property: JsonPropertyName("origin-location")] string OriginLocation,
    [property: JsonPropertyName("destination-location")] string DestinationLocation,
    [property: JsonPropertyName("is-active")] bool IsActive,
    [property: JsonPropertyName("origin-location-id")] int OriginLocationId,
    [property: JsonPropertyName("destination-location-id")] int DestinationLocationId,
    [property: JsonPropertyName("is-promoted")] bool IsPromoted,
    [property: JsonPropertyName("cancellation-offset")] int? CancellationOffset,
    [property: JsonPropertyName("has-bus-shuttle")] bool HasBusShuttle,
    [property: JsonPropertyName("disable-sales-without-gov-id")] bool DisableSalesWithoutGovId,
    [property: JsonPropertyName("display-offset")] TimeSpan? DisplayOffset, // nullable Api dökümanında belirtilmemiş.
    [property: JsonPropertyName("partner-rating")] decimal? PartnerRating
);

public sealed record Journey(
    [property: JsonPropertyName("kind")] string Kind,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("stops")] IEnumerable<Stops> Stops,
    [property: JsonPropertyName("origin")] string Origin,
    [property: JsonPropertyName("destination")] string Destination,
    [property: JsonPropertyName("departure")] DateTime Departure,
    [property: JsonPropertyName("arrival")] DateTime Arrival,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("duration")] TimeSpan Duration,
    [property: JsonPropertyName("original-price")] decimal OriginalPrice,
    [property: JsonPropertyName("internet--rice")] decimal InternetPrice, //Api dokumanına göre string fakat değer decimal tipinde
    [property: JsonPropertyName("booking")] IEnumerable<string>? Booking, //nullable Api dökumanına göre array fakat tipi belli değil
    [property: JsonPropertyName("bus-name")] string BusName,
    [property: JsonPropertyName("policy")] Policy Policy,
    [property: JsonPropertyName("features")] IEnumerable<string> Features,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("available")] string Available //Api dokumanına göre eksik veri tipi
);

public sealed record Stops(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("station")] string Station, //Api dokumanına göre int fakat değer string tipinde
    [property: JsonPropertyName("time")] string Time,
    [property: JsonPropertyName("is-origin")] bool IsOrigin,
    [property: JsonPropertyName("is-destination")] bool IsDestination
);

public sealed record Policy(
    [property: JsonPropertyName("max-seats")] int? MaxSeats,
    [property: JsonPropertyName("max-single")] int? MaxSingle,
    [property: JsonPropertyName("max-single-males")] int? MaxSingleMales,
    [property: JsonPropertyName("max-single-demales")] int? MaxSingleFemales,
    [property: JsonPropertyName("mixed-genders")] bool MixedGenders,
    [property: JsonPropertyName("gov-id")] bool GovId,
    [property: JsonPropertyName("lht")] bool Lht
);

public sealed record Features
(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("priority")] byte? Priority,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("is-promoted")] bool  IsPromoted,
    [property: JsonPropertyName("back-color")] string BackColor,
    [property: JsonPropertyName("fore-color")] string ForeColor
);

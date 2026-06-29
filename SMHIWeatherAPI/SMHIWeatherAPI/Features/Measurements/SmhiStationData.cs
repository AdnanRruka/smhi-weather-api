namespace SMHIWeatherAPI.Features.Measurements;

public record SmhiStationData(
    string StationId,
    string StationName,
    double Latitude,
    double Longitude,
    double? Value,
    DateTimeOffset Timestamp
);

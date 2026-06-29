namespace SMHIWeatherAPI.Features.Measurements.Dtos;

public record StationMeasurement(
    string StationId,
    string StationName,
    double Latitude,
    double Longitude,
    double? Temperature,
    double? GustWind,
    DateTimeOffset Timestamp
);

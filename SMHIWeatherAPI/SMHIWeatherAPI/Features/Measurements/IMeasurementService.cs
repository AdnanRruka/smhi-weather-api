using SMHIWeatherAPI.Features.Measurements.Dtos;

namespace SMHIWeatherAPI.Features.Measurements;

public interface IMeasurementService
{
    Task<IEnumerable<StationMeasurement>> GetMeasurementsAsync(int? stationId, string period);
}

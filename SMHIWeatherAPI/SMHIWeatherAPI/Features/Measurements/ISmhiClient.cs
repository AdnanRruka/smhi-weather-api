namespace SMHIWeatherAPI.Features.Measurements;

public interface ISmhiClient
{
    Task<IEnumerable<SmhiStationData>> GetTemperaturesAsync(int? stationId, string period);
    Task<IEnumerable<SmhiStationData>> GetGustWindsAsync(int? stationId, string period);
}

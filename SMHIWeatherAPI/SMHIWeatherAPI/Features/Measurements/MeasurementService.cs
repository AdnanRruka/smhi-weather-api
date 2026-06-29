using SMHIWeatherAPI.Features.Measurements.Dtos;

namespace SMHIWeatherAPI.Features.Measurements;

public class MeasurementService(ISmhiClient smhiClient) : IMeasurementService
{
    public async Task<IEnumerable<StationMeasurement>> GetMeasurementsAsync(int? stationId, string period)
    {
        var temperatureTask = smhiClient.GetTemperaturesAsync(stationId, period);
        var gustWindTask = smhiClient.GetGustWindsAsync(stationId, period);

        await Task.WhenAll(temperatureTask, gustWindTask);

        var tempByStation = temperatureTask.Result.ToDictionary(temp => temp.StationId);
        var windByStation = gustWindTask.Result.ToDictionary(wind => wind.StationId);

        var allStationIds = tempByStation.Keys.Union(windByStation.Keys);

        return allStationIds.Select(id =>
        {
            var temp = tempByStation.GetValueOrDefault(id);
            var wind = windByStation.GetValueOrDefault(id);
            var station = temp ?? wind!;

            return new StationMeasurement(
                id,
                station.StationName,
                station.Latitude,
                station.Longitude,
                temp?.Value,
                wind?.Value,
                temp?.Timestamp ?? wind!.Timestamp
            );
        });
    }
}

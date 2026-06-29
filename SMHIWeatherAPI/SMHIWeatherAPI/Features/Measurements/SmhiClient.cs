using SMHIWeatherAPI.Features.Measurements.Dtos;
using System.Globalization;
using System.Net.Http.Json;

namespace SMHIWeatherAPI.Features.Measurements;

public class SmhiClient(HttpClient httpClient) : ISmhiClient
{
    private const int TemperatureParameter = 1;
    private const int GustWindParameter = 21;

    public Task<IEnumerable<SmhiStationData>> GetTemperaturesAsync(int? stationId, string period) =>
        FetchAsync(TemperatureParameter, stationId, period);

    public Task<IEnumerable<SmhiStationData>> GetGustWindsAsync(int? stationId, string period) =>
        FetchAsync(GustWindParameter, stationId, period);

    private async Task<IEnumerable<SmhiStationData>> FetchAsync(int parameter, int? stationId, string period)
    {
        if (stationId.HasValue)
        {
            var url = $"parameter/{parameter}/station/{stationId}/period/{period}/data.json";
            var httpResponse = await httpClient.GetAsync(url);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                return [];

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.Content.ReadFromJsonAsync<SmhiSingleStationResponse>();
            return response is null ? [] : MapSingleStation(response);
        }
        else
        {
            var url = $"parameter/{parameter}/station-set/all/period/{period}/data.json";
            var response = await httpClient.GetFromJsonAsync<SmhiAllStationsResponse>(url);
            return response is null ? [] : MapAllStations(response);
        }
    }

    private static IEnumerable<SmhiStationData> MapAllStations(SmhiAllStationsResponse response) =>
        response.Station
            .Where(station => station.Value.Count > 0)
            .Select(station => new SmhiStationData(
                station.Key,
                station.Name,
                station.Latitude,
                station.Longitude,
                ParseValue(station.Value[^1].Value),
                DateTimeOffset.FromUnixTimeMilliseconds(station.Value[^1].Date)
            ));

    private static IEnumerable<SmhiStationData> MapSingleStation(SmhiSingleStationResponse response)
    {
        if (response.Value.Count == 0) return [];

        var latest = response.Value[^1];
        var position = response.Position.FirstOrDefault();

        return [new SmhiStationData(
            response.Station.Key,
            response.Station.Name,
            position?.Latitude ?? 0,
            position?.Longitude ?? 0,
            ParseValue(latest.Value),
            DateTimeOffset.FromUnixTimeMilliseconds(latest.Date)
        )];
    }

    private static double? ParseValue(string value) =>
        double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : null;
}

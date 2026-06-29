using Moq;
using SMHIWeatherAPI.Features.Measurements;

namespace SMHIWeatherAPI.Tests;

public class MeasurementServiceTests
{
    private readonly Mock<ISmhiClient> _smhiClientMock = new();
    private readonly MeasurementService _sut;

    public MeasurementServiceTests()
    {
        _sut = new MeasurementService(_smhiClientMock.Object);
    }

    [Fact]
    public async Task GetMeasurements_WhenBothDatasetsHaveSameStation_MergesCorrectly()
    {
        var timestamp = DateTimeOffset.UtcNow;

        _smhiClientMock
            .Setup(c => c.GetTemperaturesAsync(null, "latest-hour"))
            .ReturnsAsync([new SmhiStationData("72420", "Stockholm A", 59.35, 18.06, 18.3, timestamp)]);

        _smhiClientMock
            .Setup(c => c.GetGustWindsAsync(null, "latest-hour"))
            .ReturnsAsync([new SmhiStationData("72420", "Stockholm A", 59.35, 18.06, 12.1, timestamp)]);

        var result = (await _sut.GetMeasurementsAsync(null, "latest-hour")).ToList();

        Assert.Single(result);
        Assert.Equal("72420", result[0].StationId);
        Assert.Equal(18.3, result[0].Temperature);
        Assert.Equal(12.1, result[0].GustWind);
    }

    [Fact]
    public async Task GetMeasurements_WhenStationHasTemperatureButNoWind_ReturnsNullGustWind()
    {
        var timestamp = DateTimeOffset.UtcNow;

        _smhiClientMock
            .Setup(c => c.GetTemperaturesAsync(null, "latest-hour"))
            .ReturnsAsync([new SmhiStationData("72420", "Stockholm A", 59.35, 18.06, 18.3, timestamp)]);

        _smhiClientMock
            .Setup(c => c.GetGustWindsAsync(null, "latest-hour"))
            .ReturnsAsync([]);

        var result = (await _sut.GetMeasurementsAsync(null, "latest-hour")).ToList();

        Assert.Single(result);
        Assert.Equal(18.3, result[0].Temperature);
        Assert.Null(result[0].GustWind);
    }
}

namespace SMHIWeatherAPI.Features.Measurements;

public static class MeasurementEndpoints
{
    private static readonly string[] ValidPeriods = ["latest-hour", "latest-day"];

    public static void MapMeasurementEndpoints(this WebApplication app)
    {
        app.MapGet("/measurements", GetMeasurements)
           .WithName("GetMeasurements")
           .RequireAuthorization();
    }

    private static async Task<IResult> GetMeasurements(
        IMeasurementService measurementService,
        int? stationId = null,
        string period = "latest-hour")
    {
        if (!ValidPeriods.Contains(period))
            return Results.BadRequest($"Invalid period. Valid values: {string.Join(", ", ValidPeriods)}");

        var result = await measurementService.GetMeasurementsAsync(stationId, period);
        return Results.Ok(result);
    }
}

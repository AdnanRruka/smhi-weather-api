namespace SMHIWeatherAPI.Features.Measurements.Dtos;

public class SmhiAllStationsResponse
{
    public List<SmhiStationWithValues> Station { get; set; } = [];
}

public class SmhiStationWithValues
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<SmhiValue> Value { get; set; } = [];
}

public class SmhiSingleStationResponse
{
    public SmhiStationInfo Station { get; set; } = new();
    public List<SmhiPosition> Position { get; set; } = [];
    public List<SmhiValue> Value { get; set; } = [];
}

public class SmhiStationInfo
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class SmhiPosition
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class SmhiValue
{
    public long Date { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Quality { get; set; } = string.Empty;
}

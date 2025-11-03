namespace ShopTARgv24.Core.Dto
{
    public class OpenWeatherDto
    {
        public string? CityName { get; set; }
    }

    public class OpenWeatherResponseDto
    {
        public CoordDto? coord { get; set; }
        public WeatherDto[]? weather { get; set; }
        public string? @base { get; set; }
        public MainDto? main { get; set; }
        public int? visibility { get; set; }
        public WindDto? wind { get; set; }
        public CloudsDto? clouds { get; set; }
        public int? dt { get; set; }
        public SysDto? sys { get; set; }
        public int? timezone { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public int? cod { get; set; }
    }

    public class CoordDto
    {
        public double? lon { get; set; }
        public double? lat { get; set; }
    }

    public class WeatherDto
    {
        public int? id { get; set; }
        public string? main { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }
    }

    public class MainDto
    {
        public double? temp { get; set; }
        public double? feels_like { get; set; }
        public double? temp_min { get; set; }
        public double? temp_max { get; set; }
        public int? pressure { get; set; }
        public int? humidity { get; set; }
    }

    public class WindDto
    {
        public double? speed { get; set; }
        public int? deg { get; set; }
    }

    public class CloudsDto
    {
        public int? all { get; set; }
    }

    public class SysDto
    {
        public int? type { get; set; }
        public int? id { get; set; }
        public string? country { get; set; }
        public int? sunrise { get; set; }
        public int? sunset { get; set; }
    }
}
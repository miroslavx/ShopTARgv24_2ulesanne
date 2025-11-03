namespace ShopTARgv24.Models.Weather
{
    public class OpenWeatherViewModel
    {
        public string? CityName { get; set; }
        public string? Country { get; set; }
        public double? Temperature { get; set; }
        public double? FeelsLike { get; set; }
        public double? TempMin { get; set; }
        public double? TempMax { get; set; }
        public int? Humidity { get; set; }
        public int? Pressure { get; set; }
        public string? Description { get; set; }
        public string? WeatherMain { get; set; }
        public string? Icon { get; set; }
        public double? WindSpeed { get; set; }
        public int? WindDeg { get; set; }
        public int? Cloudiness { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }

    public class OpenWeatherSearchViewModel
    {
        public string CityName { get; set; } = string.Empty;
    }
}
using System.Text.Json;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {
        public async Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto)
        {
            string accuApiKey = "your_api_key_here"; // Замените на ваш реальный API ключ
            string baseUrl = "http://dataservice.accuweather.com/locations/v1/cities/search";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Исправленный URL с правильными параметрами
                var response = await httpClient.GetAsync($"?apikey={accuApiKey}&q={dto.CityName}&details=true");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<AccuLocationWeatherResultDto>(jsonResponse);
                    return weatherData ?? new AccuLocationWeatherResultDto { CityName = dto.CityName };
                }
                else
                {
                    throw new Exception($"Error retrieving weather data: {response.StatusCode}");
                }
            }
        }
    }
}
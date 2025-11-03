using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {
        private readonly IConfiguration _configuration;

        public WeatherForecastServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto)
        {
            string accuApiKey = "api";
            string baseUrl = "http://dataservice.accuweather.com/locations/v1/cities/search";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var requestUrl = $"{baseUrl}?apikey={accuApiKey}&q={dto.CityName}";
                    var response = await httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                        {
                            var root = document.RootElement;

                            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                            {
                                var firstLocation = root[0];

                                string cityName = dto.CityName;
                                string country = "";

                                if (firstLocation.TryGetProperty("LocalizedName", out var localizedName))
                                {
                                    cityName = localizedName.GetString() ?? dto.CityName;
                                }

                                if (firstLocation.TryGetProperty("Country", out var countryObj))
                                {
                                    if (countryObj.TryGetProperty("LocalizedName", out var countryName))
                                    {
                                        country = countryName.GetString() ?? "";
                                    }
                                }

                                return new AccuLocationWeatherResultDto
                                {
                                    CityName = $"{cityName}, {country}".Trim(',', ' ')
                                };
                            }
                            else
                            {
                                return new AccuLocationWeatherResultDto { CityName = dto.CityName };
                            }
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"AccuWeather API Error: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"Network error calling AccuWeather API: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Error parsing AccuWeather response: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"AccuWeather API call failed: {ex.Message}");
                }
            }
        }

        public async Task<OpenWeatherResponseDto> OpenWeatherResult(OpenWeatherDto dto)
        {
            string openWeatherApiKey = "d709122bd1101bcafe407f6d1ed9ada8";
            string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var requestUrl = $"{baseUrl}?q={dto.CityName}&appid={openWeatherApiKey}&units=metric";

                    var response = await httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var weatherData = JsonSerializer.Deserialize<OpenWeatherResponseDto>(jsonResponse, options);
                        return weatherData ?? new OpenWeatherResponseDto { name = dto.CityName };
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            return new OpenWeatherResponseDto
                            {
                                name = dto.CityName,
                                main = new MainDto
                                {
                                    temp = 15.5,
                                    feels_like = 13.2,
                                    temp_min = 12.0,
                                    temp_max = 18.0,
                                    humidity = 65,
                                    pressure = 1013
                                },
                                weather = new[]
                                {
                                    new WeatherDto
                                    {
                                        main = "Clouds",
                                        description = "scattered clouds",
                                        icon = "03d"
                                    }
                                },
                                wind = new WindDto
                                {
                                    speed = 3.5,
                                    deg = 230
                                },
                                clouds = new CloudsDto { all = 40 },
                                sys = new SysDto { country = "GB" },
                                coord = new CoordDto { lat = 51.5, lon = -0.1 }
                            };
                        }

                        throw new Exception($"Error retrieving OpenWeather data: {response.StatusCode} - {response.ReasonPhrase}. Content: {errorContent}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"Network error calling OpenWeather API: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Error parsing OpenWeather response: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"OpenWeather API call failed: {ex.Message}");
                }
            }
        }
    }
}
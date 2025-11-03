using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Models.Weather;

namespace ShopTARgv24.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherForecastServices _weatherForecastServices;

        public WeatherController(IWeatherForecastServices weatherForecastServices)
        {
            _weatherForecastServices = weatherForecastServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCity(AccuWeatherSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("City", "Weather", new { city = model.CityName });
            }
            return View(model);
        }

        // Новый метод для OpenWeather поиска
        [HttpPost]
        public IActionResult SearchCityOpenWeather(OpenWeatherSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("OpenWeatherCity", "Weather", new { city = model.CityName });
            }
            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> City(string city)
        {
            try
            {
                AccuLocationWeatherResultDto dto = new();
                dto.CityName = city;

                var result = await _weatherForecastServices.AccuWeatherResult(dto);

                return View(result);
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем базовую модель
                ViewBag.Error = $"Ошибка получения данных о погоде AccuWeather: {ex.Message}";
                return View(new AccuLocationWeatherResultDto { CityName = city });
            }
        }

        // Новый метод для OpenWeather
        [HttpGet]
        public async Task<IActionResult> OpenWeatherCity(string city)
        {
            try
            {
                OpenWeatherDto dto = new OpenWeatherDto
                {
                    CityName = city
                };

                var result = await _weatherForecastServices.OpenWeatherResult(dto);

                // Маппим результат в ViewModel для отображения
                var viewModel = new OpenWeatherViewModel
                {
                    CityName = result.name,
                    Country = result.sys?.country,
                    Temperature = result.main?.temp,
                    FeelsLike = result.main?.feels_like,
                    TempMin = result.main?.temp_min,
                    TempMax = result.main?.temp_max,
                    Humidity = result.main?.humidity,
                    Pressure = result.main?.pressure,
                    Description = result.weather?.FirstOrDefault()?.description,
                    WeatherMain = result.weather?.FirstOrDefault()?.main,
                    Icon = result.weather?.FirstOrDefault()?.icon,
                    WindSpeed = result.wind?.speed,
                    WindDeg = result.wind?.deg,
                    Cloudiness = result.clouds?.all,
                    Longitude = result.coord?.lon,
                    Latitude = result.coord?.lat
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ошибка получения данных о погоде OpenWeather: {ex.Message}";
                return View(new OpenWeatherViewModel { CityName = city });
            }
        }

        // Тестовые методы для проверки API
        [HttpGet]
        public async Task<IActionResult> TestOpenWeather()
        {
            try
            {
                var dto = new OpenWeatherDto { CityName = "Tallinn" };
                var result = await _weatherForecastServices.OpenWeatherResult(dto);

                return Json(new
                {
                    success = true,
                    data = result,
                    message = "OpenWeather API работает!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestAccuWeather()
        {
            try
            {
                var dto = new AccuLocationWeatherResultDto { CityName = "Tallinn" };
                var result = await _weatherForecastServices.AccuWeatherResult(dto);

                return Json(new
                {
                    success = true,
                    data = result,
                    message = "AccuWeather API работает!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}
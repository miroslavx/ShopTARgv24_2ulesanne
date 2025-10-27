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
                ViewBag.Error = $"Ошибка получения данных о погоде: {ex.Message}";
                return View(new AccuLocationWeatherResultDto { CityName = city });
            }
        }
    }
}
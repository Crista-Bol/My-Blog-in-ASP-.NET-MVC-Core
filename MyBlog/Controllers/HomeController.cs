using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly WeatherClient weatherClient;


        public HomeController(ILogger<HomeController> logger,WeatherClient weatherClient)
        {
            _logger = logger;
            this.weatherClient = weatherClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult AboutMe()
        {
            return View();
        }
                
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("weather/{city}")]
        public async Task<WeatherForecast> getCurrentWeather(string city)
        {
            Console.WriteLine(city);
            var weatherForecast = await weatherClient.getCurrentWeatherAsync(city);
            return new WeatherForecast
            {
                Date = DateTimeOffset.FromUnixTimeSeconds(weatherForecast.dt).DateTime,
                Summary = weatherForecast.weather[0].description,
                TemperatureC=(int)weatherForecast.main.temp
            };
        }
    }
}

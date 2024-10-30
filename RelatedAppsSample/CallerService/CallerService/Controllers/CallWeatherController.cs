using Microsoft.AspNetCore.Mvc;

namespace CallerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallWeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CallWeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            // Call the Weather API from the first service
            var response = await _httpClient.GetStringAsync("http://weather-service/WeatherForecast");
            return Ok(response);
        }
    }
}
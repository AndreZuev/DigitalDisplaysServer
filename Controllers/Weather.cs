using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DigitalProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{


    public WeatherController()
    {

    }

    /// <summary>
    /// Fetch weather information about Butte.
    /// </summary>
    /// <response code="200">Fetched weather data successfully.</response>
    /// <response code="400">The request was malformed.</response>
    [HttpGet(Name = "weather")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(WeatherModel), 200)]
    public async Task<IActionResult> Get()
    {
        WeatherModel weather = new WeatherModel();
        using (HttpClient client = new HttpClient())
        {
            string Apikey = "8d22ce60497679c6e7120a11e3fb228b";
            string longitude = "-112.536508";
            string latitude = "46.0131505";

            try
            {
                HttpResponseMessage response = await client.GetAsync(
                    String.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}", latitude, longitude, Apikey));
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic? jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                weather.Condtion = jsonResponse["weather"][0]["main"];
                weather.TemperatureF = jsonResponse["main"]["temp"];
                weather.TemperatureF = Math.Round(1.8 * (weather.TemperatureF - 273.0) + 32.0,2, MidpointRounding.AwayFromZero);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

        }

        return Ok(weather);
    }
}

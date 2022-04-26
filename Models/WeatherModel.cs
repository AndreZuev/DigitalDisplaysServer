namespace DigitalProject;

public class WeatherModel
{
    public string Time { get; set; } = DateTime.Now.ToString("h:mm:ss tt");
    public string Condtion  { get; set; } = "Unknown";
    public double TemperatureF { get; set; } = 32;
}
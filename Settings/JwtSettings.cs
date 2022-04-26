namespace DigitalProject.Settings;

public interface IJwtSettings {
    string? SecretKey { get; set; }
    int MinutesExpire { get; set; }
}

public class JwtSettings : IJwtSettings
{
    public string? SecretKey { get; set; }
    public int MinutesExpire { get; set; }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string OpenWeatherMapApiKey;
    private readonly string CityName;
    private readonly string Units;

    private readonly string OpenWeatherMapApiUrl;

    public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        OpenWeatherMapApiKey = configuration["ApiKeys:OpenWeatherMap"]!;
        OpenWeatherMapApiUrl = configuration["ApiKeys:OpenWeatherMapUrl"]!;
        CityName = configuration["city"]!;
        Units = configuration["units"]!;
    }

    public async Task<WeatherResponse> GetWeatherData()
    {
        using var httpClient = _httpClientFactory.CreateClient();

        // Using String.Format to replace placeholders
        var openWeatherMapUrl = string.Format(OpenWeatherMapApiUrl, CityName, OpenWeatherMapApiKey, Units);

        var response = await httpClient.GetAsync(openWeatherMapUrl);

        if (!response.IsSuccessStatusCode)
        {
            // Handle non-success status code
            throw new HttpRequestException($"Failed to retrieve weather data. Status code: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);

        return weatherResponse ?? throw new WeatherServiceException("Weather data is null.");
    }
}

public class WeatherResponse
{
    public MainInfo? Main { get; set; }
}

public class MainInfo
{
    public float Temp { get; set; }
}

public class WeatherServiceException : Exception
{
    public WeatherServiceException(string message) : base(message)
    {
    }
}

public interface IWeatherService
{
    Task<WeatherResponse> GetWeatherData();
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.Configuration;
using BikeDashboard.DTO;
using BikeDashboard.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BikeDashboard.Services;

public class OpenWeatherMapClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenWeatherMapClient> _logger;
    private readonly string _weatherServiceApiKey;
    private readonly string _tempUnit = "metric";
    private readonly int _numberOfForecastRecords = 4; // 3 hours between forecasts

    public OpenWeatherMapClient(HttpClient httpClient, IOptions<WeatherServiceSettings> weatherServiceSettings, ILogger<OpenWeatherMapClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _weatherServiceApiKey = weatherServiceSettings.Value.WeatherServiceApiKey;
        _httpClient.BaseAddress = weatherServiceSettings.Value.ApiBaseAddress;
    }

    public async Task<WeatherReportDTO> GetDailyForeCastAsync(StationCoordinates coordinates)
    {
        var response = await _httpClient.GetAsync($"?lat={coordinates.Latitude}" +
                                                  $"&lon={coordinates.Longitude}" +
                                                  $"&APPID={_weatherServiceApiKey}" +
                                                  $"&units={_tempUnit}" +
                                                  $"&cnt={_numberOfForecastRecords}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Error when getting weather forecast from {_httpClient.BaseAddress}");
            throw new NotImplementedException($"Could not find any weather data, {_httpClient.BaseAddress} returned status code {response.StatusCode}");
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<WeatherReportDTO>(content);
    }
}
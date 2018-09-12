using System;
using System.Threading.Tasks;
using BikeDashboard.Models;

namespace BikeDashboard.Services
{
    public interface IWeatherService
    {
		Task<string> GetDailyForeCastAsync(StationCoordinates coordinates);
	}
}

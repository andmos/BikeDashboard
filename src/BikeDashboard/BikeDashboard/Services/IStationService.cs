using System;
using System.Threading.Tasks;
using BikeDashboard.Models;

namespace BikeDashboard.Services
{
	public interface IStationService
    {
		Task<FavoriteStation> GetFavoriteStation();
    }
}

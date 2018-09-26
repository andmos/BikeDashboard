using System;
using BikeDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using BikeshareClient.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BikeDashboard.Controllers
{
	[Route("api/[controller]")]
    public class StationsController
    {
		private readonly IStationService _stationService;

		public StationsController(IStationService stationService)
        {
			_stationService = stationService;
		}

		[HttpGet]
		public async Task<IEnumerable<Station>> GetAsync()
		{
			return await _stationService.GetAllAvailableStations();
		}
    }
}

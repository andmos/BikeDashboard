using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BikeDashboard.Pages
{
    public class IndexModel : PageModel
    {
		private readonly IStationService _stationService; 

		public IndexModel(IStationService stationService)
		{
			_stationService = stationService; 
		}

		[BindProperty]
		public FavoriteStation FavoriteStation { get; set; }
      
		public async Task OnGetAsync()
        {
			FavoriteStation = await _stationService.GetFavoriteStation(); 
        }
    }
}

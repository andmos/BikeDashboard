using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.Models;
using Newtonsoft.Json;
using Xunit;
using System.Linq;

namespace TestBikedashboard.Controllers
{
    public class TestStationsController : TestBase
    {

        private readonly HttpClient _client;


        public TestStationsController() 
        {
            _client = Factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsStations() 
        {
            var response = await _client.GetAsync("/api/Stations");

            var content = await response.Content.ReadAsStringAsync();
            var stations = JsonConvert.DeserializeObject<IEnumerable<FavoriteStation>>(content);

            Assert.True(stations.Count() > 1);
        }
    }
}

using System.Threading.Tasks;
using Xunit;
using BikeDashboard;
using System.Net.Http;
using HtmlAgilityPack;
using System.Linq;

namespace TestBikedashboard.Pages
{
    public class TestIndexPage : TestBase
    {

        private readonly HttpClient _client;

        public TestIndexPage()
        {
            _client = Factory.CreateClient();
        }

        [Fact]
        public async Task GetAsync_GivenCorrectConfiguration_ReturnsFavoriteStation()
        {
            var response = await _client.GetAsync("/");

            var content = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var htmlElement = doc.DocumentNode.Descendants("h1");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.Collection(htmlElement, s => s.InnerText.Contains(DefaultStation));
        }

        [Fact]
        public async Task GetAsync_GivenStationQueryString_ReturnsCorrectStation()
        {
            var response = await _client.GetAsync("/?stationName=skansen");

            var content = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var htmlElement = doc.DocumentNode.Descendants("h1");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.Collection(htmlElement, s => s.InnerText.Contains("Skansen"));
        }

        [Fact]
        public async Task GetAsync_GivenInvalidStationQuery_ReturnsDefaultStation()
        {
            var response = await _client.GetAsync("/?stationName=skanseeeen");

            var content = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var htmlElement = doc.DocumentNode.Descendants("h1");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.Collection(htmlElement, s => s.InnerText.Contains(DefaultStation));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public RegionsController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {

            List<RegionDTO> response= new List<RegionDTO>();

            try
            {
                //Get All Regions from Web API
                var client = clientFactory.CreateClient();

                var httpResponeMessage = await client.GetAsync("https://localhost:7118/api/regions");

                httpResponeMessage.EnsureSuccessStatusCode();

                 response.AddRange( await httpResponeMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

            }
            catch (Exception ex)
            {
                //Log exception
                
            }

            return View(response);
        }
    }
}

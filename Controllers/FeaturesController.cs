using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OboEnergyAPI.Models;

namespace Api.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Route("[controller]")]
    public class FeaturesController : ControllerBase
    {
        private readonly ILogger<FeaturesController> _logger;
        private readonly CosmosDbClient cosmosDbClient;

        public FeaturesController(ILogger<FeaturesController> logger, CosmosDbClient cosmosDbClient)
        {
            _logger = logger;
            this.cosmosDbClient = cosmosDbClient;
        }

        [HttpGet(Name = "features"), Authorize]
        public async Task<IActionResult> Get()
        {
            Feature[] features;

            //var client = new CosmosDbClient();
            var featuresList = await cosmosDbClient.GetFeatures();
            features = featuresList.ToArray();

            return Ok(new { features });
        }

        [HttpPost(Name = "features"), Authorize]
        public async Task<IActionResult> Post([FromBody] Feature feature)
        {
            //var client = new CosmosDbClient();
            await cosmosDbClient.UpdateFeature(feature.Id, feature.Name, feature.Roles, feature.IsEnabled);

            return NoContent();
        }
    }
}
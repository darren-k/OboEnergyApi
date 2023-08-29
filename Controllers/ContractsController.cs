using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OboEnergyAPI.Models;

namespace Api.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Route("[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly ILogger<ContractsController> _logger;
        private readonly CosmosDbClient cosmosDbClient;

        public ContractsController(ILogger<ContractsController> logger, CosmosDbClient cosmosDbClient)
        {
            _logger = logger;
            this.cosmosDbClient = cosmosDbClient;
        }

        [HttpGet(Name = "contracts"), Authorize]
        public async Task<IActionResult> Get()
        {
            //// Simulated contract data
            //var contracts = new[]
            //{
            //    new Models.Contract { StartDate = "2023-08-01", EndDate = "2023-12-31", CompanyName = "Acme Corporation", Id = "123456" },
            //    new Models.Contract { StartDate = "2023-09-15", EndDate = "2024-09-14", CompanyName = "Tech Innovators Ltd.", Id = "789012" },
            //    new Models.Contract { StartDate = "2023-07-01", EndDate = "2024-06-30", CompanyName = "Global Logistics Solutions", Id = "345678" },
            //    new Models.Contract { StartDate = "2023-10-01", EndDate = "2024-09-30", CompanyName = "Innovate Enterprises", Id = "901234" },
            //    new Models.Contract { StartDate = "2023-08-15", EndDate = "2023-11-15", CompanyName = "EcoTech Solutions", Id = "567890" },
            //    new Models.Contract { StartDate = "2023-09-01", EndDate = "2024-08-31", CompanyName = "Financial Services Inc.", Id = "234567" },
            //    new Models.Contract { StartDate = "2023-11-01", EndDate = "2024-10-31", CompanyName = "Healthcare Innovations", Id = "890123" },
            //    new Models.Contract { StartDate = "2023-10-15", EndDate = "2024-10-14", CompanyName = "Retail Solutions Ltd.", Id = "456789" }
            //};

            Contract[] contracts;

            //var client = new CosmosDbClient();
            var contractsList = await cosmosDbClient.GetContracts();
            contracts = contractsList.ToArray();

            return Ok(new { contracts });
        }

        [HttpPost(Name = "contracts"), Authorize]
        public async Task<IActionResult> Post([FromBody] Contract contract)
        {
            //var client = new CosmosDbClient();
            await cosmosDbClient.UpdateContractDates(contract.Id, contract.CompanyName, contract.StartDate, contract.EndDate);

            return NoContent();
        }
    }
}
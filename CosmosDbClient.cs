using Microsoft.Azure.Cosmos;
using OboEnergyAPI.Models;

namespace Api
{
    public class CosmosDbClient
    {
        private readonly CosmosClient _client;

        public CosmosDbClient(CosmosClient cosmosClient)
        {
            _client = cosmosClient;
        }

        public async Task<List<Contract>> GetContracts()
        {
            Container container = GetCosmosContainer("contracts");

            // Query the container 
            var sqlQueryText = "SELECT * FROM contracts";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Contract> queryResultSet = container.GetItemQueryIterator<Contract>(queryDefinition);

            List<Contract> contracts = new List<Contract>();
            while (queryResultSet.HasMoreResults)
            {
                FeedResponse<Contract> response = await queryResultSet.ReadNextAsync();
                foreach (Contract c in response)
                {
                    contracts.Add(c);
                }
            }

            return contracts;
        }

        public async Task<List<Feature>> GetFeatures()
        {
            Container container = GetCosmosContainer("features");

            // Query the container 
            var sqlQueryText = "SELECT * FROM features";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Feature> queryResultSet = container.GetItemQueryIterator<Feature>(queryDefinition);

            List<Feature> features = new List<Feature>();
            while (queryResultSet.HasMoreResults)
            {
                FeedResponse<Feature> response = await queryResultSet.ReadNextAsync();
                foreach (Feature f in response)
                {
                    features.Add(f);
                }
            }

            return features;
        }

        public async Task UpdateContractDates(string id, string companyName, string startDate, string endDate)
        {
            var updatedContract = new
            {
                companyName,
                id,
                startDate,
                endDate,
            };

            await GetCosmosContainer("contracts").ReplaceItemAsync(
                updatedContract, id, new PartitionKey(updatedContract.id));
        }
        public async Task UpdateFeature(string id, string name, string roles, bool isEnabled)
        {
            var updatedFeature = new
            {
                name,
                id,
                roles,
                isEnabled,
            };

            await GetCosmosContainer("features").ReplaceItemAsync(
                updatedFeature, id, new PartitionKey(updatedFeature.id));
        }

        private Container GetCosmosContainer(string containerName)
        {
            // CosmosClient client = new("AccountEndpoint=https://project1-cosmos.documents.azure.com:443/;AccountKey=hnaWhcJCqPZjliXDZiY6r2f0tEhBhJCc4rir2WSinAHUB4ITzwCNUKWJtoEwFSImNaZYoCMQ2DIgACDbizLqrw==");
            
            Database database = _client.GetDatabase("project1db");
            return database.GetContainer(containerName);
        }

    }
}

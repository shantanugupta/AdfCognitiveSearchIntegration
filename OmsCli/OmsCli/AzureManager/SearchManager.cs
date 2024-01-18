using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using OmsCli.AdfManager;
using OmsCli.Model;
namespace OmsCli.AzureManager
{
    public class SearchManager
    {
        private static readonly string searchServiceEndpoint;
        private static readonly string apiKey;

        static SearchManager() {
            Settings setting = new();
            searchServiceEndpoint = KeyVaultManager.GetSecret(setting.SearchUrlSecret); ;
            apiKey = KeyVaultManager.GetSecret(setting.SearchSecret);
        }

        public static async void Search(string indexName = "products-index")
        {
            // Create a SearchClient
            Uri serviceEndpointUri = new(searchServiceEndpoint);
            AzureKeyCredential credential = new(apiKey);
            SearchClient client = new(serviceEndpointUri, indexName, credential);
           
            SearchResults<Product> searchResponse = await client.SearchAsync<Product>("day");
            await foreach (SearchResult<Product> result in searchResponse.GetResultsAsync())
            {
                Product p = result.Document;
                Console.WriteLine($"Name: {p.Name}, Price: {p.Price}, Date: {p.Date}");
            }
        }
    }
}

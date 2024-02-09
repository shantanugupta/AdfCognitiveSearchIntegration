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

        public static async void Search(string indexName = "products-index", string searchText= "abbott")
        {
            try { 
            // Create a SearchClient
            Uri serviceEndpointUri = new(searchServiceEndpoint);
            AzureKeyCredential credential = new(apiKey);
            SearchClient client = new(serviceEndpointUri, indexName, credential);

            Console.WriteLine($"Searching {searchText} in index - {indexName}");
            SearchResults<ProductSearchModel> searchResponse = await client.SearchAsync<ProductSearchModel>(searchText);
            await foreach (SearchResult<ProductSearchModel> result in searchResponse.GetResultsAsync())
            {
                    ProductSearchModel p = result.Document;
                Console.WriteLine($"Name: {p.Name}, Price: {p.Price}, Date: {p.Date}");
            }
            }
            catch(Exception ex ) { 
                Console.WriteLine($"Failed to search: {ex.Message}");
            }
        }
    }
}

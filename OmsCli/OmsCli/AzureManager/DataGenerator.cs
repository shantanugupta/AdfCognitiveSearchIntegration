using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using CsvHelper;
using Faker;
using Newtonsoft.Json;
using OmsCli.AdfManager;
using OmsCli.Model;


namespace OmsCli.AzureManager
{
    /// <summary>
    /// This class depends on azure storage account secret on which generated files will be uploaded.
    /// </summary>
    public class DataGenerator
    {
        private readonly BlobServiceClient storageAccount;
        private readonly BlobContainerClient container;
        private static int MaxCategories { get; set; }
        private static int MaxProducts { get; set; }
        private static int MaxOrders { get; set; }

        /// <summary>
        /// Max no of categories to create
        /// </summary>
        /// <param name="seed"></param>
        public DataGenerator(int seed)
        {
            Settings setting = new Settings();
            var blobConnectionString = KeyVaultManager.GetSecret(setting.OmsblobstoreConnectionStringSecret);

            storageAccount = new BlobServiceClient(blobConnectionString);
            container = storageAccount.GetBlobContainerClient("inputdata");
            MaxCategories = seed;
            MaxProducts = MaxCategories * 50;
            MaxOrders = MaxProducts * 5;

            Console.WriteLine($"MaxCategories: {MaxCategories}, MaxProducts: {MaxProducts}, MaxOrders: {MaxOrders}");
        }
        public void Generate()
        {
            // Generate data for Categories table
            List<Category> categoriesData = GenerateCategoriesData();
            WriteToCsv(categoriesData.OfType<IBaseModel>().ToList(), "categories.csv");
            Console.WriteLine("CSV creation completed for categories");

            // Generate data for Products table
            List<Product> productsData = GenerateProductsData(categoriesData);
            WriteToCsv(productsData.OfType<IBaseModel>().ToList(), "products.csv");
            Console.WriteLine("CSV creation completed for products");

            // Generate data for Orders table
            List<Order> ordersData = GenerateOrdersData(productsData);
            WriteToCsv(ordersData.OfType<IBaseModel>().ToList(), "orders.csv");
            Console.WriteLine("CSV creation completed for orders");


            categoriesData = GenerateCategoriesData();
            WriteToJson(categoriesData.OfType<IBaseModel>().ToList(), "categories.json");
            Console.WriteLine("JSON creation completed for categories");

            productsData = GenerateProductsData(categoriesData);
            WriteToJson(productsData.OfType<IBaseModel>().ToList(), "products.json");
            Console.WriteLine("JSON creation completed for products");

            ordersData = GenerateOrdersData(productsData);
            WriteToJson(ordersData.OfType<IBaseModel>().ToList(), "orders.json");
            Console.WriteLine("JSON creation completed for orders");
        }

        /// <summary>
        /// Generates random categories
        /// </summary>
        /// <returns></returns>
        private static List<Category> GenerateCategoriesData()
        {
            var items = MaxCategories;
            var categories = new List<Category>();
            for (int i = 0; i < items; i++)
            {
                var category = new Category
                {
                    Id = i + 1,
                    Name = Faker.Company.Name()
                };
                categories.Add(category);

                if (i % (items / 10) == 0)
                    Console.WriteLine($"Generated {i} of {items} categories");
            }
            return categories;
        }

        /// <summary>
        /// Generate random products and uses categories generated to be mapped with the products.
        /// </summary>
        /// <param name="categories">These categories will be assigned to products.</param>
        /// <returns></returns>
        private static List<Product> GenerateProductsData(List<Category> categories)
        {
            var items = MaxProducts;
            var products = new List<Product>();
            for (int i = 0; i < items; i++)
            {
                var randomId = Faker.RandomNumber.Next(0, MaxCategories - 1);
                var product = new Product
                {
                    Id = i + 1,
                    Name = Faker.Internet.DomainWord(),
                    Category = categories.ElementAt(randomId).Name,
                    Price = Faker.Finance.Coupon(),
                    Description = Faker.Lorem.Sentence(),
                    ImageUrl = Faker.Internet.Url(),
                    Date = Faker.Finance.Maturity()
                };
                products.Add(product);

                if (i % (items / 10) == 0)
                    Console.WriteLine($"Generated {i} of {items} products");
            }
            return products;
        }

        /// <summary>
        /// Multiple order records are generated. Order number would be duplicated so as to create many to many mapping between products and orders
        /// </summary>
        /// <param name="products">Will be used to map order to this product</param>
        /// <returns></returns>
        private static List<Order> GenerateOrdersData(List<Product> products)
        {
            var items = MaxOrders;
            var orders = new List<Order>();
            for (int i = 0; i < items; i++)
            {
                var randomId = Faker.RandomNumber.Next(0, MaxProducts - 1);
                var order = new Order
                {
                    OrderNumber = Faker.RandomNumber.Next(1, MaxProducts),
                    Date = Faker.Finance.Maturity(),
                    Name = Faker.Name.FullName(),
                    ProductName = products[randomId].Name
                };
                orders.Add(order);

                if (i % (items / 10) == 0)
                    Console.WriteLine($"Generated {i} of {items} orders.");
            }
            return orders;
        }

        /// <summary>
        /// Writes the data into CSV and uploads into blob storage directly.
        /// </summary>
        /// <param name="data">Data to be converted into csv</param>
        /// <param name="filename"></param>
        private async void WriteToCsv(List<IBaseModel> data, string filename)
        {
            BlockBlobClient blockBlob = container.GetBlockBlobClient("CSV" + "/" + filename);

            var httpHeader = new BlobHttpHeaders
            {
                ContentType = "text/csv"
            };

            var folderPath = "./generated_files/";
            Directory.CreateDirectory(folderPath);
            using (var csvfile = new StreamWriter(Path.Combine(folderPath, filename)))
            {
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i].GetLine();

                    if (i == 0)
                        csvfile.WriteLine(data[i].GetHeader());

                    csvfile.WriteLine(item);
                    csvfile.Flush();
                }
            }
            FileStream fileStream = File.OpenRead(Path.Combine(folderPath, filename));
            await blockBlob.UploadAsync(fileStream, httpHeader);
            fileStream.Close();
        }

        /// <summary>
        /// This function writes data into json format and saves the file in blob storage on azure account.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        private async void WriteToJson(List<IBaseModel> data, string filename)
        {
            var httpHeader = new BlobHttpHeaders
            {
                ContentType = "application/json"
            };
            BlockBlobClient blockBlob = container.GetBlockBlobClient("JSON" + "/" + filename);
            var folderPath = "./generated_files/";
            var filePath = Path.Combine(folderPath, filename);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));

            FileStream fileStream = File.OpenRead(Path.Combine(folderPath, filename));
            await blockBlob.UploadAsync(fileStream, httpHeader);
            fileStream.Close();
        }
    }
}

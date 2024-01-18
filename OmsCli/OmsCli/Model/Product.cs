using Azure.Search.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmsCli.Model
{
    public class Product : IBaseModel
    {
        [JsonPropertyName("product_id")]
        public int Id { get; set; }


        [JsonPropertyName("product_name")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Name { get; set; }


        [JsonPropertyName("category_id")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Category { get; set; }


        [JsonPropertyName("price")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public Decimal Price { get; set; }


        [JsonPropertyName("description")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Description { get; set; }

        [JsonPropertyName("image_url")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string ImageUrl { get; set; }

        [JsonPropertyName("date_added")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTime Date { get; set; }

        public string GetLine()
        {
            string delimiter = Constants.DELIMITER;
            return $"{Id}{delimiter}{Name}{delimiter}{Category}{delimiter}{Price}{delimiter}{Description}{delimiter}{ImageUrl}{delimiter}{Date.ToString("yyyy-MM-dd")}";
        }

        public string GetHeader() {
            string delimiter = Constants.DELIMITER;
            return $"product_id{delimiter}product_name{delimiter}category_name{delimiter}price{delimiter}description{delimiter}image_url{delimiter}date_added";
        }
    }
}

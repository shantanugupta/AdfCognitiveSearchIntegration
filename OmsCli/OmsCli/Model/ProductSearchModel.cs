using Azure.Search.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmsCli.Model
{
    public class ProductSearchModel
    {
        [JsonPropertyName("product_id")]
        public string Id { get; set; }


        [JsonPropertyName("product_name")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Name { get; set; }


        [JsonPropertyName("category_id")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public int Category { get; set; }


        [JsonPropertyName("price")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Price { get; set; }


        [JsonPropertyName("description")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Description { get; set; }

        [JsonPropertyName("image_url")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string ImageUrl { get; set; }

        [JsonPropertyName("date_added")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTime Date { get; set; }
    }
}

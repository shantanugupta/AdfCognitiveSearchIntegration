using Azure.Search.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmsCli.Model
{
    public class Order : IBaseModel
    {
        [JsonPropertyName("order_number")]
        public int OrderNumber { get; set; }

        [JsonPropertyName("customer_name")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Name { get; set; }

        [JsonPropertyName("order_date")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTime Date { get; set; }

        [JsonPropertyName("product_name")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string ProductName { get; set; }

        public string GetLine()
        {
            string delimiter = Constants.DELIMITER;
            return $"{OrderNumber}{delimiter}{Name}{delimiter}{Date.ToString("yyyy-MM-dd")}{delimiter}{ProductName}";
        }

        public string GetHeader()
        {
            string delimiter = Constants.DELIMITER;
            return $"order_number{delimiter}customer_name{delimiter}order_date{delimiter}product_name";
        }
    }
}

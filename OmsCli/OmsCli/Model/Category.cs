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
    public class Category : IBaseModel
    {
        [JsonPropertyName("category_id")]
        public int Id { get; set; }


        [JsonPropertyName("category_name")]
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Name { get; set; }

        public string GetLine()
        {
            string delimiter = Constants.DELIMITER;
            return $"{Id}{delimiter}{Name}";
        }

        public string GetHeader()
        {
            string delimiter = Constants.DELIMITER;
            return $"category_id{delimiter}category_name";
        }
    }
}

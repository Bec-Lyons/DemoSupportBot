using Newtonsoft.Json;

namespace AzureSearchBot.Model
{
    /// <summary>
    /// TODO: Update with your search model 
    /// </summary>
    //Data model for Azure search result 
    public class SearchResult
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public Value[] value { get; set; }
    }

    //Data model for fetching facets
    public class FacetResult
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("@search.facets")]
        public SearchFacets searchfacets { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        [JsonProperty("@search.score")]
        public float searchscore { get; set; }
        public string ArticleID { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleCategory { get; set; }
        public string ArticleBody { get; set; }
        public string ArticleIMG { get; set; }
        public string ArticleURL { get; set; }
        public string ArticleTopics { get; set; }

    }

    public class SearchFacets
    {
        [JsonProperty("ArticleCategory@odata.type")]
        public string ArticleCategoryodatatype { get; set; }
        public ArticleCategory[] ArticleCategory { get; set; }
    }

    public class ArticleCategory
    {
        public int count { get; set; }
        public string value { get; set; }
    }
}
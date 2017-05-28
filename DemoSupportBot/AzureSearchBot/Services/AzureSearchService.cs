using System.Net.Http;
using System.Web.Configuration;
using System.Threading.Tasks;
using AzureSearchBot.Model;
using Newtonsoft.Json;
using System;

namespace AzureSearchBot.Services
{
    /// <summary>
    /// Azure Search is a cloud search-as-a-service solution that delegates server and infrastructure management to Microsoft, 
    /// leaving you with a ready-to-use service that you can populate with your data and then use to add search to your web or mobile application. 
    /// Azure Search allows you to easily add a robust search experience to your applications using 
    /// a simple REST API or .NET SDK without managing search infrastructure or becoming an expert in search.
    /// 
    /// To set up: 
    /// 1. Provision service --> from Azure Portal or ARM API
    /// 2. Create index --> index is like a database table that holds your data and can accept search queries
    /// 3. Index Data --> Upload content 
    /// 4. SEARCH --> This code issues search queries to your service endpoint using simple HTTP requests with the .NET SDK
    /// </summary>
    /// 

    [Serializable]
    public class AzureSearchService
    {
        private static readonly string QueryString = $"https://{WebConfigurationManager.AppSettings["SearchName"]}.search.windows.net/indexes/{WebConfigurationManager.AppSettings["IndexName"]}/docs?api-key={WebConfigurationManager.AppSettings["SearchKey"]}&api-version=2015-02-28&";

        //Search Data for name 
        public async Task<SearchResult> SearchByName(string name)
        {
            using (var httpClient = new HttpClient())
            {
                string nameQuey = $"{QueryString}search={name}";
                string response = await httpClient.GetStringAsync(nameQuey);
                return JsonConvert.DeserializeObject<SearchResult>(response);
            }
        }

         
        public async Task<FacetResult> FetchFacets()
        {
            using (var httpClient = new HttpClient())
            {
                string facetQuey = $"{QueryString}facet=ArticleCategory";
                string response = await httpClient.GetStringAsync(facetQuey);
                return JsonConvert.DeserializeObject<FacetResult>(response);
            }
        }

        //filter data by category
        public async Task<SearchResult> SearchByCategory(string cat)
        {
            using (var httpClient = new HttpClient())
            {
                string nameQuey = $"{QueryString}$filter=ArticleCategory eq '{cat}'";
                string response = await httpClient.GetStringAsync(nameQuey);
                return JsonConvert.DeserializeObject<SearchResult>(response);
            }
        }
    }
}
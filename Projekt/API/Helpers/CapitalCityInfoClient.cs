using Newtonsoft.Json;
using Projekt.API.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Projekt.API.Helpers
{
    public class CapitalCityInfoClient
    {
        public const string Url = "https://restcountries.com/v2/name/";


        public async Task<IEnumerable<CapitalCityModel>> FetchCapitalCityDataAsync()
        {
            var client = new RestClient($"https://restcountries.com/v2");
            //var request = new RestRequest($"/{countryName}", Method.Get);
            var request = new RestRequest("/all", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                JsonNumberHandling.WriteAsString
            };
            var response = await client.ExecuteAsync(request);

            try
            {

                var result =
                           Newtonsoft.Json.JsonConvert.DeserializeObject<CapitalCityModel[]>(response.Content);

                return result;
            }
            catch (Exception ex)
            {
                var result =
                           Newtonsoft.Json.JsonConvert.DeserializeObject<CapitalCityModel[]>(response.Content);
                return result;
            }
        }
    }
}

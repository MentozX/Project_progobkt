using Projekt.API.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.API.Helpers
{
    public class OpenWeatherClient
    {
        public const string Url = "https://api.openweathermap.org";
        public string ApiKey { get; set; }
        public string AppId { get; set; }

        public OpenWeatherClient(string apiKey, string appId)
        {
            this.ApiKey = apiKey;
            this.AppId = appId;
        }

        public async Task<WeatherModel> FetchWeatherDataAsync(string lat = null, string lon = null)
        {
            var client = new RestClient(Url);
            var request = new RestRequest("/data/2.5/onecall", Method.Get);
            request.AddParameter("lat", lat);
            request.AddParameter("lon", lon);
            request.AddParameter("exclude", "hourly,daily");
            request.AddParameter("appid", AppId);
            var response = await client.ExecuteAsync<WeatherModel>(request);

            try
            {
                var result =
                          Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(response.Content);

                return result;
            }
            catch (Exception ex)
            {
                var result =
                          Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(response.Content);

                return result;
            }
        }

        public async Task<CityInfoModel> FetchCityData(string cityName)
        {
            var client = new RestClient(Url);
            var request = new RestRequest("/geo/1.0/direct", Method.Get);
            request.AddParameter("q", cityName);
            request.AddParameter("limit", 1);
            request.AddParameter("appid", AppId);
            var response = await client.ExecuteAsync<CityInfoModel>(request);
            try
            {
                var result =
                          Newtonsoft.Json.JsonConvert.DeserializeObject<CityInfoModel[]>(response.Content);

                return result[0];
            }
            catch (Exception ex)
            {
                var result =
                          Newtonsoft.Json.JsonConvert.DeserializeObject<CityInfoModel[]>(response.Content);

                return result[0];
            }
        }
    }
}

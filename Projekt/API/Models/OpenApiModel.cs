using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.API.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Alert
    {
        public string sender_name { get; set; }
        public string @event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
    }

    public class CurrentWeather
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public double wind_gust { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class Minutely
    {
        public int dt { get; set; }
        public int precipitation { get; set; }
    }

    public class WeatherModel
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public CurrentWeather current { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class LocalNames
    {
        public string cs { get; set; }
        public string hu { get; set; }
        public string eu { get; set; }
        public string pt { get; set; }
        public string be { get; set; }
        public string sr { get; set; }
        public string ja { get; set; }
        public string es { get; set; }
        public string it { get; set; }
        public string fi { get; set; }
        public string fa { get; set; }
        public string el { get; set; }
        public string en { get; set; }
        public string de { get; set; }
        public string uk { get; set; }
        public string pl { get; set; }
        public string nl { get; set; }
        public string ru { get; set; }
        public string eo { get; set; }
        public string sk { get; set; }
        public string mt { get; set; }
        public string lv { get; set; }
        public string la { get; set; }
        public string zh { get; set; }
        public string sl { get; set; }
        public string hr { get; set; }
        public string ar { get; set; }
        public string lt { get; set; }
        public string ro { get; set; }
        public string fr { get; set; }
        public string ca { get; set; }
        public string mk { get; set; }
    }

    public class CityInfoModel
    {
        public string name { get; set; }
        public LocalNames local_names { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }



}

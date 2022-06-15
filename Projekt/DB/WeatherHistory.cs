using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DB
{
    public class WeatherHistory
    {
        public int Id { get; set; }
        public string Longitude { get; set; }   
        public string Latitude { get; set; }
        public string TimeZone { get; set; }
        public string Temperature { get; set; }
        public string FeelTemperature { get; set; }
        public string Pressure { get; set; }
        public string WindSpeed { get; set; }
        public string Cloud { get; set; }
        public string City { get; set; }

    }
}

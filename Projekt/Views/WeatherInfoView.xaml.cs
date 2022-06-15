using Projekt.API.Helpers;
using Projekt.API.Models;
using Projekt.DB;
using Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekt.Views
{
    /// <summary>
    /// Logika interakcji dla klasy WeatherInfoView.xaml
    /// </summary>
    public partial class WeatherInfoView : UserControl
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string SelectedCity { get; set; }
        public string ApiKey { get; set; }
        public string AppId { get; set; }
        public WeatherModel WeatherModel { get; set; } = new WeatherModel() { current = new CurrentWeather() };
        List<CapitalCityModel> CapitalCitiesModel { get; set; } = new List<CapitalCityModel>();
        public ObservableCollection<string> CitiesCollection { get; set; } = new ObservableCollection<string>();

        public WeatherInfoView()
        {
            FetchCapitalCityLatLongAsync();
            FetchSettings();
            InitializeComponent();

        }

        public void FetchSettings()
        {
            _context.Database.EnsureCreated();
            var settings = _context.Settings.ToList();
            if (settings != null && settings.Count > 0)
            {
                var appId = settings.Where(x => x.Name == SettingsConstants.AppId).FirstOrDefault()?.Value;
                var apiKey = settings.Where(x => x.Name == SettingsConstants.ApiKey).FirstOrDefault()?.Value;
                this.ApiKey = apiKey;
                this.AppId = appId;
            }
            //else
            //    MessageBox.Show("Nie udało się wczytać ustawień z bazy danych. Jeżeli ich jeszcze nie definiowaleś, zrób to w zakładce Ustawienia i zapisz.");
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedCity = e.AddedItems[0].ToString();
        }

        private void SaveDataButton_Clicked(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            var oldHistories = _context.WeatherHistories.Where(x => x.Latitude == this.WeatherModel.lat.ToString() && x.Longitude == this.WeatherModel.lon.ToString()).ToList();
            foreach(var record in oldHistories)
            {
                _context.Entry(record).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _context.SaveChanges();
            }

            _context.WeatherHistories.Add(new WeatherHistory()
            {
                Latitude = this.WeatherModel.lat.ToString(),
                Longitude = this.WeatherModel.lon.ToString(),
                FeelTemperature = this.WeatherModel.current.feels_like.ToString(),
                Cloud = this.WeatherModel.current.clouds.ToString(),
                Pressure = this.WeatherModel.current.pressure.ToString(),
                Temperature = this.WeatherModel.current.temp.ToString(),
                TimeZone = this.WeatherModel.timezone.ToString(),
                WindSpeed = this.WeatherModel.current.wind_speed.ToString(),
                City = !string.IsNullOrEmpty(CityName.Text) ? CityName.Text : SelectedCity
        });
            _context.SaveChanges();
            MessageBox.Show("Pomyślnie zapisano rekord w bazie danych");
        }

        private void ReadDataButton_Clicked(object sender, RoutedEventArgs e)
        {
            HideWeatherInfo();
        }

        public void FetchLastSavedWeatherInfo(double lat, double lng, string? cityName = "")
        {
            //find last weather info by lat, long
            if (lat != 0 && lng != 0)
            {
                var lastWeatherHistory = _context.WeatherHistories.Where(x => x.Latitude == lat.ToString() && x.Longitude == lng.ToString()).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastWeatherHistory != null)
                {
                    this.WeatherModel.lon = Convert.ToDouble(lastWeatherHistory.Longitude);
                    this.WeatherModel.lat = Convert.ToDouble(lastWeatherHistory.Latitude);
                    this.WeatherModel.timezone = lastWeatherHistory.TimeZone;
                    this.WeatherModel.current.temp = Convert.ToDouble(lastWeatherHistory.Temperature);
                    this.WeatherModel.current.feels_like = Convert.ToDouble(lastWeatherHistory.FeelTemperature);
                    this.WeatherModel.current.pressure = Convert.ToInt32(lastWeatherHistory.Pressure);
                    this.WeatherModel.current.wind_speed = Convert.ToDouble(lastWeatherHistory.WindSpeed);
                    this.WeatherModel.current.clouds = Convert.ToInt32(lastWeatherHistory.Cloud);
                    ShowWeatherInfo();
                }
                else
                {
                    MessageBox.Show("Nie udało się pobrać historycznego rekordu dla tego miasta, ponieważ w bazie danych nie ma odpowiadających mu rekordów");
                }
            }
            //find by name if the connection is broken
            else if (!string.IsNullOrEmpty(cityName))
            {
                var lastWeatherHistory = _context.WeatherHistories.Where(x => x.City.Trim().ToLower() == cityName.Trim().ToLower()).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastWeatherHistory != null)
                {
                    this.WeatherModel.lon = Convert.ToDouble(lastWeatherHistory.Longitude);
                    this.WeatherModel.lat = Convert.ToDouble(lastWeatherHistory.Latitude);
                    this.WeatherModel.timezone = lastWeatherHistory.TimeZone;
                    this.WeatherModel.current.temp = Convert.ToDouble(lastWeatherHistory.Temperature);
                    this.WeatherModel.current.feels_like = Convert.ToDouble(lastWeatherHistory.FeelTemperature);
                    this.WeatherModel.current.pressure = Convert.ToInt32(lastWeatherHistory.Pressure);
                    this.WeatherModel.current.wind_speed = Convert.ToDouble(lastWeatherHistory.WindSpeed);
                    this.WeatherModel.current.clouds = Convert.ToInt32(lastWeatherHistory.Cloud);
                    ShowWeatherInfo();
                }
                else
                {
                    MessageBox.Show("Nie udało się pobrać historycznego rekordu dla tego miasta, ponieważ w bazie danych nie ma odpowiadających mu rekordów");
                }
            }
            else
            {
                MessageBox.Show("Nazwa miasta nie może być pusta!");
            }
                
        }

        private void FetchData_Clicked(object sender, RoutedEventArgs e)
        {
            string text = string.Empty;
            try
            {
                text = CityName.Text;
                if (string.IsNullOrEmpty(text))
                {
                    text = this.SelectedCity;
                }
                Tuple<double, double> cityLatLong = GetCityLatLong(text);
                if (FetchWeatherData(cityLatLong))
                    ShowWeatherInfo();
                else
                {
                    MessageBox.Show("Nie udało się pobrać danych, zostaną wczytane ostatnie dane pogodowe dla podanego miasta.");
                    FetchLastSavedWeatherInfo(cityLatLong.Item1, cityLatLong.Item2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się pobrać danych, zostaną wczytane ostatnie dane pogodowe dla podanego miasta.");
                FetchLastSavedWeatherInfo(0,0,text);
            }
            
        }

        private void HideWeatherInfo()
        {
            this.CitySelect.Visibility = Visibility.Visible;
            this.FetchButton.Visibility = Visibility.Visible;
            this.CityName.Visibility = Visibility.Visible;
            this.ChooseCityLabel.Visibility = Visibility.Visible;
            this.ChooseCityNameLabel.Visibility = Visibility.Visible;

            this.Latitude.Visibility = Visibility.Hidden;
            this.LatitudeLabel.Visibility = Visibility.Hidden;
            this.LongitudeLabel.Visibility = Visibility.Hidden;
            this.Longitude.Visibility = Visibility.Hidden;
            this.TimeZone.Visibility = Visibility.Hidden;
            this.TimeZoneLabel.Visibility = Visibility.Hidden;
            this.Temperature.Visibility = Visibility.Hidden;
            this.TemperatureLabel.Visibility = Visibility.Hidden;
            this.FeelTemperature.Visibility = Visibility.Hidden;
            this.FeelTemperatureLabel.Visibility = Visibility.Hidden;
            this.Pressure.Visibility = Visibility.Hidden;
            this.PressureLabel.Visibility = Visibility.Hidden;
            this.WindSpeed.Visibility = Visibility.Hidden;
            this.WindSpeedLabel.Visibility = Visibility.Hidden;
            this.CloudLabel.Visibility = Visibility.Hidden;
            this.Cloud.Visibility = Visibility.Hidden;

            this.SaveDataButton.Visibility = Visibility.Hidden;
            this.ReadDataButton.Visibility = Visibility.Hidden;
        }

        private void ShowWeatherInfo()
        {
            this.CitySelect.Visibility = Visibility.Hidden;
            this.FetchButton.Visibility = Visibility.Hidden;
            this.CityName.Visibility = Visibility.Hidden;
            this.ChooseCityLabel.Visibility = Visibility.Hidden;
            this.ChooseCityNameLabel.Visibility = Visibility.Hidden;

            this.Latitude.Visibility = Visibility.Visible;
            this.LatitudeLabel.Visibility = Visibility.Visible;
            this.LongitudeLabel.Visibility = Visibility.Visible;
            this.Longitude.Visibility = Visibility.Visible;
            this.TimeZone.Visibility = Visibility.Visible;
            this.TimeZoneLabel.Visibility = Visibility.Visible;
            this.Temperature.Visibility = Visibility.Visible;
            this.TemperatureLabel.Visibility = Visibility.Visible;
            this.FeelTemperature.Visibility = Visibility.Visible;
            this.FeelTemperatureLabel.Visibility = Visibility.Visible;
            this.Pressure.Visibility = Visibility.Visible;
            this.PressureLabel.Visibility = Visibility.Visible;
            this.WindSpeed.Visibility = Visibility.Visible;
            this.WindSpeedLabel.Visibility = Visibility.Visible;
            this.CloudLabel.Visibility = Visibility.Visible;
            this.Cloud.Visibility = Visibility.Visible;

            this.SaveDataButton.Visibility = Visibility.Visible;
            this.ReadDataButton.Visibility = Visibility.Visible;

            this.Longitude.Text = this.WeatherModel.lon.ToString();
            this.Latitude.Text = this.WeatherModel.lat.ToString();
            this.TimeZone.Text = this.WeatherModel.timezone;
            this.Temperature.Text = this.WeatherModel.current.temp.ToString();
            this.FeelTemperature.Text = this.WeatherModel.current.feels_like.ToString();
            this.Pressure.Text = this.WeatherModel.current.pressure.ToString();
            this.WindSpeed.Text = this.WeatherModel.current.wind_speed.ToString();
            this.Cloud.Text = this.WeatherModel.current.clouds.ToString();
        }

        private void FetchCapitalCityLatLongAsync()
        {
            try
            {
                CapitalCityInfoClient capitalCityInfoClient = new CapitalCityInfoClient();
                var result = Task.Run(async () => await capitalCityInfoClient.FetchCapitalCityDataAsync()).Result;
                this.CapitalCitiesModel = result.ToList();
                this.CapitalCitiesModel.ForEach(x =>
                {
                    this.CitiesCollection.Add(x.capital);
                });
            }
            catch
            {
                MessageBox.Show("Sprawdź połączenie z internetem. Nie udało się wczytać listy miast z serwisu restcountries do zasilenia combo boxa. Użyj wyszukiwarki manualnej.");
            }
        }

        private Tuple<double, double> GetCityLatLong(string cityName)
        {
            OpenWeatherClient client = new OpenWeatherClient(this.ApiKey, this.AppId);
            var result = Task.Run(async () => await client.FetchCityData(cityName)).Result;
            CityInfoModel cityInfoModel = result;
            return Tuple.Create(cityInfoModel.lat, cityInfoModel.lon);
        }

        private bool FetchWeatherData(Tuple<double, double> cityLatLong)
        {
            try
            {
                OpenWeatherClient client = new OpenWeatherClient(this.ApiKey, this.AppId);
                var result = Task.Run(async () => await client.FetchWeatherDataAsync(cityLatLong.Item1.ToString(), cityLatLong.Item2.ToString())).Result;
                this.WeatherModel = result;
                return true;
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Sprawdź połączenie z internetem.");
                return false;
            }
        }
    }
}

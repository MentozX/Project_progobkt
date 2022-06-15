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
    public partial class SettingsView : UserControl
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public SettingsView()
        {
            InitializeComponent();
            FetchSettings();
        }

        private void SaveSettingsButton_Clicked(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }
        public void FetchSettings()
        {
            _context.Database.EnsureCreated();
            List<Setting> settings = _context.Settings.ToList();
            this.AppId.Text = settings.Where(x => x.Name == SettingsConstants.AppId).FirstOrDefault()?.Value;
            this.ApiKey.Text = settings.Where(x => x.Name == SettingsConstants.ApiKey).FirstOrDefault()?.Value;
        }

        public void SaveSettings()
        {
            try
            {
                _context.Database.EnsureCreated();
                var oldSettings = _context.Settings.ToList();
                var oldAppId = oldSettings.Where(x => x.Name == SettingsConstants.AppId).FirstOrDefault();
                if (oldAppId == null)
                {
                    var appIdSettings = new Setting() { Name = SettingsConstants.AppId, Value = AppId.Text };
                    _context.Entry(appIdSettings).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                }
                else
                {
                    oldAppId.Value = AppId.Text;
                    _context.Entry(oldAppId).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                var oldApiKey = oldSettings.Where(x => x.Name == SettingsConstants.ApiKey).FirstOrDefault();
                if (oldApiKey == null)
                {
                    var apiKeySettings = new Setting() { Name = SettingsConstants.ApiKey, Value = ApiKey.Text };
                    _context.Entry(apiKeySettings).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                }
                else
                {
                    oldApiKey.Value = ApiKey.Text;
                    _context.Entry(oldApiKey).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                _context.SaveChanges();
                MessageBox.Show("Pomyślnie zapisano ustawienia");
            }
            catch 
            {
                MessageBox.Show("Wystąpił błąd podczas zapisu ustawień");
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DB
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<WeatherHistory> WeatherHistories { get; set; }

        protected override void OnConfiguring(
           DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=weather_app_database.db");
        }
    }
}

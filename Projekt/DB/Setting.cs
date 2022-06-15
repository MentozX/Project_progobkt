using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DB
{
    public static class SettingsConstants
    {
        public const string ApiKey = "ApiKey";
        public const string AppId = "AppId";
    }
    public class Setting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

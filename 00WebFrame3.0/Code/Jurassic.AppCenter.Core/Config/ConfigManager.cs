using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.Config
{
    public class ConfigManager
    {
        private String _configFileName;
        public ConfigManager()
        {
       }

        public ConfigManager(string configFileName)
        {
            _configFileName = configFileName;
        }

        public string GetAppSettings(string settingsKey)
        {
            return ConfigurationManager.AppSettings[settingsKey];
        }

        public string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}

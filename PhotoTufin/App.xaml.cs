using System;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;

namespace PhotoTufin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string DEFAULT_VERSION = "1.0";
        private const string DEFAULT_APP = "Photo Tufin";
        private const int YEAR  = 2022;
        private const int MONTH = 10;
        private const int DAY = 22;
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Gibt die Versionsnummer der Assembly zurück.
        /// </summary>
        /// <returns>string</returns>
        public static string Version
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version == null ? DEFAULT_VERSION : $"{VersionShort}.{Build}.{Revision}";
            }
        }

        /// <summary>
        /// Calculate the build as number of days since starting development
        /// </summary>
        private static string Build
        {
            get
            {
                var start = new DateTime(YEAR, MONTH, DAY);
                var today = DateTime.Today;
           
                return (today - start).Days.ToString();
            }
        }
        
        /// <summary>
        /// Calculate the revision as number of days of the year
        /// </summary>
        private static string Revision => DateTime.Today.DayOfYear.ToString();

        /// <summary>
        /// Major and Minor of the actual version
        /// </summary>
        /// <returns>string</returns>
        public static string VersionShort
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version == null ? DEFAULT_VERSION : $"{version.Major}.{version.Minor}";
            }
        }
        
        /// <summary>
        /// Productname of the App.
        /// </summary>
        /// <returns>string</returns>
        public static string Product
        {
            get
            {
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                return name == null ? DEFAULT_APP : $"{name}";
            }
        }
        
        /// <summary>
        /// Title of the Assembly.
        /// </summary>
        /// <returns>string</returns>
        public static string Title
        {
            get
            {
                var element = Assembly.GetExecutingAssembly();
                var type = typeof(AssemblyTitleAttribute);
                var attribute = Attribute.GetCustomAttribute(element, type, false);
                
                return attribute != null ? ((AssemblyTitleAttribute)attribute).Title : DEFAULT_APP;
            }
        }
        
        /// <summary>
        /// Full path file name of the SQLite database.
        /// </summary>
        /// <returns>string</returns>
        public static string DataBaseFile
        {
            get
            {
                var product = Regex.Replace(Product.Trim(), @"\s+", "");
                var dbName = $"{product}.sqlite"; //PhotoTufin.sqlite
                var dir = System.Windows.Forms.Application.CommonAppDataPath;
                return $"{dir}\\{dbName}";
            }
        }

        /// <summary>
        /// Full path of the application.
        /// </summary>
        /// <returns>string</returns>
        private static string AppPath => Assembly.GetExecutingAssembly().Location; //AppDomain.CurrentDomain.BaseDirectory + "PhotoTufin.dll";
        //var appPath = Assembly.GetExecutingAssembly().Location";
            
        /// <summary>
        /// Get config setting by key. True if filter is checked.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetFilterSetting(string key)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(AppPath);
                var keySetting = config.AppSettings.Settings[key];

                if (config.AppSettings.Settings[key] == null)
                    return false;
            
                return keySetting.Value == "1";
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }
 
        /// <summary>
        /// Set configuration by key value pair
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isChecked"></param>
        public static void SetFilterSetting(string key, bool isChecked)
        {
            try
            {
                var value = isChecked ? "1" : "0";
                var config = ConfigurationManager.OpenExeConfiguration(AppPath);
            
                // if existing we first have to remove the key
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings.Remove(key);
            
                config.AppSettings.Settings.Add(key, value);
            
                // save the updated config
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
            
        }

        /// <summary>
        /// Removes all keys and values.
        /// </summary>
        public static void ClearFilterSetting()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(AppPath);
                foreach (var key in config.AppSettings.Settings.AllKeys)
                {
                    if (config.AppSettings.Settings[key] != null)
                        config.AppSettings.Settings.Remove(key);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }
        
        /// <summary>
        /// Removes all keys and values.
        /// </summary>
        public static bool IsFilterSettingEmpty()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(AppPath);
                return config.AppSettings.Settings.AllKeys.Length == 0;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
            
        }
    }
}
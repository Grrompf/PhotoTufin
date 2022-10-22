using System;
using System.Reflection;

namespace PhotoTufin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string DEFAULT_VERSION = "1.0";
        private const string DEFAULT_APP = "PhotoTufin";
        private const int YEAR  = 2022;
        private const int MONTH = 10;
        private const int DAY = 22;
            
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
        /// Gibt die Versionsnummer der Assembly zurück.
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
        /// Gibt die Versionsnummer der Assembly zurück.
        /// </summary>
        /// <returns>string</returns>
        public static string AppName
        {
            get
            {
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                return name == null ? DEFAULT_APP : $"{name}";
            }
        }
    }
}
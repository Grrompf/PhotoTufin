using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

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
                return $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar.ToString()}{dbName}";
            }
        }
    }
}
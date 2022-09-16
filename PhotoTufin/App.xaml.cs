using System.Reflection;

namespace PhotoTufin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Gibt die Versionsnummer der Assembly zurück.
        /// </summary>
        /// <returns>string</returns>
        public static string Version
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version != null ? $"{version}" : "1.0";
            }
        }
        
        /// <summary>
        /// Gibt die Versionsnummer der Assembly zurück.
        /// </summary>
        /// <returns>string</returns>
        public static string VersionShort
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version != null ? $"{version.Major}.{version.Minor}" : "1.0";
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
                return name != null ? $"{name}" : "MyApp";
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoTufin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = AppName;
            AppVersion.Text = $"v{VersionShort}";
        }

        /// <summary>
        /// Gibt die Versionsnummer der Assembly zurück.
        /// </summary>
        /// <returns>string</returns>
        private static string Version
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
        private static string VersionShort
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
        private static string AppName
        {
            get
            {
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                return name != null ? $"{name}" : "MyApp";
            }
        }
    }
}
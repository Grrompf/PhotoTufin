using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;
using static System.Windows.Forms.MessageBox;
using MessageBox = System.Windows.Forms.MessageBox;

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
        
        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
            
            using var fbd = new FolderBrowserDialog();
            
            var result = fbd.ShowDialog();
            if (result != OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
 
            lblSelectedDirectory.Text = $"Verzeichnis: {fbd.SelectedPath}";
            var files = Directory.GetFiles(fbd.SelectedPath);

            MessageBox.Show("Files found: " + files.Length, "Message");
        }
    
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {   
            Current.Shutdown();
        }

        private void mnuInfo_Click(object sender, RoutedEventArgs e)
        {
            string message = $"{Title} {Version}\nentwickelt für McGerhard Photography\nhttp://mcgerhard.de/\n";
            MessageBox.Show(message, $"Über {Title}", MessageBoxButtons.OK, MessageBoxIcon.Question);
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
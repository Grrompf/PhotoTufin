using System.Windows;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PhotoTufin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = App.AppName;
            AppVersion.Text = $"v{App.VersionShort}";

            ImageFilter myFilter = new ImageFilter();
            myFilter.makeFilter(Filter);
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
            var about = new About();
            about.ShowDialog();
        }
    }
}
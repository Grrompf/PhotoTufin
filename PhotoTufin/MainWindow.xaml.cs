using System.Linq;
using System.Windows;
using System.Windows.Forms;
using PhotoTufin.Search;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;

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
            Title = App.Product;
            AppVersion.Text = $"v{App.VersionShort}";

            ImageFilter.makeFilter(Filter);
        }
        
        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
            
            using var fbd = new FolderBrowserDialog();
            
            var result = fbd.ShowDialog();
            if (result != OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;

            var extensions = ImageFilter.makeFilter(Filter);
            
            lblSelectedDirectory.Text = $"Verzeichnis: {fbd.SelectedPath}";
            var search = new WalkFolders(fbd.SelectedPath, extensions);
            var fileInfos = search.search();

            // adds each data found to list
            foreach (var row in fileInfos.Select(file => new FileRow(file)))
            {
                lbFiles.Items.Add(row);
            }
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
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using PhotoTufin.Search;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;

namespace PhotoTufin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Title = App.Product;
            AppVersion.Text = $"v{App.VersionShort}";
        }
        
        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
            
            using var fbd = new FolderBrowserDialog();
            
            // remove tbl rows  
            lbFiles.Items.Clear();
            
            var result = fbd.ShowDialog();
            if (result != OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
            
            // find files and duplicates
            var factory = new ImageInfoFactory(fbd.SelectedPath, Filter);
            var imageInfos = factory.findImages();
            
            // adds each data found to list
            foreach (var row in imageInfos)
            {
                lbFiles.Items.Add(row);
            }
            
            diskInfoBox.Items.Add(factory.HDDInfo?.Model);  
            diskInfoBox.Text = factory.HDDInfo?.Model;
            
            
            lblInterface.Text = factory.HDDInfo?.InterfaceType;
            lblMedia.Text = factory.HDDInfo?.MediaType;
            lblModel.Text = factory.HDDInfo?.Model;
            lblSerialNo.Text = factory.HDDInfo?.SerialNo;
            statusDiskInfo.Visibility = Visibility.Visible;
            
            lblNoDuplicates.Text = $"{factory.NoDuplicates.ToString()} Duplikate";
            lblSelectedDirectory.Text = $"Startverzeichnis: {fbd.SelectedPath}";
            lblNoFiles.Text = $"{imageInfos.Count.ToString()} Bilder";
        }
    
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {   
            Current.Shutdown();
        }
        
        public static readonly RoutedCommand ExitCommand = 
        new RoutedUICommand("Exit", "ExitCommand", typeof(MainWindow), new InputGestureCollection(
                new InputGesture[]
                {
                   new KeyGesture(Key.X, ModifierKeys.Control)
                }
            )
        );
        
        public static readonly RoutedCommand ScanCommand = 
            new RoutedUICommand("Scan", "ScanCommand", typeof(MainWindow), new InputGestureCollection(
                    new InputGesture[]
                    {
                        new KeyGesture(Key.S, ModifierKeys.Control)
                    }
                )
            );

        private void mnuInfo_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }
    }
}
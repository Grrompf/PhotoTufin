using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using PhotoTufin.Data;
using PhotoTufin.Search;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;
using ComboBox = System.Windows.Controls.ComboBox;

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
            InitDiskComboBox();
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
            // foreach (var row in imageInfos)
            // {
            //     lbFiles.Items.Add(row);
            // }

            //REFACTOR
            // set the scan result as selected item
            var diskInfo = DiskInfoFactory.GetDiskByScanResult(factory.HDDInfo);
            
            InitDiskComboBox();
            diskInfoBox.SelectedItem = diskInfo == null ? "" : diskInfo.Model;
            viewDiskInfo.Items.Add(diskInfo);
            //REFACTOR
            
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

        private void InitDiskComboBox()
        {
            diskInfoBox.Items.Clear();
            var diskList = DiskInfoFactory.GetAllDisks();
            foreach (var disk in diskList)
            {
                diskInfoBox.Items.Add(disk.Model);
            }
        }

        private void DiskInfoBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ComboBox)sender).SelectedItem.ToString();
            var photoList = PhotoInfoFactory.GetDuplicatesByDiskInfo(selectedItem);
            
            lbFiles.Items.Clear();
            foreach (var row in photoList)
            {
                lbFiles.Items.Add(row);
            }
        }
    }
}
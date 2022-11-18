using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using PhotoTufin.Data;
using PhotoTufin.Repository;
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
        
        private void mnuScan_Click(object sender, RoutedEventArgs e)
        {
            
            using var fbd = new FolderBrowserDialog();
            
            // remove tbl rows  
            viewPhotoList.Items.Clear();
            
            // directory selection
            var result = fbd.ShowDialog();
            if (result != OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
            
            // find files and duplicates -> saves new files to db
            var factory = new ImageInfoFactory(fbd.SelectedPath, Filter);
            var imageInfos = factory.findImages();
            
            // set the scan result as selected item
            var diskInfo = DiskInfoFactory.GetDiskByScanResult(factory.HDDInfo);
            
            InitDiskComboBox();
            diskInfoBox.SelectedItem = diskInfo == null ? "" : diskInfo.Model;
            //REFACTOR
            
            dbMenuBar.Visibility = Visibility.Visible;
            
            lblNoDuplicates.Text = $"{factory.NoDuplicates.ToString()} Duplikate";
            lblSelectedDirectory.Text = $"Startverzeichnis: {fbd.SelectedPath}";
            lblNoFiles.Text = $"{imageInfos.Count.ToString()} Bilder";
        }
    
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {   
            Current.Shutdown();
        }
        
        /// <summary>
        /// Keybinding command for a shortcut to exit.
        /// </summary>
        public static readonly RoutedCommand ExitCommand = 
        new RoutedUICommand("Exit", "ExitCommand", typeof(MainWindow), new InputGestureCollection(
                new InputGesture[]
                {
                   new KeyGesture(Key.X, ModifierKeys.Control)
                }
            )
        );
        
        private void mnuInfo_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        /// <summary>
        /// Init comboBox with values from database.
        /// </summary>
        private void InitDiskComboBox()
        {
            diskInfoBox.Items.Clear();
            var diskList = DiskInfoFactory.GetAllDisks();
            foreach (var disk in diskList)
            {
                diskInfoBox.Items.Add(disk.Model);
            }

            //shows mnBar for db action
            dbMenuBar.Visibility = diskInfoBox.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// ComboBox for selecting a diskInfo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiskInfoBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null)
                return;

            var displayName = ((ComboBox)sender).SelectedItem.ToString();
            ShowDiskInfo(displayName);
            ShowPhotoDuplicates(displayName);
        }

        private void ShowPhotoDuplicates(string? displayName)
        {
            if (displayName == null)
                return;
            
            var photoList = PhotoInfoFactory.GetDuplicatesByDiskInfo(displayName);    
            viewPhotoList.Items.Clear();
            foreach (var row in photoList)
            {
                viewPhotoList.Items.Add(row);
            }
        }

        private void ShowDiskInfo(string? displayName)
        {
            if (displayName == null)
                return;
            
            var diskInfo = DiskInfoFactory.GetDiskInfoByDisplayName(displayName);
            if (diskInfo != null)
            {
                viewDiskInfo.Items.Clear();
                viewDiskInfo.Items.Add(diskInfo);
            }
            viewDiskInfo.Visibility = diskInfo == null ? Visibility.Collapsed : Visibility.Visible;
            btnClear.IsEnabled = diskInfo != null;
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            var displayName = diskInfoBox.SelectedItem.ToString();
            if (displayName == null)
                return;
            
            var diskInfo = DiskInfoFactory.GetDiskInfoByDisplayName(displayName);
            if (diskInfo == null)
                return;

            var photoInfoRepo = new PhotoInfoRepository();
            photoInfoRepo.DeleteByDiskInfo(diskInfo.Id);
            
            var diskInfoRepo = new DiskInfoRepository();
            diskInfoRepo.DeleteById(diskInfo.Id);
            
            //here you assign the values to other List

            diskInfoBox.SelectedItem = null;
            viewPhotoList.Items.Clear();
            diskInfoBox.Items.Remove(displayName);
            viewDiskInfo.Items.Clear();
            viewDiskInfo.Visibility = Visibility.Collapsed;
            btnClear.IsEnabled = false;
        }
    }
}
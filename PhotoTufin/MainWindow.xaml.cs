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
        
        private void mnuScan_Click(object sender, RoutedEventArgs e)
        {
            
            using var fbd = new FolderBrowserDialog();
            
            // directory selection
            var result = fbd.ShowDialog();
            var selectedPath = fbd.SelectedPath;
            if (result != OK || string.IsNullOrWhiteSpace(selectedPath)) return;
            
            // display action
            lblAction.Text = $"Scanning: {selectedPath}";
            
            // remove tbl rows  
            viewPhotoList.Items.Clear();

            var diskInfo = DiskInfoFactory.SaveDiskInfo(selectedPath);
            
            // find files and duplicates -> saves new files to db
            var imageInfos = ScanFactory.FindImages(selectedPath, Filter);
            lblNoDuplicates.Text = $"{ScanFactory.NoDuplicates.ToString()} Duplikate";
            lblNoFiles.Text = $"{imageInfos.Count} Bilder";
            
            // display action
            lblAction.Text = $"Speicher in Datenbank: {imageInfos.Count} Bilder";
            PhotoInfoFactory.SavePhotos(imageInfos, diskInfo);
            
            // mandatory last step: trigger photo list 
            InitDiskComboBox();
            diskInfoBox.SelectedItem = diskInfo?.DisplayName;
            dbMenuBar.Visibility = Visibility.Visible;
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
                diskInfoBox.Items.Add(disk.DisplayName);
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

            var noImages = PhotoInfoFactory.GetImageCount(displayName); 
            lblNoDuplicates.Text = $"{viewPhotoList.Items.Count.ToString()} Duplikate";
            lblAction.Text = $"Anzeige der Duplikate auf {displayName}";
            lblNoFiles.Text = $"{noImages} Bilder";
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

            if (!DiskInfoFactory.DeleteDiskAndPhotoData(displayName))
                return;
            
            // reset selection and remove item from comboBox
            diskInfoBox.SelectedItem = null;
            diskInfoBox.Items.Remove(displayName);
            
            // clear and hide the diskInfo bar 
            viewDiskInfo.Items.Clear();
            viewDiskInfo.Visibility = Visibility.Collapsed;
            
            // clear photo list
            viewPhotoList.Items.Clear();
            
            // disable btn (no selection)
            btnClear.IsEnabled = false;
            
            lblNoDuplicates.Text = $"{viewPhotoList.Items.Count.ToString()} Duplikate";
            lblAction.Text = $"{displayName} wurde gelöscht";
            lblNoFiles.Text = $"{viewPhotoList.Items.Count.ToString()} Bilder";
        }
    }
}
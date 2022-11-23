using System;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using NLog;
using PhotoTufin.Data;
using PhotoTufin.Model;
using PhotoTufin.Repository;
using PhotoTufin.Search;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;
using ComboBox = System.Windows.Controls.ComboBox;
using ListView = System.Windows.Controls.ListView;
using MessageBox = System.Windows.MessageBox;

namespace PhotoTufin;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[SupportedOSPlatform("windows")]
public partial class MainWindow
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    
    /// <summary>
    /// Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        Title = App.Product;
        AppVersion.Text = $"v{App.VersionShort}";
        InitFilterSettings();
        InitDiskComboBox();
    }
    
    /// <summary>
    /// Scan the selected volume (directory) processing diskInfo, filter settings and photos. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mnuScan_Click(object sender, RoutedEventArgs e)
    {
        try
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
        catch (Exception exception)
        {
            log.Error(exception);
        }
    }
    
    /// <summary>
    /// Exit the application 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    
    /// <summary>
    /// Showing the About dialogue... 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        try
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
        catch (Exception e)
        {
            log.Error(e);
        }
    }
        
    /// <summary>
    /// Init filter settings by configuration.
    /// </summary>
    private void InitFilterSettings()
    {
        try
        {
            var repo = new FilterConfigRepository();
            // on first run have the default values
            if (repo.FindAll().Count == 0)
                return;
            
            foreach ( var item in Filter.Items)
            {
                var mi = item as MenuItem;
                if (mi is null or { IsCheckable: false} or {HasHeader: false}) 
                    continue;

                var filter = mi.Header.ToString();
                
                if (filter == null) continue;

                var filterConfig = repo.FindByFilter(filter);
                if (filterConfig == null)
                    continue;
                
                mi.IsChecked = filterConfig.IsChecked;  
            }
        }
        catch (Exception e)
        {
            log.Error(e);
        }
    }

    /// <summary>
    /// ComboBox for selecting a diskInfo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DiskInfoBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
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
        catch (Exception exception)
        {
            log.Error(exception);
        }
        
    }

    /// <summary>
    /// Helper method for filling the list by duplicates found on the selected disk
    /// </summary>
    /// <param name="displayName"></param>
    private void ShowPhotoDuplicates(string? displayName)
    {
        try
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
        catch (Exception e)
        {
            log.Error(e);
        } 
        
    }

    /// <summary>
    /// Showing details of the selected disk. Triggers the clear button dependant on selection
    /// </summary>
    /// <param name="displayName"></param>
    private void ShowDiskInfo(string? displayName)
    {
        try
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
        catch (Exception e)
        {
            log.Error(e);
        }
    }

    /// <summary>
    /// Clears the database of a selected DiskInfo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var displayName = diskInfoBox.SelectedItem.ToString();
            if (displayName == null)
                return;
            
            var result = MessageBox.Show(
                $"Möchtest du die Daten von \"{displayName}\" löschen ?",
                "Sicherheitsabfrage",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.Cancel)
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
        catch (Exception exception)
        {
            log.Error(exception);
        }
        
    }
    
    /// <summary>
    /// Clears the complete database. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonAllDataClear_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var result = MessageBox.Show(
                $"Möchtest du alle Daten löschen ?",
                "Sicherheitsabfrage",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.Cancel)
                return;

            new PhotoInfoRepository().DropTable();
            new DiskInfoRepository().DropTable();
            
            // clear comboBox
            diskInfoBox.SelectedItem = null;
            diskInfoBox.Items.Clear();
            
            // clear and hide the diskInfo bar 
            viewDiskInfo.Items.Clear();
            viewDiskInfo.Visibility = Visibility.Collapsed;
            
            // clear photo list
            viewPhotoList.Items.Clear();
            
            // disable btn (no selection)
            btnClear.IsEnabled = false;
            
            lblNoDuplicates.Text = $"{viewPhotoList.Items.Count.ToString()} Duplikate";
            lblAction.Text = $"Komplette Datenbank wurde gelöscht";
            lblNoFiles.Text = $"{viewPhotoList.Items.Count.ToString()} Bilder";
        }
        catch (Exception exception)
        {
            log.Error(exception);
        }
    }

    /// <summary>
    /// Showing details of selected duplicate and its duplicates all over the db
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewPhotoList_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        try
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item == null)
                return;
            
            var selectedObject = (PhotoInfo)item;

            var details = new DuplicateDetails(selectedObject);
            details.ShowDialog();
        }
        catch (Exception exception)
        {
            log.Error(exception);
        }
    }
}
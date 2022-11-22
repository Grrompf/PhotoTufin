using System;
using System.Windows;
using System.Windows.Controls;
using NLog;
using PhotoTufin.Model;
using PhotoTufin.Repository;

namespace PhotoTufin;

public partial class DuplicateDetails
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="photoInfo"></param>
    public DuplicateDetails(IPhotoInfo photoInfo)
    {
        InitializeComponent();
        Title = $"{Title} (Hash: {photoInfo.HashString})";
        InitDuplicateList(photoInfo);
    }

    /// <summary>
    /// Fills the list and the count in the status bar
    /// </summary>
    /// <param name="photoInfo"></param>
    private void InitDuplicateList(IPhotoInfo photoInfo)
    {
        try
        {
            var list = new PhotoInfoRepository().FindDuplicatesByHashString(photoInfo.HashString);

            foreach (var duplicate in list)
            {
                DuplicateList.Items.Add(duplicate);
            }

            NoFiles.Text = $"{DuplicateList.Items.Count} Duplikate";
        }
        catch (Exception e)
        {
            log.Error(e);
        }
    }

    /// <summary>
    /// Showing more disk related details of the duplicate 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DuplicateList_OnGotFocus(object sender, RoutedEventArgs e)
    {
        try
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item == null)
                return;
            
            var selectedObject = (PhotoInfo)item;
        
            DiskInfo.Text = $"{selectedObject.DiskInfo?.Model} ({selectedObject.DiskInfo?.MediaType})";
        }
        catch (Exception exception)
        {
            log.Error(exception);
        }
        
    }
}
using System.Windows;
using System.Windows.Controls;
using PhotoTufin.Model;
using PhotoTufin.Repository;

namespace PhotoTufin;

public partial class DuplicateDetails
{
    public DuplicateDetails(IPhotoInfo photoInfo)
    {
        InitializeComponent();
        Title = $"{Title} (Hash: {photoInfo.HashString})";
        InitDuplicateList(photoInfo);
    }

    private void InitDuplicateList(IPhotoInfo photoInfo)
    {
        var list = new PhotoInfoRepository().FindDuplicatesByHashString(photoInfo.HashString);

        foreach (var duplicate in list)
        {
            DuplicateList.Items.Add(duplicate);
        }

        NoFiles.Text = $"{DuplicateList.Items.Count} Duplikate";
    }

    private void DuplicateList_OnGotFocus(object sender, RoutedEventArgs e)
    {
        var item = (sender as ListView)?.SelectedItem;
        if (item == null)
            return;
            
        var selectedObject = (PhotoInfo)item;
        
        DiskInfo.Text = $"{selectedObject.DiskInfo?.Model} ({selectedObject.DiskInfo?.MediaType})";
    }
}
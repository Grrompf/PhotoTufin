using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PhotoTufin.Search.Filter;

/// <summary>
/// Helper for an file extension filter. Using the user UI Filter of the windows menu and storing
/// updated settings in a configuration. The new config will remember the filter settings on next start.   
/// </summary>
public static class ImageFilter
{
    private static List<string> FileExtensions { get; } = new();

    /// <summary>
    /// Reads the checked header infos of the GUI (menu: Filter).
    /// Return a collection of strings of selected image extensiosns.
    /// </summary>
    /// <param name="Filter"></param>
    /// <returns></returns>
    public static List<string> makeFilter(MenuItem Filter)
    {
        FileExtensions.Clear();
        App.ClearFilterSetting();

        foreach ( var item in Filter.Items)
        {
            var mi = item as MenuItem;
            if (mi is null or { IsCheckable: false} or {IsChecked: false} or {HasHeader: false}) 
                continue;

            var filter = mi.Header.ToString();
            if (filter == null) continue;
            
            App.SetFilterSetting(filter, mi.IsChecked);
            convertHeaderToExtensions(filter);
        }

        return FileExtensions;
    }

    /// <summary>
    /// Convert the header string into a filter by adding a leading dot.
    /// </summary>
    /// <param name="header"></param>
    private static void convertHeaderToExtensions(string header)
    {
        var tokens = header.Split(',').ToList();
        foreach (var token in tokens)
        {
            FileExtensions.Add($".{token}");
        }
    }
}
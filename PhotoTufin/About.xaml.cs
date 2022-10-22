using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace PhotoTufin;

public partial class About
{
    public About()
    {
        InitializeComponent();
        Title = $"Über {App.AppName}";
        AppName.Text = "Photo Tupled Finder";
        Version.Text = $"Version: {App.Version}";
        Author.Text  = $"Autor: Grrompf";
        Company.Text = $"McGerhard Photography";
    }
    
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void btnUri_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Hyperlink link) return;
        
        var defaultBrowser = new Process();
        defaultBrowser.StartInfo.UseShellExecute = true;
        defaultBrowser.StartInfo.FileName = link.NavigateUri.AbsoluteUri;
        defaultBrowser.Start();
    }
}
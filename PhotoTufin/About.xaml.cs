using System.Windows;

namespace PhotoTufin;

public partial class About
{
    public About()
    {
        InitializeComponent();
        Title = $"Über {App.AppName}";
        VersionLong.Text = $"Version: {App.Version}";
    }
    
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
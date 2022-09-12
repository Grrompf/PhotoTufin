using System.Windows;

namespace PhotoTufin;

public partial class About
{
    public About()
    {
        InitializeComponent();
        Title = $"Über {MainWindow.AppName}";
        VersionLong.Text = $"Version: {MainWindow.Version}";
    }
    
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
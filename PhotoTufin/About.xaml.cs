using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using NLog;

namespace PhotoTufin;

public partial class About
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    
    /// <summary>
    /// Constructor
    /// </summary>
    public About()
    {
        InitializeComponent();
        Title = $"Über {App.Product}";
        AppName.Text = App.Title;
        Version.Text = $"Version: {App.Version}";
        Author.Text  = "Autor: Grrompf";
        Company.Text = "McGerhard Photography";
    }
    
    /// <summary>
    /// Exit the dialogue by btn
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    /// <summary>
    /// Starts browsing the hyperlink
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnUri_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is not Hyperlink link) return;
        
            var defaultBrowser = new Process();
            defaultBrowser.StartInfo.UseShellExecute = true;
            defaultBrowser.StartInfo.FileName = link.NavigateUri.AbsoluteUri;
            defaultBrowser.Start();
        }
        catch (Exception exception)
        {
            log.Error(exception);
            throw;
        }
        
    }
}
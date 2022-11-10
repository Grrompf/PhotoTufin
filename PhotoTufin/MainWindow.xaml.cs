using System;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Forms;
using PhotoTufin.Data;
using PhotoTufin.Model;
using PhotoTufin.Search;
using static System.Windows.Application;
using static System.Windows.Forms.DialogResult;

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
            // ICustomerRepository rep = new SqLiteCustomerRepository();
            // var customer = new Customer
            // {
            //     FirstName = "Sergey",
            //     LastName = "Maskalik",
            //     DateOfBirth = DateTime.Now
            // };
            // rep.SaveCustomer(customer);

            var repo = new DiskInfoRepository();
            repo.DropTable();
            repo.CreateTable();
            
            var info = new DiskInfo
            {
                Model = "Sergey",
                SerialNo = "Maskalik",
                CreatedAt = DateTime.Now
            };
            repo.SaveDiskInfo(info);
            
            info = repo.GetDiskInfo(1);
            
            Console.WriteLine($"{info}");

            info = repo.GetDiskInfo(2);
            
            Console.WriteLine($"{info}");
            //Customer retrievedCustomer = rep.GetCustomer(customer.Id);
        }
        
        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
            
            using var fbd = new FolderBrowserDialog();
            
            // remove tbl rows  
            lbFiles.Items.Clear();
            
            var result = fbd.ShowDialog();
            if (result != OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
            
            // find files and duplicates
            var factory = new ImageInfoFactory(fbd.SelectedPath, Filter);
            var imageInfos = factory.findImages();
            
            // adds each data found to list
            foreach (var row in imageInfos)
            {
                lbFiles.Items.Add(row);
            }

            lblNoDuplicates.Text = $"{factory.NoDuplicates.ToString()} Duplikate";
            lblSelectedDirectory.Text = $"Startverzeichnis: {fbd.SelectedPath}";
            lblNoFiles.Text = $"{imageInfos.Count.ToString()} Bilder";
        }
    
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {   
            Current.Shutdown();
        }

        private void mnuInfo_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }
    }
}
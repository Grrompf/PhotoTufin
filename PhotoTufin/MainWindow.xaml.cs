using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Forms;
using PhotoTufin.Repository;
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
            var info = new DiskInfo
            {
                Model = "Sergey",
                SerialNo = "123"
            };
            repo.Save(info);
            
            info = new DiskInfo
            {
                Model = "Hummel",
                SerialNo = "567"
            };
            repo.Save(info);
            
            info = repo.FindBySerialNo("567");
            
            
            
            Console.WriteLine($"{info}");

            List<DiskInfo> myList = repo.FindAll();
            Console.WriteLine($"{myList}");
            //Customer retrievedCustomer = rep.GetCustomer(customer.Id);
            
            var repository = new PhotoInfoRepository();
            repository.DropTable();
            
            repository.CreateTable();
            var photo = new PhotoInfo
            {
                DiskInfoId = info.Id,
                FileName = "hans.jpg",
                FilePath = "hans.jpg",
                Size = "null",
                HashString = "jkdsahkjahfk"
            };
            repository.Save(photo);

            photo.Size = "45";
            repository.Save(photo);
            
            photo.HashString = "blabla";
            repository.Save(photo);
            
            photo = new PhotoInfo
            {
                DiskInfoId = 2,
                FileName = "jupp.jpg",
                FilePath = "jupp.jpg",
                Size = "null",
                HashString = "12jkdsahkjahfk"
            };
            repository.Save(photo);
            
            
            var klist = repository.FindAll();
            Console.WriteLine($"{klist}");
            
            var ph = repository.Find(1);
            Console.WriteLine($"{ph}");
            
            List<PhotoInfo> myList2 = repository.FindDuplicates();
            Console.WriteLine($"{myList2}");
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
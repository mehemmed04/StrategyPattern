using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Imaging;
using StrategyPattern.ImageStrategy;

namespace StrategyPattern
{
    public partial class MainWindow : Window
    {
        public List<string> Types { get; set; }
        public MainWindow()
        {
            
            InitializeComponent();
            this.DataContext = this;
            
            Types = new List<string>
            {
                "pdf",
                "png",
                "jpg",
                "gif",
                "bmp"
            };
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = true };
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                string[] files = openFileDialog.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    string filename = System.IO.Path.GetFullPath(files[i]);
                    FileInfo fileInfo = new FileInfo(files[i]);
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(filename);
                    logo.EndInit();
                    mainImage.Source = logo;

                }
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            IImageSaveStrategy saveStrategy;
            string extension = TypesCombobox.SelectedItem as string;
            switch (extension)
            {
                case "pdf":
                    saveStrategy = new PdfStrategy();
                    break;
                case "jpg":
                    saveStrategy = new JpgStrategy();
                    break;
                case "png":
                    saveStrategy = new PngStrategy();
                    break;
                case "gif":
                    saveStrategy = new GifStrategy();
                    break;
                case "bmp":
                    saveStrategy = new BmpStrategy();
                    break;
                default:
                    throw new ArgumentNullException(extension);
            }
            saveStrategy.SaveFile(((BitmapImage)mainImage.Source).UriSource.AbsolutePath);
        }


    };
}
    

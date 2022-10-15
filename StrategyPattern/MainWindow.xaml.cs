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
            ImageConverter imageConverter = new ImageConverter();
            string extension = TypesCombobox.SelectedItem as string;
            switch (extension)
            {
                case "pdf":
                    imageConverter.SetStrategy(new PdfStrategy());
                    break;
                case "jpg":
                    imageConverter.SetStrategy(new JpgStrategy());
                    break;
                case "png":
                    imageConverter.SetStrategy(new PngStrategy());
                    break;
                case "gif":
                    imageConverter.SetStrategy(new GifStrategy());
                    break;
                case "bmp":
                    imageConverter.SetStrategy(new BmpStrategy());
                    break;
                default:
                    throw new ArgumentNullException(extension);
            }
            imageConverter.ConvertAndSaveImage(((BitmapImage)mainImage.Source).UriSource.AbsolutePath);
        }


    };
}
    

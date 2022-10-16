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
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;

namespace StrategyPattern
{
    public partial class MainWindow : Window
    {
        private Bitmap OriginalImage;
        private Bitmap EditedImage;
        private int CurrentRotation = 0;
        public string FileName { get; set; }

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

        private static BitmapImage BitmapToSource(Bitmap src)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            src.Save(ms, ImageFormat.Jpeg);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void UpdateImage(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                float brightness = (float)BrightnessSlider.Value;
                float contrast = (float)ContrastSlider.Value;

                if (ContrastSlider.Value < 1)
                {
                    contrast = (float)(ContrastSlider.Value / 2) + 0.5f;
                }

                if (sender is Button btn)
                {
                    if (btn.Name == "RotateAnticlockwiseBtn")
                    {
                        if (CurrentRotation <= 0)
                        {
                            CurrentRotation = 270;
                        }
                        else
                        {
                            CurrentRotation -= 90;
                        }
                    }
                    else if (btn.Name == "RotateClockwiseBtn")
                    {
                        if (CurrentRotation >= 270)
                        {
                            CurrentRotation = 0;
                        }
                        else
                        {
                            CurrentRotation += 90;
                        }
                    }
                }

                float[][] greyscale = new float[][] {
                    new float[] { 0.299f, 0.299f, 0.299f, 0, 0},
                    new float[] { 0.587f, 0.587f, 0.587f, 0, 0},
                    new float[] { 0.114f, 0.114f, 0.114f, 0, 0},
                    new float[] { 0, 0, 0, 1, 0},
                    new float[] { 0, 0, 0, 0, 1}
                };

                float[][] light = new float[][] {
                    new float[] {contrast, 0, 0, 0, 0},
                    new float[] {0, contrast, 0, 0, 0},
                    new float[] {0, 0, contrast, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {brightness, brightness, brightness, 0, 1}
                };

                Bitmap bmp = new Bitmap(OriginalImage);
                ImageAttributes imgattr = new ImageAttributes();
                Rectangle rc = new Rectangle(0, 0, bmp.Width, bmp.Height);

                if (NoFilterRadio.IsChecked == true)
                {
                    imgattr.SetColorMatrix(new ColorMatrix(light));
                }
                else
                {
                    imgattr.SetColorMatrix(new ColorMatrix(Multiply(greyscale, light)));
                }

                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(bmp, rc, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imgattr);

                    if (RedRadio.IsChecked == true)
                    {
                        g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(100, 235, 58, 52)), rc);
                    }
                    else if (GreenRadio.IsChecked == true)
                    {
                        g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(100, 52, 235, 73)), rc);
                    }
                    else if (BlueRadio.IsChecked == true)
                    {
                        g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(100, 52, 122, 235)), rc);
                    }
                }

                switch (CurrentRotation)
                {
                    case 90:
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 180:
                        bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 270:
                        bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        break;
                }

                EditedImage = bmp;
                mainImage.Source = BitmapToSource(EditedImage);
            }
        }

        private float[][] Multiply(float[][] f1, float[][] f2)
        {
            float[][] X = new float[5][];
            for (int d = 0; d < 5; d++)
                X[d] = new float[5];
            int size = 5;
            float[] column = new float[5];
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    column[k] = f1[k][j];
                }
                for (int i = 0; i < 5; i++)
                {
                    float[] row = f2[i];
                    float s = 0;
                    for (int k = 0; k < size; k++)
                    {
                        s += row[k] * column[k];
                    }
                    X[i][j] = s;
                }
            }
            return X;
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
                    OriginalImage = new Bitmap(filename);
                    mainImage.Source = BitmapToSource(new Bitmap(OriginalImage));

                    var path = filename.Split('\\');
                    FileName = path[path.Length - 1];



                }
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            ImageStrategy.ImageConverter imageConverter = new ImageStrategy.ImageConverter();
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
            EditedImage.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"+FileName);
            imageConverter.ConvertAndSaveImage(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"+FileName);
        }


    };
}
    

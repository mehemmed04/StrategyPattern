using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern.ImageStrategy
{
    public class PngStrategy : IImageSaveStrategy
    {
        public void SaveFile(string ImagePath)
        {
            var doc = new Document();
            var builder = new DocumentBuilder(doc);

            var shape = builder.InsertImage(ImagePath);
            var list = System.IO.Path.GetFileName(ImagePath).Split('.');
            var path = list[0].Split('\\');
            var filename = path[path.Length - 1];
            var newfilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + filename + ".png";
            shape.ImageData.Save(newfilename);
        }
    }
}

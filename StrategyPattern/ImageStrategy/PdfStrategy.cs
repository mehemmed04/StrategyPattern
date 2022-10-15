using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern.ImageStrategy
{
    public class PdfStrategy : IImageSaveStrategy
    {
        public void SaveFile(string ImagePath)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
            var list = System.IO.Path.GetFileName(ImagePath).Split('.');
            var path = list[0].Split('\\');
            var filename = path[path.Length - 1];
            string PDFpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + filename + ".pdf";
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(PDFpath, FileMode.Create));
            doc.Open();
            var logo = iTextSharp.text.Image.GetInstance(ImagePath);
            logo.ScaleAbsoluteHeight(500);
            logo.ScaleAbsoluteWidth(500);
            doc.Add(logo);
            doc.Close();
        }
    }
}

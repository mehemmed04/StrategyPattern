using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern.ImageStrategy
{
    public class ImageConverter
    {
        public IImageSaveStrategy _imageSaveStrategy;
        public ImageConverter()
        {

        }
        public ImageConverter( IImageSaveStrategy imageSaveStrategy)
        {
            _imageSaveStrategy = imageSaveStrategy;
        }
        public void SetStrategy(IImageSaveStrategy imageSaveStrategy)
        {
            this._imageSaveStrategy = imageSaveStrategy;
        }
        public void ConvertAndSaveImage(string path)
        {
            _imageSaveStrategy.SaveFile(path);
        }
    }
}

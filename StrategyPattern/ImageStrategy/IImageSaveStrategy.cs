using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern.ImageStrategy
{
    public interface IImageSaveStrategy
    {
        void SaveFile(string ImagePath);
    }
}

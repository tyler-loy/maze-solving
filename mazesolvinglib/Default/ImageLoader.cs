using System;
using System.IO;
using mazesolvinglib.Entities;
using mazesolvinglib.Interfaces;
using mazesolvinglib.Utility;
using SixLabors.ImageSharp;

namespace mazesolvinglib.Default
{
    public class ImageLoader : IImageLoader
    {
        private ILogger _logger;

        public ImageLoader(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Image Loader");
        }

        public SourceImage LoadImage(string path)
        {

            var loadStart = DateTime.Now;
            SourceImage sourceImage;
            using (FileStream stream = File.OpenRead(path))
            using (Image<Rgba32> image = Image.Load<Rgba32>(stream))
            {
                sourceImage = BuildSourceImage(image);
            }

            var loadEnd = DateTime.Now;
            _logger.Log($"Time Taken: {(loadEnd - loadStart).ToString("g")}");
            return sourceImage;
        }

        public virtual SourceImage BuildSourceImage(Image<Rgba32> image)
        {
            SourceImage sourceImage = new SourceImage();
            sourceImage.Source = image.Clone();
            sourceImage.Cells = GetCellTypes(image);
            return sourceImage;
        }

        private CellType[,] GetCellTypes(Image<Rgba32> image)
        {
            CellType[,] cellTypes = new CellType[image.Height, image.Width];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var c = image[x, y];
                    cellTypes[y, x] = GetCellType(c);
                }
            }
            return cellTypes;
        }

        public virtual CellType GetCellType(Rgba32 pixel)
        {
            var hex = pixel.ToHex();
            switch (hex)
            {
                case "000000FF":
                    return CellType.Wall;
                case "FFFFFFFF":
                    return CellType.Pathable;
                default:
                    return CellType.Unkown;
            }
        }

    }
}
using System;
using System.Drawing;
using mazesolvinglib.Interfaces;
using mazesolvinglib.Utility;

namespace mazesolvinglib.Default
{
    public class ImageLoader : IImageLoader
    {
        private ILogger _logger;

        public ImageLoader(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Image Loader");
        }

        public Bitmap LoadImage(string path)
        {
            var loadStart = DateTime.Now;
            var image = (Bitmap)Image.FromFile(path);
            var loadEnd = DateTime.Now;
            _logger.Log($"Time Taken: {(loadEnd - loadStart).ToString("g")}");
            return image;
        }
    }
}
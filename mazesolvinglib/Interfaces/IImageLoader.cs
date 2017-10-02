using System.Drawing;

namespace mazesolvinglib.Interfaces
{
    public interface IImageLoader
    {
        Bitmap LoadImage(string path);
    }
}

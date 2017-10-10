using System.IO;
using mazesolvinglib.Entities;

namespace mazesolvinglib.Interfaces
{
    public interface IImageLoader
    {
        SourceImage LoadImage(FileInfo path);
    }
}

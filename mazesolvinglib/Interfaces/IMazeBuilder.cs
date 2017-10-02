using System.Drawing;
using mazesolvinglib.Entities;

namespace mazesolvinglib.Interfaces
{
    public interface IMazeBuilder
    {
        Maze Build(Bitmap image);
    }
}

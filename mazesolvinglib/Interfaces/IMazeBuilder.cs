using mazesolvinglib.Entities;

namespace mazesolvinglib.Interfaces
{
    public interface IMazeBuilder
    {
        Maze Build(SourceImage image);
    }
}

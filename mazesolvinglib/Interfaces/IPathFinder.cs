using System.Collections.Generic;
using System.Text;
using mazesolvinglib.Entities;

namespace mazesolvinglib.Interfaces
{
    public interface IPathFinder
    {
        string Name { get; set; }
        Path Path(Maze maze);
    }
}

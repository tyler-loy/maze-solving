using mazesolvinglib.Entities;

namespace mazesolvinglib.Interfaces
{
    public interface IMazeSolver
    {
        Solution Solve(Maze maze);
    }
}

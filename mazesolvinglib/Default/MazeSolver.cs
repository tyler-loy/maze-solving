using System;
using System.Collections.Generic;
using mazesolvinglib.Entities;
using mazesolvinglib.Interfaces;
using mazesolvinglib.Utility;

namespace mazesolvinglib.Default
{
    public class MazeSolver : IMazeSolver
    {
        private readonly List<IPathFinder> _pathFinders;
        private ILogger _logger;

        public MazeSolver(List<IPathFinder> pathFinders, ILoggerFactory loggerFactory)
        {
            _pathFinders = pathFinders;
            _logger = loggerFactory.CreateLogger("Maze Solver");
        }

        public Solution Solve(Maze maze)
        {
            _logger.Log($"We have {_pathFinders.Count} path finders to try");

            var solution = new Solution();
            solution.Paths = new List<Path>();

            foreach (IPathFinder pathFinder in _pathFinders)
            {
                _logger.Log($"Starting {pathFinder.Name}");
                var pfStart = DateTime.Now;
                var path = pathFinder.Path(maze);
                var pfEnd = DateTime.Now;

                _logger.Log($"{pathFinder.Name} Finished Time Taken: {(pfEnd - pfStart).ToString("g")}");

                if (path == null)
                {
                    _logger.Log($"{pathFinder.Name} failed to find a path");
                }
                else
                {
                    solution.Paths.Add(path);
                    _logger.Log($"{pathFinder.Name} found a path with a distance of {path.TotalDistance}");
                }
            }

            return solution;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using mazesolving.DependencyInjection;
using mazesolvinglib.Interfaces;
using mazesolvinglib.Utility;
using StructureMap;

namespace mazesolving
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            var filePath = args.FirstOrDefault();
            var file = new FileInfo(filePath);
            
            using (IContainer container = new Container(new DefaultRegistry()))
            {
                var solver = container.GetInstance<IMazeSolver>();
                var loader = container.GetInstance<IImageLoader>();
                var builder = container.GetInstance<IMazeBuilder>();
                var vis = container.GetInstance<ISolutionVisualizar>();
                var logger = container.GetInstance<ILoggerFactory>().CreateLogger("Main");
                logger.Log($"DI Time Taken: {(DateTime.Now - startTime).ToString("g")}");

                var image = loader.LoadImage(file);
                var maze = builder.Build(image);
                var solution = solver.Solve(maze);
                vis.VisualizeSolution(solution, image);

                var endTime = DateTime.Now;
                logger.Log($"Finished Time Taken: {(endTime - startTime).ToString("g")}");
                Console.ReadLine();
            }
        }
    }
}

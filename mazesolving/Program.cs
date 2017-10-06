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
                var logger = container.GetInstance<ILoggerFactory>().CreateLogger("Main");
                
                var image = loader.LoadImage(file.FullName);
                var maze = builder.Build(image);
                var solution = solver.Solve(maze);

                var endTime = DateTime.Now;
                logger.Log($"Finished Time Taken: {(endTime - startTime).ToString("g")}");
                Console.ReadLine();
            }
        }
    }
}

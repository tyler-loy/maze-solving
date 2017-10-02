using System;
using System.IO;
using System.Linq;
using mazesolving.DependencyInjection;
using mazesolvinglib.Interfaces;
using StructureMap;

namespace mazesolving
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HELLO!");

            var filePath = args.FirstOrDefault();
            var file = new FileInfo(filePath);
            

            using (IContainer container = new Container(new DefaultRegistry()))
            {
                var solver = container.GetInstance<IMazeSolver>();
                var loader = container.GetInstance<IImageLoader>();
                var builder = container.GetInstance<IMazeBuilder>();
                
                var image = loader.LoadImage(file.FullName);
                var maze = builder.Build(image);
                var solution = solver.Solve(maze);

            }
        }
    }
}

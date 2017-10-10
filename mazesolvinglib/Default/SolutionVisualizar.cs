using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using mazesolvinglib.Entities;
using mazesolvinglib.Interfaces;
using mazesolvinglib.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.Primitives;
using SixLabors.Shapes;
using Path = mazesolvinglib.Entities.Path;

namespace mazesolvinglib.Default
{
    public class SolutionVisualizar : ISolutionVisualizar
    {
        private ILogger _logger;

        public SolutionVisualizar(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("SolutionVisualizar");
        }

        public void VisualizeSolution(Solution solution, SourceImage sourceImage)
        {
            foreach (Path solutionPath in solution.Paths)
            {
                var fileName = $"./{sourceImage.FileName.Split('.').FirstOrDefault()}_{solutionPath.PathFinderName}.png";
                using (FileStream output = File.OpenWrite(fileName))
                using (Image<Rgba32> image = sourceImage.Source.Clone())
                {
                    _logger.Log($"Visualizing {solutionPath.PathFinderName} {fileName}");
                    PathBuilder pathBuilder = new PathBuilder();
                    PathNode prev = null;
                    foreach (PathNode pathNode in solutionPath.PathNodes)
                    {
                        if (prev != null)
                        {
                            pathBuilder.AddLine(new PointF(prev.X, prev.Y), new PointF(pathNode.X, pathNode.Y));

                        }

                        prev = pathNode;
                    }

                    IPath path = pathBuilder.Build();

                    image.Mutate(ctx => ctx.Draw(Rgba32.Blue, 1, path));

                    image.Save(output, new PngEncoder());
                }
            }
        }
    }
}

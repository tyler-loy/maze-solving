using mazesolvinglib.Interfaces;
using StructureMap;

namespace mazesolvinglib.DependencyInjection
{
    public class LibRegistry : Registry
    {
        public LibRegistry()
        {
            Scan(s =>
            {
                s.WithDefaultConventions();

                s.AddAllTypesOf<IPathFinder>();
            });
        }
    }
}

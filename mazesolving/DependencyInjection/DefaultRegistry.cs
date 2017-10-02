using System;
using System.Collections.Generic;
using System.Text;
using mazesolvinglib.DependencyInjection;
using mazesolvinglib.Interfaces;
using StructureMap;

namespace mazesolving.DependencyInjection
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(s =>
            {
                s.TheCallingAssembly();
                s.AssemblyContainingType<LibRegistry>();
                s.LookForRegistries();
                s.WithDefaultConventions();
                s.AddAllTypesOf<IPathFinder>();
            });
        }
    }
}

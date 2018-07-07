using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agrimanagr.Core.Storage;
using Mobile.core.Repositories;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Mobile.core.Dependencies
{
    public  class DependencyContainerBuilder
    {
        //Add any additional modules from Distributr.Mobile.Core here
        private readonly List<Registry> coreModules = new List<Registry>()
        {
            new StorageModule(),
            new RepositoriesModule(),
           
        };

        private readonly List<Registry> additional = new List<Registry>();

        //This is to allow modules that use Mobile.Core to provide environment specific-stuff
        public DependencyContainerBuilder AddModule(Registry module)
        {
            additional.Add(module);
            return this;
        }

        public DependencyContainer Build()
        {
            coreModules.AddRange(additional);
            return new DependencyContainer(coreModules);
        }
    }

    public class DependencyContainer
    {
        private readonly Container container;

        internal DependencyContainer(List<Registry> modules)
        {
            var builder = new PluginGraphBuilder();
            modules.ForEach(e => builder.Add(e));
            container = new Container(builder.Build());
        }

        public T Resolve<T>()
        {
            return container.GetInstance<T>();
        }
    }
}



using Mobile.core.SQLiteDatabase;
using Mobile.Common.Storage;
using StructureMap.Configuration.DSL;

namespace Agrimanagr.Core.Storage
{
    public class StorageModule : Registry
    {
        public StorageModule()
        {
            For<IDatabase>().Singleton().Use<Database>();
        }
    }
}
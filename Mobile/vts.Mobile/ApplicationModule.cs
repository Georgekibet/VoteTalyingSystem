using Mobile.core.SQLiteDatabase;
using Mobile.Common.Core;
using Mobile.Common.Core.Net;
using Mobile.Common.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using StructureMap.Configuration.DSL;
using vts.Core.Shared.Entities.Master;
using vts.Mobile.SQLiteDatabase;

namespace vts.Mobile
{
    public class ApplicationModule : Registry
    {
        public ApplicationModule(BaseApplication<User> app)
        {
            For<BaseApplication<User>>().Use(app);
            For<IFileSystemLocations>().Use<AndroidFileLocations>();
            For<ISQLitePlatform>().Use(new SQLitePlatformAndroid());
            For<IConnectivityMonitor>().Use(new ConnectivityMonitor<User>(app));
        }


    }
}
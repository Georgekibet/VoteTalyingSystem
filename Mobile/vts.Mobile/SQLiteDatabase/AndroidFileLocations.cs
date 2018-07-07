using System;
using System.IO;
using Mobile.core.SQLiteDatabase;


namespace vts.Mobile.SQLiteDatabase
{
    public class AndroidFileLocations : IFileSystemLocations
    {
        public string DatabaseLocation
        {
            get
            {
                //return Path.Combine(GetRootStorageFolder(), "agrimanagr.test.db");
                return Path.Combine(GetRootStorageFolder(), "vts.db");
            }
        }

        private string GetRootStorageFolder()
        {
            //return Android.OS.Environment.ExternalStorageDirectory.Path;
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}
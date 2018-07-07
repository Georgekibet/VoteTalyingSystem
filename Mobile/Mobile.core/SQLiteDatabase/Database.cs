using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Agrimanagr.Core.Storage;
using Mobile.Common.Storage;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;

namespace Mobile.core.SQLiteDatabase
{
    public class Database : SQLiteConnection, IDatabase
    {
        private const SQLiteOpenFlags OpenFlags =
            SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

        public Database(ISQLitePlatform platform, IFileSystemLocations locations): base(platform, locations.DatabaseLocation, OpenFlags, storeDateTimeAsTicks: true)
        {
            BusyTimeout = new TimeSpan(0, 1, 0);
            CreateTables();
        }

        public void CreateTables()
        {
            foreach (var type in DatabaseConfig.GetPersistentTypes())
            {
                CreateTable(type);
            }
        }

        public void ClearTables()
        {
            foreach (var type in DatabaseConfig.GetTransientTypes())
            {
                DeleteAll(type);
            }        
        }

        public void DeleteAll(Type type)
        {
            Execute("delete from " + type.Name);
        }

        public void Insert(object entity)
        {
            base.Insert(entity);
        }

        public int Count(Type type)
        {
            var name = GetTableName(type);
            return ExecuteScalar<int>("select count(*) from " + name);
        }

        public static string GetTableName(Type type)
        {
            var name = type.Name;
            foreach (var tableAttribute in type.GetCustomAttributes().OfType<TableAttribute>())
            {
                name = tableAttribute.Name;
            }
            return name;
        }

        public List<T> GetAll<T>() where T : class, new()
        {
            return this.GetAllWithChildren<T>(recursive: true, filter: p => true);
        }

        public long SequenceNextValue(SequenceName sequenceName)
        {
            var sequence = Find<DatabaseSequence>(sequenceName);
            if (sequence == null)
            {
                Insert(new DatabaseSequence() { SequenceName = sequenceName, NextValue = 2 });
                return 1;
            }

            var value = sequence.NextValue++;
            Update(sequence);

            return value;
        }
    }

    public enum SequenceName { DocumentReference ,FielAquisition,VehicleLoading,VehicleManifest,VehicleUnloading}

    public class DatabaseSequence
    {
        [PrimaryKey]
        public SequenceName SequenceName { get; set; }

        public int NextValue { get; set; }
    }
}

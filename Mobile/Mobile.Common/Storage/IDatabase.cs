using System;

namespace Mobile.Common.Storage
{
    public interface IDatabase
    {
        void ClearTables();
        void DeleteAll(Type type);
        int Execute(string sql, params object [] args);
        void Insert(object entity);
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}

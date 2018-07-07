using System;
using System.Collections.Generic;
using Mobile.core.SQLiteDatabase;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using vts.Shared.Utility;

namespace Mobile.core.Repositories
{
    public class WardRepository : IWardRepository
    {
        public WardRepository(Database database)
        {
            Database = database;
        }

        private Database Database { get; }

        public ValidationResultInfo Validate(Ward itemToValidate)
        {
            throw new NotImplementedException();
        }

        public Guid Save(Ward entity, bool? isSync = null)
        {
            Database.InsertOrReplace(entity);
            return entity.Id;
        }

        public void SetInactive(Ward entity)
        {
            throw new NotImplementedException();
        }

        public void SetActive(Ward entity)
        {
            throw new NotImplementedException();
        }

        public void SetAsDeleted(Ward entity)
        {
            throw new NotImplementedException();
        }

        public Ward GetById(Guid id, bool includeDeactivated = false)
        {
            return Database.Get<Ward>(id);
        }

        public IEnumerable<Ward> GetAll(bool includeDeactivated = false)
        {
            return Database.GetAll<Ward>();
        }

        public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastTimeItemUpdated()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ward> GetItemUpdated(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public int GetCount(bool includeDeactivated = false)
        {
            throw new NotImplementedException();
        }

        public IPaginatedList<Ward> GetAll(int currentPage, int itemPerPage, string searchText,
            bool includeDeactivated = false)
        {
            throw new NotImplementedException();
        }
    }
}
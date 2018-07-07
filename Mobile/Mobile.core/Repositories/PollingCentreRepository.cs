using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.core.SQLiteDatabase;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using vts.Shared.Utility;

namespace Mobile.core.Repositories
{
    class PollingCentreRepository : IPollingCentreRepository
    {
        public PollingCentreRepository(Database database)
        {
            Database = database;
        }

        private Database Database { get; }
        public ValidationResultInfo Validate(PollingCentre itemToValidate)
        {
            throw new NotImplementedException();
        }

        public Guid Save(PollingCentre entity, bool? isSync = null)
        {
            Database.InsertOrReplace(entity);
            return entity.Id;
        }

        public void SetInactive(PollingCentre entity)
        {
            throw new NotImplementedException();
        }

        public void SetActive(PollingCentre entity)
        {
            throw new NotImplementedException();
        }

        public void SetAsDeleted(PollingCentre entity)
        {
            throw new NotImplementedException();
        }

        public PollingCentre GetById(Guid id, bool includeDeactivated = false)
        {
            return Database.Get<PollingCentre>(id);

        }

        public IEnumerable<PollingCentre> GetAll(bool includeDeactivated = false)
        {
            return  Database.GetAll<PollingCentre>();
        }

        public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastTimeItemUpdated()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PollingCentre> GetItemUpdated(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public int GetCount(bool includeDeactivated = false)
        {
            throw new NotImplementedException();
        }

        public IPaginatedList<PollingCentre> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
        {
            throw new NotImplementedException();
        }
    }
}

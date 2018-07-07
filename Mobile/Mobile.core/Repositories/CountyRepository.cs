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
  public  class CountyRepository:ICountyRepository
    {
      public CountyRepository(Database database)
      {
          Database = database;
      }

      private Database Database { get; }
        public ValidationResultInfo Validate(County itemToValidate)
      {
          throw new NotImplementedException();
      }

      public Guid Save(County entity, bool? isSync = null)
      {
            Database.InsertOrReplace(entity);
          return entity.Id;
      }

      public void SetInactive(County entity)
      {
          throw new NotImplementedException();
      }

      public void SetActive(County entity)
      {
          throw new NotImplementedException();
      }

      public void SetAsDeleted(County entity)
      {
          throw new NotImplementedException();
      }

      public County GetById(Guid id, bool includeDeactivated = false)
      {
          return  Database.Get<County>(id);
      }

      public IEnumerable<County> GetAll(bool includeDeactivated = false)
      {
            return Database.GetAll<County>();
        }

        public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
      {
          throw new NotImplementedException();
      }

      public DateTime GetLastTimeItemUpdated()
      {
          throw new NotImplementedException();
      }

      public IEnumerable<County> GetItemUpdated(DateTime dateTime)
      {
          throw new NotImplementedException();
      }

      public int GetCount(bool includeDeactivated = false)
      {
          throw new NotImplementedException();
      }

      public IPaginatedList<County> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
      {
          throw new NotImplementedException();
      }
    }
}

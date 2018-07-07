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
  public  class ConstituencyRepository: IConstituencyRepository
    {
      public ConstituencyRepository(Database database)
      {
          Database = database;
      }

      private Database Database { get; }
        public ValidationResultInfo Validate(Constituency itemToValidate)
      {
          throw new NotImplementedException();
      }

      public Guid Save(Constituency entity, bool? isSync = null)
      {
          Database.InsertOrReplace(entity, typeof (Constituency));
          return entity.Id;
      }

      public void SetInactive(Constituency entity)
      {
          throw new NotImplementedException();
      }

      public void SetActive(Constituency entity)
      {
          throw new NotImplementedException();
      }

      public void SetAsDeleted(Constituency entity)
      {
          throw new NotImplementedException();
      }

      public Constituency GetById(Guid id, bool includeDeactivated = false)
      {
          return Database.Get<Constituency>(id);
      }

      public IEnumerable<Constituency> GetAll(bool includeDeactivated = false)
      {
            return Database.GetAll<Constituency>();
        }

      public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
      {
          throw new NotImplementedException();
      }

      public DateTime GetLastTimeItemUpdated()
      {
          throw new NotImplementedException();
      }

      public IEnumerable<Constituency> GetItemUpdated(DateTime dateTime)
      {
          throw new NotImplementedException();
      }

      public int GetCount(bool includeDeactivated = false)
      {
          throw new NotImplementedException();
      }

      public IPaginatedList<Constituency> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
      {
          throw new NotImplementedException();
      }
    }
}

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
 public   class RaceRepository : IRaceRepository
    {
     public RaceRepository(Database database)
     {
         Database = database;
     }

     private Database Database { get; }
        public ValidationResultInfo Validate(Race itemToValidate)
     {
         throw new NotImplementedException();
     }

     public Guid Save(Race entity, bool? isSync = null)
     {
            Database.InsertOrReplace(entity);
         return entity.Id;
     }

     public void SetInactive(Race entity)
     {
         throw new NotImplementedException();
     }

     public void SetActive(Race entity)
     {
         throw new NotImplementedException();
     }

     public void SetAsDeleted(Race entity)
     {
         throw new NotImplementedException();
     }

     public Race GetById(Guid id, bool includeDeactivated = false)
     {
         return Database.Get<Race>(id);
     }

     public IEnumerable<Race> GetAll(bool includeDeactivated = false)
     {
         return Database.GetAll<Race>();
     }

     public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
     {
         throw new NotImplementedException();
     }

     public DateTime GetLastTimeItemUpdated()
     {
         throw new NotImplementedException();
     }

     public IEnumerable<Race> GetItemUpdated(DateTime dateTime)
     {
         throw new NotImplementedException();
     }

     public int GetCount(bool includeDeactivated = false)
     {
         throw new NotImplementedException();
     }

     public IPaginatedList<Race> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
     {
         throw new NotImplementedException();
     }
    }
}

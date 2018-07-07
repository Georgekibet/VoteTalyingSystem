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
   public class RegionRepository : IRegionRepository
    {
       public RegionRepository(Database database)
       {
           Database = database;
       }

       private Database Database { get; }
        public ValidationResultInfo Validate(Region itemToValidate)
       {
           throw new NotImplementedException();
       }

       public Guid Save(Region entity, bool? isSync = null)
       {
            Database.InsertOrReplace(entity);
           return entity.Id;
       }

       public void SetInactive(Region entity)
       {
           throw new NotImplementedException();
       }

       public void SetActive(Region entity)
       {
           throw new NotImplementedException();
       }

       public void SetAsDeleted(Region entity)
       {
           throw new NotImplementedException();
       }

       public Region GetById(Guid id, bool includeDeactivated = false)
       {
          return Database.Get<Region>(id);
       }

       public IEnumerable<Region> GetAll(bool includeDeactivated = false)
       {
            return Database.GetAll<Region>();
        }

        public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public DateTime GetLastTimeItemUpdated()
       {
           throw new NotImplementedException();
       }

       public IEnumerable<Region> GetItemUpdated(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public int GetCount(bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public IPaginatedList<Region> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }
    }
}

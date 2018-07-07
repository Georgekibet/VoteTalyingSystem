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
   public class ElectionRepository : IElectionRepository
    {
       public ElectionRepository(Database database)
       {
           Database = database;
       }

       private Database Database { get; }
        public ValidationResultInfo Validate(Election itemToValidate)
       {
           throw new NotImplementedException();
       }

       public Guid Save(Election entity, bool? isSync = null)
       {
           Database.InsertOrReplace(entity);
           return entity.Id;
       }

       public void SetInactive(Election entity)
       {
           throw new NotImplementedException();
       }

       public void SetActive(Election entity)
       {
           throw new NotImplementedException();
       }

       public void SetAsDeleted(Election entity)
       {
           throw new NotImplementedException();
       }

       public Election GetById(Guid id, bool includeDeactivated = false)
       {
           return Database.Get<Election>(id);

        }

        public IEnumerable<Election> GetAll(bool includeDeactivated = false)
       {
          return  Database.GetAll<Election>();
       }

       public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public DateTime GetLastTimeItemUpdated()
       {
           throw new NotImplementedException();
       }

       public IEnumerable<Election> GetItemUpdated(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public int GetCount(bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public IPaginatedList<Election> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }
    }
}

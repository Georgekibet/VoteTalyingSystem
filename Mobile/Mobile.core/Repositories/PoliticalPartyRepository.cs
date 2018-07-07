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
 public   class PoliticalPartyRepository : IPoliticalPartyRepository
    {
     public PoliticalPartyRepository(Database database)
     {
         Database = database;
     }

     private Database Database { get; }
        public ValidationResultInfo Validate(PoliticalParty itemToValidate)
     {
         throw new NotImplementedException();
     }

     public Guid Save(PoliticalParty entity, bool? isSync = null)
     {
            Database.InsertOrReplace(entity);
         return entity.Id;
     }

     public void SetInactive(PoliticalParty entity)
     {
         throw new NotImplementedException();
     }

     public void SetActive(PoliticalParty entity)
     {
         throw new NotImplementedException();
     }

     public void SetAsDeleted(PoliticalParty entity)
     {
         throw new NotImplementedException();
     }

     public PoliticalParty GetById(Guid id, bool includeDeactivated = false)
     {
      return Database.Get<PoliticalParty>(id);
     }

     public IEnumerable<PoliticalParty> GetAll(bool includeDeactivated = false)
     {
        return Database.GetAll<PoliticalParty>();
     }

     public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
     {
         throw new NotImplementedException();
     }

     public DateTime GetLastTimeItemUpdated()
     {
         throw new NotImplementedException();
     }

     public IEnumerable<PoliticalParty> GetItemUpdated(DateTime dateTime)
     {
         throw new NotImplementedException();
     }

     public int GetCount(bool includeDeactivated = false)
     {
         throw new NotImplementedException();
     }

     public IPaginatedList<PoliticalParty> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
     {
         throw new NotImplementedException();
     }
    }
}

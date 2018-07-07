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
   public class CandidateRepository : ICandidateRepository
    {
       public CandidateRepository(Database database)
       {
           Database = database;
       }

       private Database Database { get; }
        public ValidationResultInfo Validate(Candidate itemToValidate)
       {
           throw new NotImplementedException();
       }

       public Guid Save(Candidate entity, bool? isSync = null)
       {
           Database.InsertOrReplace(entity, typeof (Candidate));
           return entity.Id;
       }

       public void SetInactive(Candidate entity)
       {
           throw new NotImplementedException();
       }

       public void SetActive(Candidate entity)
       {
           throw new NotImplementedException();
       }

       public void SetAsDeleted(Candidate entity)
       {
           throw new NotImplementedException();
       }

       public Candidate GetById(Guid id, bool includeDeactivated = false)
       {
        return   Database.Get<Candidate>(id);
       }

       public IEnumerable<Candidate> GetAll(bool includeDeactivated = false)
       {
           return Database.GetAll<Candidate>();
       }

       public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public DateTime GetLastTimeItemUpdated()
       {
           throw new NotImplementedException();
       }

       public IEnumerable<Candidate> GetItemUpdated(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public int GetCount(bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public IPaginatedList<Candidate> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }
    }
}

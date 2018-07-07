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
   public class SettingsRepository : ISettingsRepository
    {
       public SettingsRepository(Database database)
       {
           Database = database;
       }

       private Database Database { get; }
        public ValidationResultInfo Validate(Settings itemToValidate)
       {
           throw new NotImplementedException();
       }

       public Guid Save(Settings entity, bool? isSync = null)
       {
            Database.InsertOrReplace(entity);
           return entity.Id;
       }

       public void SetInactive(Settings entity)
       {
           throw new NotImplementedException();
       }

       public void SetActive(Settings entity)
       {
           throw new NotImplementedException();
       }

       public void SetAsDeleted(Settings entity)
       {
           throw new NotImplementedException();
       }

       public Settings GetById(Guid id, bool includeDeactivated = false)
       {
           return Database.Get<Settings>(id);
       }

       public IEnumerable<Settings> GetAll(bool includeDeactivated = false)
       {
            return Database.GetAll<Settings>();
        }

       public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public DateTime GetLastTimeItemUpdated()
       {
           throw new NotImplementedException();
       }

       public IEnumerable<Settings> GetItemUpdated(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public int GetCount(bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public IPaginatedList<Settings> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public Settings GetByKey(SettingsKeys key)
       {
           throw new NotImplementedException();
       }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.core.SQLiteDatabase;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using vts.Shared.Utility;

namespace Mobile.core.Repositories
{
   public class UserRepository : IUserRepository
   {
       private Database Database { get; }

       public UserRepository(Database database)
       {
           Database = database;
       }

       public ValidationResultInfo Validate(User itemToValidate)
       {
           throw new NotImplementedException();
       }

       public Guid Save(User entity, bool? isSync = null)
       {
           Database.InsertOrReplace(entity, typeof (User));
           return entity.Id;

       }

       public void SetInactive(User entity)
       {
           throw new NotImplementedException();
       }

       public void SetActive(User entity)
       {
           throw new NotImplementedException();
       }

       public void SetAsDeleted(User entity)
       {
           throw new NotImplementedException();
       }

       public User GetById(Guid id, bool includeDeactivated = false)
       {
           return Database.Get<User>(id);
       }

       public IEnumerable<User> GetAll(bool includeDeactivated = false)
       {
              return         Database.GetAll<User>();
            
       }

       public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public DateTime GetLastTimeItemUpdated()
       {
           throw new NotImplementedException();
       }

       public IEnumerable<User> GetItemUpdated(DateTime dateTime)
       {
           throw new NotImplementedException();
       }

       public int GetCount(bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public IPaginatedList<User> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }

       public User Login(string username, string password)
       {
           throw new NotImplementedException();
       }

       public void ChangePassword(string oldPassword, string newPassword, string userName)
       {
           throw new NotImplementedException();
       }

       public void ResetUserPassword(Guid userId)
       {
           throw new NotImplementedException();
       }

       public User GetUser(string username)
       {
           throw new NotImplementedException();
       }

       public User ConstructCustomPrincipal(string userName)
       {
           throw new NotImplementedException();
       }

       public List<User> GetByUserType(UserType userType, bool includeDeactivated = false)
       {
           throw new NotImplementedException();
       }
    }
}

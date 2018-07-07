using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using vts.Core.Shared.Entities.Master;
using vts.Data.Context;
using vts.Shared.Repository;
using vts.Shared.Services;

namespace vts.Data.Repository.MasterData
{
    public class UserRepository : BaseRepository<User, UserRef>, IUserRepository
    {
        public UserRepository(ContextConnection contextConnection)
            : base(contextConnection)
        {
        }

        protected override Func<User, List<User>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);
                    bool dupeName = itemsToCheck.Any(n => n.Username.ToLower() == itemToCheck.Username.ToLower());
                    if (dupeName) validationResults.Add(new ValidationResult("duplicate user name found"));
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<User>, List<User>> SearchFunc
        {
            get
            {
                return (searchText, allItems) =>
                {
                    if (string.IsNullOrEmpty(searchText)) return allItems;
                    string st = searchText.ToLower();

                    return
                        allItems.Where(
                            n => n.Username.ToLower().Contains(st))
                                .ToList();
                };
            }
        }

        public Guid Save(User entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "User not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("UserRepository"))
            {
                User u = GetById(entity.Id);
                if (u == null)
                {
                    entity.Status = EntityStatus.Active;
                    entity.DateCreated = dt;
                }
                entity.DateLastUpdated = dt;
                ctx.UpdateGraph(entity);

                ctx.SaveChanges();
            }

            return entity.Id;
        }

        private void SetStatus(User entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("UserRepository"))
            {
                User c = ctx.Users.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(User entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(User entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(User entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override User GetById(Guid id, bool includeDeactivated = false)
        {
            User user = null;
            using (var ctx = GetVtsContext("UserRepository"))
            {
                user = CtxSetup(ctx.Users).FirstOrDefault(n => n.Id == id);
            }
            return user;
        }

        public override IEnumerable<User> GetAll(bool includeDeactivated = false)
        {
            List<User> users = new List<User>();

            using (var ctx = GetVtsContext("UserRepository"))
            {
                if (includeDeactivated)
                    users =
                    CtxSetup(ctx.Users)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    users =
                    CtxSetup(ctx.Users)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return users;
        }

        public ValidationResultInfo Validate(User itemToValidate)
        {
            return base.Validate(itemToValidate, GetAll(true).ToList());
        }

        public User Login(string username, string password)
        {
            var result = GetAll().FirstOrDefault(n => n.Status == EntityStatus.Active
                                                && n.Username.ToLower() == username.ToLower()
                                                && n.Password.ToLower() == password.ToLower());

            return result;
        }

        public void ChangePassword(string oldPassword, string newPassword, string userName)
        {
            User user = GetAll().FirstOrDefault(n => n.Username.ToLower() == userName);
            if (user == null)
                throw new ValidationException("Failed to locate username");
            if (user.Password != oldPassword)
                throw new ValidationException("Please enter correct old password");
            user.Password = newPassword;
            Save(user);
        }

        public static string Encrypt(string input)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(input)));
        }

        public void ResetUserPassword(Guid userId)
        {
            User user = GetAll().FirstOrDefault(n => n.Id == userId);
            if (user != null)
            {
                user.Password = Encrypt("12345678");
                Save(user);
            }
        }

        public User GetUser(string username)
        {
            return GetAll().FirstOrDefault(n => n.Username.ToLower() == username.ToLower());
        }

        public User ConstructCustomPrincipal(string userName)
        {
            var allowedUserTypes = new List<UserType>
                                   {
                                       UserType.Admin,
                                   };
            return
                GetAll()
                    .FirstOrDefault(
                        n => allowedUserTypes.Contains(n.UserType) && n.Username.ToLower() == userName.ToLower());
        }

        public List<User> GetByUserType(UserType userType, bool includeDeactivated = false)
        {
            return GetAll(includeDeactivated).Where(n => n.UserType == userType).ToList();
        }
    }
}
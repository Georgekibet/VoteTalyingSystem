using System;
using System.Collections.Generic;
using vts.Core.Shared.Entities.Master;

namespace vts.Shared.Repository
{
    public interface IUserRepository : IMasterRepository<User>
    {
        User Login(string username, string password);

        void ChangePassword(string oldPassword, string newPassword, string userName);

        void ResetUserPassword(Guid userId);

        User GetUser(string username);

        User ConstructCustomPrincipal(string userName);

        List<User> GetByUserType(UserType userType, bool includeDeactivated = false);
    }
}
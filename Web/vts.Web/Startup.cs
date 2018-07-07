using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using vts.Web.Models;

[assembly: OwinStartupAttribute(typeof(vts.Web.Startup))]

namespace vts.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
          //  CreateRolesandUsers();
        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Create first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Create Admin User               

                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "admin@rubem.co.ke";
                user.FirstName = "Administrator";
                user.LastName = "Administrator";
                user.Status = ApplicationUser.UserStatus.Active;
                user.MainRole = MainRole.Admin;

                string userPWD = "Admin1234#";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }
            // create Manager role    
            if (!roleManager.RoleExists("Client"))
            {
                var role = new IdentityRole();
                role.Name = "Client";
                roleManager.Create(role);

            }

            // create Clerk role    
            if (!roleManager.RoleExists("Clerk"))
            {
                var role = new IdentityRole();
                role.Name = "Clerk";
                roleManager.Create(role);

            }

        }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ozn.MvcWebUI.Identity
{
    public class IdentityInitializer : CreateDatabaseIfNotExists<IdentityDataContext>
    {
        protected override void Seed(IdentityDataContext context)
        {
            // Roller
            if (!context.Roles.Any(i => i.Name == "admin"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);//ApplicationRole yazmak önemli e
                var role = new ApplicationRole() { Name = "admin", Description = "admin rolü" };
                manager.Create(role);
            }

            if (!context.Roles.Any(i => i.Name == "user"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole() { Name = "user", Description = "user rolü" }; ;
                manager.Create(role);
            }

            // User                  
            if (!context.Users.Any(i => i.Name == "cihanozan")) 
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser() { Name = "cihan", Surname = "ozan", UserName = "cihanozan", Email = "cihanozan2002@gmail.com" };

                manager.Create(user, "123456789");
                manager.AddToRole(user.Id, "admin");
                manager.AddToRole(user.Id, "user");
            }//bu user admin ve usera ait

            if (!context.Users.Any(i => i.Name == "burhanozan"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser() { Name = "burhan", Surname = "ozan", UserName = "burhanozan", Email = "burhanozn714@gmail.com" };

                manager.Create(user, "123456789");
                manager.AddToRole(user.Id, "user");//sadece usera gidecek
            }

          base.Seed(context);
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rocky.Domain;
using Rocky.Utils;
using System;
using System.Linq;

namespace Rocky.DAL.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityUser> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(WC.AdminRole).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityUser(WC.AdminRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityUser(WC.CustomerRole)).GetAwaiter().GetResult();
                }
                else
                {
                    return;
                }

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FullName = "Admin admin",
                    PhoneNumber = "123",
                    PhoneNumberConfirmed = true
                }, "hZaYNU*5zWD8H6Y").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUser.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, WC.AdminRole).GetAwaiter().GetResult();
            }
            catch(Exception exp)
            {

            }
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager){
            if(!userManager.Users.Any()){
                var user = new AppUser{
                    DisplayName = "Beth",
                    Email = "beth@test.com",
                    UserName = "beth@test.com",
                    Address = new Address{
                        FirstName = "Beth",
                        LastName = "Child",
                        Street = "10 The street",
                        City = "Newyork",
                        State = "NY",
                        Zipcode = "123456"
                    }
                };
                
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }

        }
    }
}
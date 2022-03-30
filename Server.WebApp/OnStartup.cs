using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Server.WebApp
{
  public class OnStartup
  {
    public static async Task CreateRoles( UserManager<ApplicationUser> UserManager, 
      RoleManager<ApplicationRole> RoleManager, IConfiguration configuration )
    {
      //initializing custom roles 
      var roleNames = configuration.GetSection( "InitialRoles" ).Get<string[]>();
      IdentityResult roleResult;

      foreach( var roleName in roleNames )
      {
        var roleExist = await RoleManager.RoleExistsAsync( roleName );
        if( !roleExist )
        {
          //create the roles and seed them to the database: Question 1
          var role = new ApplicationRole();
          role.Name = roleName;
          roleResult = await RoleManager.CreateAsync( role );
        }
      }

      //Here you could create a super user who will maintain the web app
      var poweruser = new ApplicationUser
      {
        UserName = configuration.GetSection( "PowerUser" ).GetSection( "UserName" ).Value,
        Email = configuration.GetSection( "PowerUser" ).GetSection( "Email" ).Value,
      };
      //Ensure you have these values in your appsettings.json file
      string userPWD = configuration.GetSection( "PowerUser" ).GetSection( "Password" ).Value;
      var _user = await UserManager.FindByEmailAsync( configuration.GetSection( "PowerUser" ).GetSection( "Email" ).Value );

      if( _user == null )
      {
        var createPowerUser = await UserManager.CreateAsync( poweruser, userPWD );
        if( createPowerUser.Succeeded )
        {
          //here we tie the new user to the role
          await UserManager.AddToRoleAsync( poweruser, "Admin" );

        }
      }
    }


  }
}

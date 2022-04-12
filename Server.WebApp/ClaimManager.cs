using System.Security.Claims;

namespace Server.WebApp
{
  public class ClaimManager
  {

    public static ApplicationUser GetUserFromClaims( ClaimsPrincipal user, ApplicationDbContext context )
    {
      //TODO add try catch
      var userEmail = user.Claims.First(c => c.Type.Equals( "email" )).Value;

      return context.Users.First(c => c.Email.Equals( userEmail ));
    }

  }
}

using System.Security.Claims;

namespace Server.WebApp
{
  public class ClaimManager
  {

    public static ApplicationUser GetUserFromClaims( ClaimsPrincipal user, ApplicationDbContext context )
    {
      var userEmail = user.Claims.Where( c => c.Type.Equals( "email" ) ).First().Value;

      return context.Users.Where( c => c.Email.Equals( userEmail ) ).First();
    }

  }
}

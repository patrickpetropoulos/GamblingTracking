using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Server.WebApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


public class Program
{
  public async static Task Main( string[] args )
  {
    var host = CreateHostBuilder( args ).Build();

    using( var scope = host.Services.CreateScope() )
    {
      var services = scope.ServiceProvider;

      try
      {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if( context.Database.IsSqlServer() )
        {
          context.Database.Migrate();
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        //await ApplicationDbContextSeed.SeedDefaultUserAsync( userManager, roleManager );
        //await ApplicationDbContextSeed.SeedSampleDataAsync( context );
      }
      catch( Exception ex )
      {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogError( ex, "An error occurred while migrating or seeding the database." );

        throw;
      }
    }

    await host.RunAsync();
  }

  public static IHostBuilder CreateHostBuilder( string[] args ) =>
      Host.CreateDefaultBuilder( args )
          .ConfigureWebHostDefaults( webBuilder =>
               webBuilder.UseStartup<Startup>() );


}
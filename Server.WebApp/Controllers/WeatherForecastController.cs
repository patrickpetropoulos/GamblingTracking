using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Managers;
using Server.Infrastructure.SQL.Managers;

namespace Server.WebApp.Controllers
{
  [ApiController]
  [Route( "[controller]" )]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;

    public WeatherForecastController( ILogger<WeatherForecastController> logger, 
      UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration )
    {
      _logger = logger;
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;

      
    }

    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
    [HttpGet( Name = "GetWeatherForecast" )]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {

      await  OnStartup.CreateRoles( _userManager, _roleManager, _configuration );

      var seed = new SeedDatabase();
      await seed.Seed();

      return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays( index ),
        TemperatureC = Random.Shared.Next( -20, 55 ),
        Summary = Summaries[Random.Shared.Next( Summaries.Length )]
      } )
      .ToArray();
    }
  }
}
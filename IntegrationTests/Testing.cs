using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NUnit.Framework;
using Respawn;
using Server.WebApp;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Server.Domain.Utilities;

namespace IntegrationTests;

[SetUpFixture]
public class Testing
{
  private static IConfigurationRoot _configuration = null!;
  private static IServiceScopeFactory _scopeFactory = null!;
  private static Checkpoint _checkpoint = null!;
  private static string? _currentUserId;

  public static WebApplicationFactory<Program>? _app = null;
  private static string _token;

  public static HttpClient GetClient()
  {
    var client = Testing._app?.CreateClient();
    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {_token}");

    return client;
  }
  
  public static HttpClient GetClient(string token)
  {
    var client = Testing._app?.CreateClient();
    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

    return client;
  }
  
  
  [OneTimeSetUp]
  public async Task RunBeforeAnyTests()
  {
    _app = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder =>
      {

        builder.UseEnvironment("Testing");

        //var startup = new Startup( _configuration );

        //builder.UseStartup<Startup>();


        // ... Configure test services
      });
    
    //put code here to create app
    //delete all users and stuff from database
    //delete all other data from database
    //seed database
      
    var configBuilder = new ConfigurationBuilder()
      .SetBasePath( Directory.GetCurrentDirectory() )
      .AddJsonFile( "appsettings.Testing.json", true, true )
      .AddEnvironmentVariables();

    var _configuration = configBuilder.Build();

    var username = _configuration["PowerUser:Username"];
    var password = _configuration["PowerUser:Password"];
      
    var client = _app?.CreateClient();
    var response = await client.GetAsync( $"/token?username={username}&password={password}" );
      
    var result = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse(result);

    _token = JSONUtilities.GetString(json, "accessToken");
  }

  [OneTimeTearDown]
  public async Task RunAfterAllTests()
  {
    // var client = Testing._app?.CreateClient();
    //
    // var response = await client.GetAsync( "/weatherForecast" );
    // var result = await response.Content.ReadAsStringAsync();
    // var json = JArray.Parse(result);
    
  }

    //var builder = new ConfigurationBuilder()
    //    .SetBasePath( Directory.GetCurrentDirectory() )
    //    .AddJsonFile( "appsettings.Development.json", true, true )
    //    .AddEnvironmentVariables();

    //_configuration = builder.Build();

    //var startup = new Startup( _configuration );

    //var services = new ServiceCollection();

    //services.AddSingleton( Mock.Of<IWebHostEnvironment>( w =>
    //      w.EnvironmentName == "Development" &&
    //      w.ApplicationName == "Server.WebApp" ) );

    //services.AddLogging();

    //startup.ConfigureServices( services );

    // Replace service registration for ICurrentUserService
    // Remove existing registration
    //var currentUserServiceDescriptor = services.FirstOrDefault( d =>
    //     d.ServiceType == typeof( ICurrentUserService ) );

    //if( currentUserServiceDescriptor != null )
    //{
    //  services.Remove( currentUserServiceDescriptor );
    //}

    //// Register testing version
    //services.AddTransient( provider =>
    //     Mock.Of<ICurrentUserService>( s => s.UserId == _currentUserId ) );

    //_scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

    //_checkpoint = new Checkpoint
    //{
    //  TablesToIgnore = new[] { "__EFMigrationsHistory" }
    //};

    //EnsureDatabase();

  private static void EnsureDatabase()
  {
    using var scope = _scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();
  }

  //public static async Task<TResponse> SendAsync<TResponse>( IRequest<TResponse> request )
  //{
  //  using var scope = _scopeFactory.CreateScope();

  //  var mediator = scope.ServiceProvider.GetRequiredService<IServer>();

  //  return await mediator.Send( request );
  //}

  //public static async Task<string> RunAsDefaultUserAsync()
  //{
  //  return await RunAsUserAsync( "test@local", "Testing1234!", Array.Empty<string>() );
  //}

  //public static async Task<string> RunAsAdministratorAsync()
  //{
  //  return await RunAsUserAsync( "administrator@local", "Administrator1234!", new[] { "Administrator" } );
  //}

  //public static async Task<string> RunAsUserAsync( string userName, string password, string[] roles )
  //{
  //  using var scope = _scopeFactory.CreateScope();

  //  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

  //  var user = new ApplicationUser { UserName = userName, Email = userName };

  //  var result = await userManager.CreateAsync( user, password );

  //  if( roles.Any() )
  //  {
  //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  //    foreach( var role in roles )
  //    {
  //      await roleManager.CreateAsync( new IdentityRole( role ) );
  //    }

  //    await userManager.AddToRolesAsync( user, roles );
  //  }

  //  if( result.Succeeded )
  //  {
  //    _currentUserId = user.Id;

  //    return _currentUserId;
  //  }

  //  var errors = string.Join( Environment.NewLine, result.ToApplicationResult().Errors );

  //  throw new Exception( $"Unable to create {userName}.{Environment.NewLine}{errors}" );
  //}

  //public static async Task ResetState()
  //{
  //  await _checkpoint.Reset( _configuration.GetConnectionString( "DefaultConnection" ) );

  //  _currentUserId = null;
  //}

  public static async Task<TEntity?> FindAsync<TEntity>( params object[] keyValues )
      where TEntity : class
  {
    using var scope = _scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    return await context.FindAsync<TEntity>( keyValues );
  }

  public static async Task AddAsync<TEntity>( TEntity entity )
      where TEntity : class
  {
    using var scope = _scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Add( entity );

    await context.SaveChangesAsync();
  }

  public static async Task<int> CountAsync<TEntity>() where TEntity : class
  {
    using var scope = _scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    return await context.Set<TEntity>().CountAsync();
  }

  [OneTimeTearDown]
  public void RunAfterAnyTests()
  {
  }
}


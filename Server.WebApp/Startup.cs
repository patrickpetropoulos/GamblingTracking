
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.WebApp
{
  public class Startup
  {
    public Startup( IConfiguration configuration )
    {
      Configuration = configuration;

      //Need to set this up if dont want all schemas text crap in JWT claims
      //https://github.com/dotnet/aspnetcore/issues/4660
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
      JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices( IServiceCollection services )
    {
      // Add services to the container.

      services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      if( Configuration.GetValue<bool>( "UseInMemoryDatabase" ) )
      {
        //fix this at some point
        //services.AddDbContext<ApplicationDbContext>( options =>
             //options.UseInMemoryDatabase( "GamblingLogDb" ) );
      }
      else
      {
        var test = Configuration["ConnectionStrings:SQL"];

        services.AddDbContext<ApplicationDbContext>( options =>
             options.UseSqlServer(
                 Configuration.GetConnectionString( "SQL" ),
                 b => b.MigrationsAssembly( typeof( ApplicationDbContext ).Assembly.FullName ) ) );
      }

      //services.AddScoped<IApplicationDbContext>( provider => provider.GetRequiredService<ApplicationDbContext>() );

      //services.AddScoped<IDomainEventService, DomainEventService>();

      //services
      //    .AddDefaultIdentity<ApplicationUser>()
      //    .AddRoles<IdentityRole>()
      //    .AddEntityFrameworkStores<ApplicationDbContext>();

      services.AddIdentityCore<ApplicationUser>()
                      .AddRoles<ApplicationRole>()
                      .AddEntityFrameworkStores<ApplicationDbContext>()
                      .AddDefaultTokenProviders();


      services.AddRazorPages();

      //allow all for COrs
      services.AddCors( options => options.AddPolicy( "AllowAll", p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader() ) );

      //services.AddDbContext<ApplicationDbContext>( options =>
      //                 options.UseSqlServer(
      //                     Configuration.GetConnectionString( "SQL" ) ) );

      //builder.Services.AddIdentity<ApplicationUser, ApplicationRole>( o =>
      //              {
      //                // configure identity options
      //                o.Password.RequireDigit = false;
      //                o.Password.RequireLowercase = false;
      //                o.Password.RequireUppercase = false;
      //                o.Password.RequireNonAlphanumeric = false;
      //                o.Password.RequiredLength = 6;                
      //              } )
      //            .AddEntityFrameworkStores<ApplicationDbContext>()
      //            .AddDefaultTokenProviders();

      services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                     .AddJwtBearer( options =>
                     {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes( Configuration["JWT"] ) ),
                         ClockSkew = TimeSpan.Zero
                       };
                     } );

      services.AddAuthorization( options =>
      {
        options.AddPolicy( "IsAdmin", policy => policy.RequireClaim( "role", "Admin" ) );
      } );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider )
    {

      ServerSystem.CreateInstance( serviceProvider, Configuration );
      //ServerSystem.Instance.SetLogger( builder.Services. );
      //ServerSystem.Instance.SetServiceProvider( app.ApplicationServices );

      // Configure the HTTP request pipeline.
      if( env.IsDevelopment() )
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      //fix when have a frontend
      app.UseCors( "AllowAll" );
      app.UseHttpsRedirection();

      app.UseAuthorization();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseSwagger();
      app.UseSwaggerUI( x =>
      {
        x.SwaggerEndpoint( "/swagger/v1/swagger.json", "TimCo API v1" );
      } );

      app.UseEndpoints( endpoints =>
      {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}" );
        endpoints.MapRazorPages();
      } );

      //app.UseCors();

      //app.Run();

      var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

      OnStartup.CreateRoles( userManager, roleManager, Configuration ).GetAwaiter().GetResult();

    }
  }
}

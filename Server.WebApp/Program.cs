using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Server.WebApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

//Need to set this up if dont want all schemas text crap in JWT claims
//https://github.com/dotnet/aspnetcore/issues/4660
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder( args );
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>( options =>
                 options.UseSqlServer(
                     builder.Configuration.GetConnectionString( "SQL" ) ) );

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

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
               .AddJwtBearer( options =>
               {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes( builder.Configuration["JWT"] ) ),
                   ClockSkew = TimeSpan.Zero
                 };
               } );

builder.Services.AddAuthorization( options =>
{
  options.AddPolicy( "IsAdmin", policy => policy.RequireClaim( "role", "Admin" ) );
} );


var app = builder.Build();

// Configure the HTTP request pipeline.
if( app.Environment.IsDevelopment() )
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors( "AllowAll" );
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();




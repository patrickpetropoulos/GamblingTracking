using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Server.Application.Managers;

namespace Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasinoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public CasinoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpGet]
    //[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
    [Authorize]
    //[Produces("application/json")]
    public async Task<IActionResult> GetAllCasinos()
    {
        var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( "CasinoManager" );

        var casinos = await casinoManager.GetAllCasinos();

        var json = new JArray();
        foreach (var casino in casinos)
        {
            json.Add(casino.ToJson());
        }
        
        //This can't be correct, can it??
        //return Ok(json);
        
        //How to more properly define it?????
        return new JsonResult(json);

    }
    
    
}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Sports.Application.Espn;

namespace Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspnController : ControllerBase
{

    [HttpGet("mlb")]
    public async Task<IActionResult> GetAllGamesByDate()
    {

        var url = "http://site.api.espn.com/apis/site/v2/sports/baseball/mlb/scoreboard?dates=20220407";
        
        
        
        var client = new HttpClient();
        var data = await client.GetStringAsync(url);

        var json = JObject.Parse(data);

        var result = EspnApiParser.ParseMlbGames(json);
        
        return new JsonResult(result);
    }
    
    
}
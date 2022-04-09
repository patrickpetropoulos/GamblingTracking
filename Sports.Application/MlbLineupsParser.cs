using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Sports.Application
{
    public class MlbLineupsParser
    {
        public async Task ParseLineups(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var divs = doc.DocumentNode.SelectNodes("//div[contains(@class, 'row lineups')]");

            foreach (var div in divs)
            {
                foreach (var childNode in div.ChildNodes)
                {
                    var t = "jhh";
                }
            }
            
            
            // foreach (var game in divs)
            // {
            //     var t = game.Attributes["data-gamepk"].Value;
            //
            //     var childs = game.ChildNodes.FirstOrDefault(c => c.InnerHtml.Contains("starting-lineups__team-names"));
            //
            //     var teamNodes = childs.ChildNodes.Where(c => c.InnerHtml.Contains("starting-lineups__team-name--link")).ToList();
            //     
            //     
            //     var teams = teamNodes[0].InnerText.Trim();
            //
            //     var teamNames = teams.Split('@');
            //
            //     var homeTeam = teamNames[0].Trim();
            //     var awayTeam = teamNames[1].Trim();
            //
            //     var timeNode =
            //         game.SelectNodes("//div[contains(@class,'starting-lineups__game-date-time')]");
            //
            //
            //     var pitcherNodes =game.SelectNodes("//div[contains(@class,'starting-lineups__pitcher-overview')]");
            //
            // }
        }
        
        
        
        
    }
}
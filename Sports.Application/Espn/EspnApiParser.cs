using Newtonsoft.Json.Linq;
using Server.Domain.Utilities;

namespace Sports.Application.Espn
{
    public class EspnApiParser
    {
        public static JObject ParseMlbGames(JObject json)
        {
            var result = new JObject();
            var resultGames = new JArray();

            var games = (JArray)json["events"];

            foreach (var game in games)
            {
                var gameObject = (JObject) game;
                var jObject = new JObject();
                
                JSONUtilities.Set(jObject, "season", gameObject["season"]);

                var competitions = (JArray) gameObject["competitions"];
                
                JSONUtilities.Set(jObject, "date", JSONUtilities.GetDateTimeOffset(gameObject,"date"));

                JSONUtilities.Set(jObject, "venue", ((JArray)gameObject["competitions"])[0]["venue"]["fullname"]);
                
                
                resultGames.Add(jObject);
                
            }
            result.Add("games", resultGames);
            return result;
        }
    }
}
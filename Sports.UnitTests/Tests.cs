using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Sports.Application;
using Sports.Application.Espn;

namespace Sports.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task Test1()
        {
            var downloader = new MlbLineupsDownloader();
            var html = await downloader.DownloadHtml(new DateTime(2022, 4, 8));

            var parser = new MlbLineupsParser();
            await parser.ParseLineups(html);
        }
        
        [Test]
        public void ValidateMovieParser_ParsesEspnMlbFileCorrectly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Sports.UnitTests.TestData.Espn.Espn MLB Api 04-11-2022 situation in blue jays yankees game.json"))
            using (var reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                var result = EspnApiParser.ParseMlbGames(JObject.Parse(text));
                var t = 3;

                Assert.AreEqual(12, ((JArray)result["games"]).Count);
            }
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Server.Domain.Entities;
using Server.Domain.Utilities;
using Server.WebApp;

namespace IntegrationTests
{
  public class CasinoTests
  {
    [SetUp]
    public async Task Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
      var client = Testing.GetClient();
      var response = await client.GetAsync( "/weatherForecast" );
      var result = await response.Content.ReadAsStringAsync();
      var json = JArray.Parse(result);
      Assert.Pass();
    }
    
    [Test]
    public async Task GetAllCasinos_ValidateReturnSameList()
    {
      var client = Testing.GetClient();
      var response = await client.GetAsync( "/api/casino" );
      var result = await response.Content.ReadAsStringAsync();
      var json = JArray.Parse(result);

      var expected = new SeedDatabase()._casinoList;

      var actual = new List<Casino>();
      foreach (var token in json.Children<JObject>())
      {
        var casino = new Casino();
        casino.FromJson(token);
        actual.Add(casino);
      }

      foreach (var casino in expected)
      {
        var actualCas = actual.FirstOrDefault(c => c.Name.Equals(casino.Name) && c.CountryCode.Equals(casino.CountryCode));
        if (actualCas == null)
        {
          Assert.Fail();
        }
      }

      Assert.Pass();
    }
  }
}
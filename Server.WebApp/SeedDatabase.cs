using Server.Application.Managers;
using Server.Domain.Entities;
using System.Linq;
using Server.Domain.Testing;

namespace Server.WebApp
{
  public class SeedDatabase
  {
    public List<Casino> _casinoList;
    public List<CasinoGame> _casinoGamesList;
    public SeedDatabase()
    {
      _casinoList = ExampleData.GetListOfCasinos();
      _casinoGamesList = ExampleData.GetListOfCasinoGames();

    }


    public async Task Seed()
    {
      await SeedCasinos();
      await SeedCasinoGames();
      await SeedGamblingSessions();

    }
    public async Task SeedCasinos()
    {
      var casinoManager = ServerSystem.Instance.Get<ICasinoManager>( "CasinoManager" );

      var currentCasinos = await casinoManager.GetAllCasinos();
      if( !currentCasinos.Any() )
      {
        foreach( var casino in _casinoList )
        {
          await casinoManager.UpsertCasino( casino );
        }
      }
      else
      {
        _casinoList = currentCasinos;
      }
    }


    private async Task SeedCasinoGames()
    {
      var casinoGameManager = ServerSystem.Instance.Get<ICasinoGameManager>( "CasinoGameManager" );

      var currentGames = await casinoGameManager.GetAllCasinoGames();
      if( !currentGames.Any() )
      {
        foreach( var casinoGame in _casinoGamesList )
        {
          await casinoGameManager.UpsertCasinoGame( casinoGame );
        }
      }
      else
      {
        _casinoGamesList = currentGames;
      }
    }

    private async Task SeedGamblingSessions()
    {
      var session = new GamblingSession()
      {
        Casino = _casinoList[0],
        CasinoGame = _casinoGamesList[0],
        StartingAmount = 0,
        EndingAmount = 230.50M

      };


      var json = session.ToJson();


      var testSession = new GamblingSession();
      testSession.FromJson( json );



      var session2 = new GamblingSession()
      {
        Casino = _casinoList[1],
        StartingAmount = 02020,
        EndingAmount = 23044.50M

      };
      var json2 = session2.ToJson();


      var testSession2 = new GamblingSession();
      testSession2.FromJson( json2 );
    }
  }
}

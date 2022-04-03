using Server.Application.Managers;
using Server.Domain.Entities;
using System.Linq;

namespace Server.WebApp
{
  public class SeedDatabase
  {
    private List<Casino> _casinoList;
    private List<CasinoGame> _casinoGames;
    public SeedDatabase()
    {
      _casinoList = new List<Casino>();
      _casinoList.Add( new Casino()
      {
        Name = "Borgata",
        CountryCode = "US"
      } );
      _casinoList.Add( new Casino()
      {
        Name = "Bellagio",
        CountryCode = "US"
      } );
      _casinoList.Add( new Casino()
      {
        Name = "Montreal Casino",
        CountryCode = "CA"
      } );

      _casinoGames = new List<CasinoGame>();
      _casinoGames.Add( new CasinoGame()
      {
        Name = "Blackjack",
        HasSubType = false
      } );
      _casinoGames.Add( new CasinoGame()
      {
        Name = "Craps",
        HasSubType = false
      } );
      _casinoGames.Add( new CasinoGame()
      {
        Name = "Video Keno",
        HasSubType = true
      } );
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
        foreach( var casinoGame in _casinoGames )
        {
          await casinoGameManager.UpsertCasinoGame( casinoGame );
        }
      }
      else
      {
        _casinoGames = currentGames;
      }
    }

    private async Task SeedGamblingSessions()
    {
      var session = new GamblingSession()
      {
        Casino = _casinoList[0],
        CasinoGame = _casinoGames[0],
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

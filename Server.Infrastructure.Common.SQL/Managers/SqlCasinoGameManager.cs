using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Application.Managers;
using Server.Domain.Entities;
using Server.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.SQL.Managers
{
  public class SqlCasinoGameManager : Manager, ICasinoGameManager
  {
    private string _connectionString;
    public SqlCasinoGameManager( IConfiguration configuration ) : base( configuration )
    {

    }
    protected override void InitializeConfiguration()
    {
      _connectionString = Configuration.GetConnectionString( "SQL" );
    }
    public override void SetLogger( ILoggerFactory loggerFactory, string name )
    {
      base.SetLogger( loggerFactory, name );
    }

    public async Task<List<CasinoGame>> GetAllCasinoGames()
    {
      return await DatabaseUtilities.ExecuteAsync<List<CasinoGame>>( _connectionString,
                async ( c ) => await SelectAllCasinoGames( c ) );
    }

    public async Task UpsertCasinoGame( CasinoGame casinoGame )
    {
      if( casinoGame.Id <= 0 )
      {
        //what to do with int??? move this logic up, have an insert call???
        await DatabaseUtilities.ExecuteAsync<int>( _connectionString,
                async ( c ) => await InsertCasinoGame( c, casinoGame ) );
      }
    }

    // Database
    public async Task<List<CasinoGame>> SelectAllCasinoGames( SqlConnection sqlConnection )
    {
      var casinoGames = new List<CasinoGame>();
      try
      {
        using( var sqlCmd = new SqlCommand( "SelectAllCasinoGames", sqlConnection )
        {
          CommandType = CommandType.StoredProcedure
        } )
        {
          using( var sqlReader = await sqlCmd.ExecuteReaderAsync() )
          {
            while( sqlReader.Read() )
            {
              var casinoGame = new CasinoGame();
              ReadCasinoGame( casinoGame, sqlReader );
              casinoGames.Add( casinoGame );
            }
          }
        }
        return casinoGames;
      }
      catch( Exception ex )
      {
        _log.LogError( ex, "SelectAllCasinos failed" );
        throw;
      }
    }

    public async Task<int> InsertCasinoGame( SqlConnection sqlConnection, CasinoGame casinoGame )
    {
      try
      {
        //TODO move these out to file
        using( var sqlCmd = new SqlCommand( "InsertCasinoGame", sqlConnection )
        {
          CommandType = CommandType.StoredProcedure
        } )
        {
          sqlCmd.Parameters.Add( new SqlParameter( "Name ", casinoGame.Name ) );
          sqlCmd.Parameters.Add( new SqlParameter( "HasSubType ", casinoGame.HasSubType ) );

          sqlCmd.Parameters.Add( "@id", SqlDbType.Int ).Direction = ParameterDirection.Output;

          sqlCmd.ExecuteNonQuery();
          string id = sqlCmd.Parameters["@id"].Value.ToString();
          return int.Parse( id );
        }
      }
      catch( SqlException e )
      {
        //TODO update name
        _log.LogError( e, "InsertCasinoGame failed" );
        throw;
      }
    }

    //Helper Functions
    public void ReadCasinoGame( CasinoGame casinoGame, SqlDataReader sqlDataReader )
    {
      casinoGame.Id = DatabaseUtilities.GetInt32( sqlDataReader, "Id", 0 ) ?? 0;
      casinoGame.Name = DatabaseUtilities.GetString( sqlDataReader, "Name", 1 );
      casinoGame.HasSubType = DatabaseUtilities.GetBoolean( sqlDataReader, "HasSubType", 2 ) ?? false;
    }


  }
}

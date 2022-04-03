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
  public class SqlCasinoManager : Manager, ICasinoManager
  {
    private string _connectionString;
    public SqlCasinoManager( IConfiguration configuration ) : base( configuration )
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

    public async Task<string> GetCasinos()
    {
      return "Hello from SqlCasinoManager";
    }

    public async Task<List<Casino>> GetAllCasinos()
    {
      return await DatabaseUtilities.ExecuteAsync<List<Casino>>( _connectionString,
                async ( c ) => await SelectAllCasinos( c ) );
    }

    public async Task UpsertCasino( Casino casino )
    {
      if( casino.Id <= 0 )
      {
        //what to do with int??? move this logic up, have an insert call???
        await DatabaseUtilities.ExecuteAsync<int>( _connectionString,
                async ( c ) => await InsertCasino( c, casino ) );
      }
    }

    //Database

    public async Task<List<Casino>> SelectAllCasinos( SqlConnection sqlConnection )
    {
      var casinos = new List<Casino>();
      try
      {
        using( var sqlCmd = new SqlCommand( "SelectAllCasinos", sqlConnection )
        {
          CommandType = CommandType.StoredProcedure
        } )
        {
          using( var sqlReader = await sqlCmd.ExecuteReaderAsync() )
          {
            while( sqlReader.Read() )
            {
              var casino = new Casino();
              ReadCasino( casino, sqlReader );
              casinos.Add( casino );
            }
          }
        }
        return casinos;
      }
      catch( Exception ex )
      {
        _log.LogError( ex, "SelectAllCasinos failed" );
        throw;
      }
    }

    public async Task<int> InsertCasino( SqlConnection sqlConnection, Casino casino )
    {
      try
      {
        //TODO move these out to file
        using( var sqlCmd = new SqlCommand( "InsertCasino", sqlConnection )
        {
          CommandType = CommandType.StoredProcedure
        } )
        {
          sqlCmd.Parameters.Add( new SqlParameter( "Name ", casino.Name ) );
          sqlCmd.Parameters.Add( new SqlParameter( "CountryCode ", casino.CountryCode ) );

          sqlCmd.Parameters.Add( "@id", SqlDbType.Int ).Direction = ParameterDirection.Output;

          sqlCmd.ExecuteNonQuery();
          string id = sqlCmd.Parameters["@id"].Value.ToString();
          return int.Parse( id );
        }
      }
      catch( SqlException e )
      {
        //TODO update name
        _log.LogError( e, "InsertCasino failed" );
        throw;
      }
    }
    //Helper Functions
    public void ReadCasino( Casino casino, SqlDataReader sqlDataReader )
    {
      casino.Id = DatabaseUtilities.GetInt32( sqlDataReader, "Id", 0 ) ?? 0;
      casino.Name = DatabaseUtilities.GetString( sqlDataReader, "Name", 1 );
      casino.CountryCode = DatabaseUtilities.GetString( sqlDataReader, "CountryCode", 2 );

    }


  }
}

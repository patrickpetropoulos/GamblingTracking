using Newtonsoft.Json.Linq;
using Server.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Domain.Entities
{
  public class GamblingSession
  {
    public Guid? Id { get; set; }
    public Casino Casino { get; set; }
    public CasinoGame CasinoGame { get; set; }
    //sub type id
    public decimal StartingAmount { get; set; }
    public decimal EndingAmount { get; set; }

    public void FromJson( JObject json )
    {
      Id = JSONUtilities.GetNullableGuid( json, "id" );
      
      var casino = new Casino();
      casino.FromJson( (JObject)json["casino"] );
      Casino = casino;
      var casinoGame = new CasinoGame();
      casinoGame.FromJson( (JObject)json["casinoGame"] );
      CasinoGame = casinoGame;

      StartingAmount = JSONUtilities.GetDecimal( json, "startingAmount" );
      EndingAmount = JSONUtilities.GetDecimal( json, "endingAmount" );
    }

    public JObject ToJson()
    {
      var json = new JObject();

      JSONUtilities.Set( json, "id", Id );
      if( Casino != null )
      {
        JSONUtilities.Set( json, "casino", Casino.ToJson() );
      }
      else
      {
        JSONUtilities.Set( json, "casino", new JObject() );
      }
      if( CasinoGame != null )
      {
        JSONUtilities.Set( json, "casinoGame", CasinoGame.ToJson() );
      }
      else
      {
        JSONUtilities.Set( json, "casinoGame", new JObject() );
      }
      JSONUtilities.Set( json, "startingAmount", StartingAmount );
      JSONUtilities.Set( json, "endingAmount", EndingAmount );

      return json;
    }


  }
}

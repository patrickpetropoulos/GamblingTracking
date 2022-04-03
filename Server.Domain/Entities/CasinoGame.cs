using Newtonsoft.Json.Linq;
using Server.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Domain.Entities
{
  public class CasinoGame
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasSubType { get; set; }
    //public ISubGame (videopoker, keno, slots)

    public void FromJson( JObject json )
    {
      Id = JSONUtilities.GetInt( json, "id" );
      Name = JSONUtilities.GetString( json, "name" );
      HasSubType = JSONUtilities.GetBool( json, "hasSubType" );
    }

    public JObject ToJson()
    {
      var json = new JObject();

      JSONUtilities.Set( json, "id", Id );
      JSONUtilities.Set( json, "name", Name );
      JSONUtilities.Set( json, "hasSubType", HasSubType );

      return json;
    }

  }
}

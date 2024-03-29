﻿using Newtonsoft.Json.Linq;
using Server.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Domain.Entities
{
  public class Casino
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }



    public void FromJson( JObject json )
    {
      Id = JSONUtilities.GetInt( json, "id" );
      Name = JSONUtilities.GetString( json, "name" );
      CountryCode = JSONUtilities.GetString( json, "countryCode" );
    }

    public JObject ToJson()
    {
      var json = new JObject();

      JSONUtilities.Set( json, "id", Id );
      JSONUtilities.Set( json, "name", Name );
      JSONUtilities.Set( json, "countryCode", CountryCode );

      return json;
    }



  }
}

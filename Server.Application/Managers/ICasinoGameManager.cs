using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Managers
{
  public interface ICasinoGameManager
  {
    Task<List<CasinoGame>> GetAllCasinoGames();
    //country, state, etc
    Task UpsertCasinoGame( CasinoGame casino );
  }
}

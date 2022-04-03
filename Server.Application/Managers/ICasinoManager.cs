using Server.Domain.Entities;
using Server.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Managers
{
  public interface ICasinoManager : IManager
  {
    Task<List<Casino>> GetAllCasinos();
    //country, state, etc
    Task UpsertCasino(Casino casino);

  }
}

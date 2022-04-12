using System.Collections.Generic;
using System.Linq;
using Server.Domain.Entities;

namespace Server.Application
{
    public class GamblingSessionCalculator
    {
        private readonly List<GamblingSession> _gamblingSessions;

        public GamblingSessionCalculator(List<GamblingSession> gamblingSessions)
        {
            _gamblingSessions = gamblingSessions;
        }
        
        
        public decimal GetTotalWinLoss()
        {
            return _gamblingSessions.Sum(gamblingSession => (gamblingSession.EndingAmount - gamblingSession.StartingAmount));
        } 
    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Server.Domain.Entities;

namespace Server.Domain.Testing
{
    public static class ExampleData
    {
        public static List<Casino> GetListOfCasinos()
        {
            var casinoList = new List<Casino>();
            casinoList.Add( new Casino()
            {
                Name = "Borgata",
                CountryCode = "US"
            } );
            casinoList.Add( new Casino()
            {
                Name = "Bellagio",
                CountryCode = "US"
            } );
            casinoList.Add( new Casino()
            {
                Name = "Montreal Casino",
                CountryCode = "CA"
            } );

            return casinoList;
        }

        public static List<CasinoGame> GetListOfCasinoGames()
        {
            var casinoGames = new List<CasinoGame>();
            casinoGames.Add( new CasinoGame()
            {
                Name = "Blackjack",
                HasSubType = false
            } );
            casinoGames.Add( new CasinoGame()
            {
                Name = "Craps",
                HasSubType = false
            } );
            casinoGames.Add( new CasinoGame()
            {
                Name = "Video Keno",
                HasSubType = true
            } );

            return casinoGames;
        }

        public static List<GamblingSession> GetListOfGamblingSessions()
        {
            var casinos = GetListOfCasinos();
            var casinoGames = GetListOfCasinoGames();

            var gamblingSessions = new List<GamblingSession>();

            var rand = new Random();
            
            foreach (var casino in casinos)
            {
                foreach (var casinoGame in casinoGames)
                {
                    var session = new GamblingSession()
                    {
                        Casino = casino,
                        CasinoGame = casinoGame,
                        StartingAmount = rand.Next(2000),
                        EndingAmount = rand.Next(0, 10001)
                    };
                }
            }

            return gamblingSessions;
        }
    }
}
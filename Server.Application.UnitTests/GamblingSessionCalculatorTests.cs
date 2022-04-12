using System;
using System.Collections.Generic;
using NUnit.Framework;
using Server.Domain.Entities;

namespace Server.Application.UnitTests
{
    [TestFixture]
    public class GamblingSessionCalculatorTests
    {
        [Test]
        public void NoSession_ZeroTotal()
        {
            var gamblingSessionCalculator =
                new GamblingSessionCalculator(new List<GamblingSession>());
            
            Assert.AreEqual(0, gamblingSessionCalculator.GetTotalWinLoss());
        }
        
        [Test]
        public void SingleSession_SameStartingAndEndingBalance_ZeroTotal()
        {
            var gamblingSession = new GamblingSession()
            {
                StartingAmount = 0,
                EndingAmount = 0
            };

            var gamblingSessionCalculator =
                new GamblingSessionCalculator(new List<GamblingSession>() {gamblingSession});
            
            Assert.AreEqual(0, gamblingSessionCalculator.GetTotalWinLoss());
        }
        
        [Test]
        public void SingleSession_DifferentStartingAndEndingBalance_PositiveBalance()
        {
            var gamblingSession = new GamblingSession()
            {
                StartingAmount = 150,
                EndingAmount = 300
            };

            var gamblingSessionCalculator =
                new GamblingSessionCalculator(new List<GamblingSession>() {gamblingSession});
            
            Assert.AreEqual(150, gamblingSessionCalculator.GetTotalWinLoss());
        }
        
        [Test]
        public void SingleSession_DifferentStartingAndEndingBalance_NegativeBalance()
        {
            var gamblingSession = new GamblingSession()
            {
                StartingAmount = 200,
                EndingAmount = 100
            };

            var gamblingSessionCalculator =
                new GamblingSessionCalculator(new List<GamblingSession>() {gamblingSession});
            
            Assert.AreEqual(-100, gamblingSessionCalculator.GetTotalWinLoss());
        }
        
        [Test]
        public void SingleSession_DifferentStartingAndEndingBalance_DecimalBalance()
        {
            var gamblingSession = new GamblingSession()
            {
                StartingAmount = 134.57M,
                EndingAmount = 130.11M
            };

            var gamblingSessionCalculator =
                new GamblingSessionCalculator(new List<GamblingSession>() {gamblingSession});
            
            Assert.AreEqual(-4.46M, gamblingSessionCalculator.GetTotalWinLoss());
        }
    }
}
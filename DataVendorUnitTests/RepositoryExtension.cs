using NUnit.Framework;
using DataVendor.Repositories;
using DataVendor.Models;
using System;

namespace DataVendorUnitTests
{
    [TestFixture]
    public class RepositoryExtension
    {
        [Test]
        public void ParserFromCSV_WithValidHungarianInput_ShouldReturnValidMarketDataEntity()
        {
            string[] input =
            {
                "1+1 DRILLISCH AG O.N.",
                "",
                "60,05",
                "2018. 06. 08. 17:35:00",
                "86531",
                "60,2",
                "File #3"
            };

            var expectedResult = new MarketDataEntity()
            {
                Name = "1+1 DRILLISCH AG O.N.",
                Isin = "",
                ClosingPrice = 60.05m,
                DateTime = Convert.ToDateTime("2018. 06. 08. 17:35:00"),
                Volumen = 86531,
                PreviousDayClosingPrice = 60.2m,
                StockExchange = "File #3"
            };

            var result = input.ParserFromCSV();

            Assert.AreNotEqual(expectedResult, result);
        }

        [Test]
        public void ParserFromCSV_WithValidUSInput_ShouldReturnValidMarketDataEntity()
        {
            string[] inputHU =
            {
                "1+1 DRILLISCH AG O.N.",
                "",
                "60.05",
                "6/8/2018 5:35:00 PM",
                "86531",
                "60.2",
                "File #3"
            };

            var expectedResult = new MarketDataEntity()
            {
                Name = "1+1 DRILLISCH AG O.N.",
                Isin = "",
                ClosingPrice = 60.05m,
                DateTime = Convert.ToDateTime("2018. 06. 08. 17:35:00"),
                Volumen = 86531,
                PreviousDayClosingPrice = 60.2m,
                StockExchange = "File #3"
            };

            var result = inputHU.ParserFromCSV();

            Assert.AreEqual(expectedResult, result);
        }
    }
}

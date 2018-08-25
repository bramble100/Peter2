using Screener.Models;
using StockDataModels;
using System;
using System.Collections.Generic;

namespace Screener.Helpers
{
    public static class Converter
    {
        public static RegistryItem ToRegistryItem(IDictionary<string, string> input, out string Isin)
        {
            var result = new RegistryItem
            {
                FinancialReport = new FinancialReport()
            };

            try
            {
                Isin = string.IsNullOrEmpty(input["ISIN"])
                    ? throw new ArgumentException("ISIN cannot be null or empty.")
                    : input["ISIN"];
                result.Name = string.IsNullOrEmpty(input["Name"])
                    ? throw new ArgumentException("Name cannot be null or empty.")
                    : input["Name"];

                if (!string.IsNullOrEmpty(input["Stock Exchange Link"]))
                {
                    result.StockExchangeLink = new Uri(input["Stock Exchange Link"]);
                }
                if (!string.IsNullOrEmpty(input["Own Investor Link"]))
                {
                    result.OwnInvestorLink = new Uri(input["Own Investor Link"]);
                }

                if (!string.IsNullOrEmpty(input["EPS"]))
                {
                    result.FinancialReport.EarningPerShare = Convert.ToDecimal(input["EPS"]);
                }
                if (!string.IsNullOrEmpty(input["Months in Report"]))
                {
                    result.FinancialReport.MonthsInReport = Convert.ToInt32(input["Months in Report"]);
                }
                if (!string.IsNullOrEmpty(input["Next Report Date"]))
                {
                    result.FinancialReport.NextReportDate = Convert.ToDateTime(input["Next Report Date"]);
                }

                var position = input["Position"].ToLower();
                if (string.Equals(position, "long"))
                {
                    result.Position = Position.Long;
                }
                else if (string.Equals(position, "short"))
                {
                    result.Position = Position.Short;
                }
                else
                {
                    result.Position = Position.NoPosition;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Provided strings cannot be converted into registry item.", ex);
            }

            return result;
        }

        public static MarketDataEntity ToMarketDataEntity(IDictionary<string, string> input)
        {
            var result = new MarketDataEntity();
            // 			Previous Day Closing Price	

            try
            {
                result.Name = string.IsNullOrEmpty(input["Name"])
                    ? throw new ArgumentException("Name cannot be null or empty.")
                    : input["Name"];
                result.Isin = string.IsNullOrEmpty(input["ISIN"])
                    ? throw new ArgumentException("ISIN cannot be null or empty.")
                    : input["ISIN"];
                result.ClosingPrice = string.IsNullOrEmpty(input["Closing Price"])
                    ? throw new ArgumentException("Closing Price cannot be null or empty.")
                    : Convert.ToDecimal(input["Closing Price"]);
                result.DateTime = string.IsNullOrEmpty(input["DateTime"])
                    ? throw new ArgumentException("DateTime cannot be null or empty.")
                    : Convert.ToDateTime(input["DateTime"]);
                result.Volumen = string.IsNullOrEmpty(input["Volumen"])
                    ? throw new ArgumentException("Volumen cannot be null or empty.")
                    : Convert.ToInt32(input["Volumen"]);
                result.StockExchange = string.IsNullOrEmpty(input["Stock Exchange"])
                    ? throw new ArgumentException("Stock Exchange cannot be null or empty.")
                    : input["Stock Exchange"];
            }
            catch (Exception ex)
            {
                throw new Exception("Provided strings cannot be converted into market data entity.", ex);
            }

            return result;
        }
    }
}

using Screener.Models;
using StockDataModels;
using System;
using System.Collections.Generic;

namespace Screener.Helpers
{
    internal static class Extensions
    {
        /// <summary>
        /// Returns a formatted string for writing into CSV file.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        internal static string FormatterForCSV(this MarketDataEntity entity, string separator)
        {
            return String.Join(separator,
                entity.Name.WrapWithQuotes(),
                entity.Isin,
                entity.ClosingPrice,
                entity.DateTime,
                entity.Volumen,
                entity.PreviousDayClosingPrice,
                entity.StockExchange);
        }

        /// <summary>
        /// Returns a formatted string for writing into CSV file.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        internal static string FormatterForCSV(this ShareAnalysis analysis, string isin, string name, string separator)
        {
            return String.Join(separator,
                name.WrapWithQuotes(),
                isin,
                analysis.Recommendation,
                analysis.Taz);
        }

        internal static MarketDataEntity ParserFromCSV(this IEnumerable<string> strings)
        {
            var queue = new Queue<string>(strings);

            return new MarketDataEntity
            {
                Name = queue.Dequeue(),
                Isin = queue.Dequeue(),
                ClosingPrice = Convert.ToDecimal(queue.Dequeue()),
                DateTime = Convert.ToDateTime(queue.Dequeue()),
                Volumen = Convert.ToInt32(queue.Dequeue()),
                PreviousDayClosingPrice = Convert.ToDecimal(queue.Dequeue()),
                StockExchange = queue.Dequeue()
            };
        }

        internal static RegistryItem RegistryParserFromCSV(this IEnumerable<string> strings, out string Isin)
        {
            var inputList = new List<string>(strings);
            try
            {
                if (string.IsNullOrEmpty(inputList[0]) || string.IsNullOrEmpty(inputList[1]))
                {
                    throw new ArgumentNullException();
                }

                var item = new RegistryItem();
                
                item.FinancialReport = new FinancialReport();

                item.Name = inputList[0];

                Isin = inputList[1];

                if (!string.IsNullOrEmpty(inputList[2]))
                {
                    item.FinancialReport.EarningPerShare = Convert.ToDecimal(inputList[2]);
                }

                if (!string.IsNullOrEmpty(inputList[3]))
                {
                    item.FinancialReport.MonthsInReport = Convert.ToInt32(inputList[3]);
                }

                if (!string.IsNullOrEmpty(inputList[4]))
                {
                    item.FinancialReport.NextReportDate = Convert.ToDateTime(inputList[4]);
                }

                if (!string.IsNullOrEmpty(inputList[5]))
                {
                    item.StockExchangeLink = new Uri(inputList[5]);
                }

                if (!string.IsNullOrEmpty(inputList[6]))
                {
                    item.OwnInvestorLink = new Uri(inputList[6]);
                }                    

                if (String.Equals(inputList[7].ToLower(), "long"))
                {
                    item.Position = Position.Long;
                }
                else if (String.Equals(inputList[7].ToLower(), "short"))
                {
                    item.Position = Position.Short;
                }
                else
                {
                    item.Position = Position.NoPosition;
                }

                return item;
            }
            catch (Exception ex)
            {
                throw new FormatException((new List<string>(strings))[0], ex);
            }
        }

        internal static string WrapWithQuotes(this object obj) => $"\"{obj.ToString()}\"";

        public static KeyValuePair<string, string> IsinParserFromCSV(this IList<string> strings) 
            => new KeyValuePair<string, string>(strings[0], strings[1]);
    }
}

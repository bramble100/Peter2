using DataVendor.Models;
using System;
using System.Collections.Generic;

namespace DataVendor.Services.FileProviders
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

        internal static string WrapWithQuotes(this object obj) => $"\"{obj.ToString()}\"";

        public static KeyValuePair<string, string> IsinParserFromCSV(this IEnumerable<string> strings)
        {
            var queue = new Queue<string>(strings);
            return new KeyValuePair<string, string>(queue.Dequeue(), queue.Dequeue());
        }
    }
}

using StockDataModels;
using System;

namespace Screener.Models
{
    /// <summary>
    /// One entry in the share registry. Contains no ISIN intentionally.
    /// </summary>
    public class RegistryItem
    {
        public FinancialReport FinancialReport { get; set; }
        /// <summary>
        /// Name of the stock.
        /// </summary>
        public string Name { get; set; }
        public Uri OwnInvestorLink { get; set; }
        public Position Position { get; set; }
        public Uri StockExchangeLink { get; set; }
    }
}

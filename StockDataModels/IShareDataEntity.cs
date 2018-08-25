using System;

namespace StockDataModels
{
    public interface IShareDataEntity
    {
        /// <summary>
        /// Latest recorded price (in euro). If recorded after closing it is called closing price.
        /// </summary>
        decimal ClosingPrice { get; set; }
        //FinancialReport FinancialReport { get; set; }
        /// <summary>
        /// ISIN of the stock (ISIN = International Securities Identification Number).
        /// </summary>
        string Isin { get; set; }
        /// <summary>
        /// Name of the stock.
        /// </summary>
        string Name { get; set; }
        Uri OwnInvestorLink { get; set; }
        Position Position { get; set; }
        Uri StockExchangeLink { get; set; }
        /// <summary>
        /// Number of stocks traded during the day.
        /// </summary>
        int Volumen { get; set; }
    }
}

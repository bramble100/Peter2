using System;

namespace StockDataModels
{
    public class FinancialReport
    {
        /// <summary>
        /// Earning Per Share (according to annual or interim financial report).
        /// </summary>
        public decimal EarningPerShare { get; set; }
        /// <summary>
        /// Number of months covered by report (may be 3, 6,9 or 12).
        /// </summary>
        public int MonthsInReport { get; set; }
        /// <summary>
        /// Date on which the next report is expected to be published.
        /// </summary>
        public DateTime NextReportDate { get; set; }
    }
}
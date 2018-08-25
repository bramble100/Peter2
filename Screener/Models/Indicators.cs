namespace StockDataModels
{
    public class Indicators
    {
        /// <summary>
        /// Fast (etc the last 7 days) moving average (in EUR).
        /// </summary>
        public decimal FastMovingAverage { get; set; }
        /// <summary>
        /// Slow (etc the last 21 days) moving average (in EUR).
        /// </summary>
        public decimal SlowMovingAverage { get; set; }
        /// <summary>
        /// Price/Earning (P/E) ratio.
        /// </summary>
        public decimal PriceToEarningRatio { get; set; }
    }
}
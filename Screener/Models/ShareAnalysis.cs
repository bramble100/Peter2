using StockDataModels;
using System;
using System.Collections.Generic;

namespace Screener.Models
{
    /// <summary>
    /// Holds all the information regarding a share.
    /// </summary>
    public class ShareAnalysis : IEquatable<ShareAnalysis>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ShareAnalysis()
        {
            Errors = new HashSet<string>();
            Indicators = new Indicators();
            Recommendation = RecommendationInfo.Hold;
        }
        /// <summary>
        /// True if stock is buyable.
        /// </summary>
        public bool Buyable { get; set; }
        /// <summary>
        /// Error messages (filled only in case of errors).
        /// </summary>
        public HashSet<string> Errors { get; set; }
        /// <summary>
        /// Latest recorded price (in euro). If recorded after closing it is called closing price.
        /// </summary>
        public decimal LatestClosingPrice { get; set; }
        /// <summary>
        /// The calculated indicators.
        /// </summary>
        public Indicators Indicators { get; set; }
        public RecommendationInfo Recommendation { get; set; }
        /// <summary>
        /// The closing price relative to Traders Action Zone.
        /// </summary>
        public TazInfo Taz { get; set; }
        public TrendInfo Trend { get; set; }
        /// <summary>
        /// True if financial report is outdated.
        /// </summary>
        public bool Updateable { get; set; }
        public int LatestVolumen { get; set; }

        public override bool Equals(object obj) => Equals(obj as ShareAnalysis);

        public bool Equals(ShareAnalysis other)
        {
            return other != null &&
                   Buyable == other.Buyable &&
                   LatestClosingPrice == other.LatestClosingPrice &&
                   EqualityComparer<Indicators>.Default.Equals(Indicators, other.Indicators) &&
                   Recommendation == other.Recommendation &&
                   Taz == other.Taz &&
                   Updateable == other.Updateable &&
                   LatestVolumen == other.LatestVolumen;
        }

        public override int GetHashCode()
        {
            var hashCode = -1235458270;
            hashCode = hashCode * -1521134295 + Buyable.GetHashCode();
            hashCode = hashCode * -1521134295 + LatestClosingPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Indicators>.Default.GetHashCode(Indicators);
            hashCode = hashCode * -1521134295 + Recommendation.GetHashCode();
            hashCode = hashCode * -1521134295 + Taz.GetHashCode();
            hashCode = hashCode * -1521134295 + Updateable.GetHashCode();
            hashCode = hashCode * -1521134295 + LatestVolumen.GetHashCode();
            return hashCode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Screener.Models;
using Screener.Repositories;

namespace Screener.Services
{
    public class ScreenerService
    {
        private readonly MarketDataRepository _marketDataRepository = new MarketDataRepository();
        private readonly RegistryRepository _registryRepository = new RegistryRepository();
        private readonly AnalysesRepository _analysesRepository = new AnalysesRepository();

        private const int FAST_MOVING_AVERAGE_SUBSETSIZE = 7;
        private const int SLOW_MOVING_AVERAGE_SUBSETSIZE = 21;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScreenerService()
        {
            _registryRepository.LoadFromCsv();
            _marketDataRepository.LoadFromCsv();
        }

        public void AnalyseShares()
        {
            foreach (var isin in _registryRepository.Keys)
            {
                _analysesRepository.Add(isin, AnalyseShare(isin, _registryRepository[isin]));
            }

            foreach (var item in _registryRepository)
            {
                _analysesRepository.AddRegistryData(item.Key, item.Value);
            }

            _marketDataRepository
                .Select(md => md.Isin)
                .Distinct()
                .Where(s => !_registryRepository.ContainsKey(s))
                .ToList()
                .ForEach(s => _analysesRepository.Add(s, new ShareAnalysis
                    {
                        Errors = new HashSet<string> { "Share not found in registry." }
                    }
                ));
        }

        private ShareAnalysis AnalyseShare(string isin, RegistryItem share)
        {
            var analysis = new ShareAnalysis();
            var marketData = _marketDataRepository
                .Where(md => string.Equals(md.Isin, isin))
                .OrderByDescending(md => md.DateTime);
            if (!marketData.Any())
            {
                analysis.Errors.Add("No market data found.");
                return analysis;
            }
            else if (marketData.Count() < Math.Max(FAST_MOVING_AVERAGE_SUBSETSIZE, SLOW_MOVING_AVERAGE_SUBSETSIZE))
            {
                analysis.Errors.Add("Not enough market data found.");
                return analysis;
            }

            analysis.Indicators.FastMovingAverage = marketData
                .Take(FAST_MOVING_AVERAGE_SUBSETSIZE)
                .Average(md => md.ClosingPrice);
            analysis.Indicators.SlowMovingAverage = marketData
                .Take(SLOW_MOVING_AVERAGE_SUBSETSIZE)
                .Average(md => md.ClosingPrice);

            if (analysis.Indicators.FastMovingAverage > analysis.Indicators.SlowMovingAverage)
            {
                analysis.Trend = TrendInfo.UpTrend;
            }
            else if (analysis.Indicators.FastMovingAverage < analysis.Indicators.SlowMovingAverage)
            {
                analysis.Trend = TrendInfo.DownTrend;
            }
            else
            {
                analysis.Trend = TrendInfo.Uncertain;
            }

            var closingPrice = marketData.First().ClosingPrice;

            if (closingPrice <= 0)
            {
                analysis.Errors.Add("Closing price must be greater than zero.");
                return analysis;
            }

            if (closingPrice > Math.Max(analysis.Indicators.FastMovingAverage, analysis.Indicators.SlowMovingAverage))
            {
                analysis.Taz = TazInfo.Above;
            }
            else if (closingPrice < Math.Min(analysis.Indicators.FastMovingAverage, analysis.Indicators.SlowMovingAverage))
            {
                analysis.Taz = TazInfo.Below;
            }
            else
            {
                analysis.Taz = TazInfo.In;
            }

            if (share.FinancialReport.MonthsInReport == 0)
            {
                analysis.Errors.Add("Number of months in financial report must not be zero (make sure it is not left blank).");
                return analysis;
            }
            if (share.FinancialReport.EarningPerShare == 0)
            {
                analysis.Errors.Add("Earning Per Share in financial report must not be zero (make sure it is not left blank).");
                return analysis;
            }

            var yearlyEPS = share.FinancialReport.EarningPerShare / share.FinancialReport.MonthsInReport * 12;
            analysis.Indicators.PriceToEarningRatio = closingPrice / yearlyEPS;

            return analysis;
        }

        public void SaveAnalysis()
        {
            _analysesRepository.SaveIntoCsv();
        }

        public override string ToString()
        {
            return String.Join("\n",
                _marketDataRepository.Any() ? $"{_marketDataRepository.Count} line(s) of market data read." : "No market data read.",
                _registryRepository.Any() ? $"{_registryRepository.Count} line(s) of registry data read." : "No registry data read.");
        }
    }
}

using Screener.Helpers;
using Screener.Models;
using System.Collections;
using System.Collections.Generic;

namespace Screener.Repositories
{
    public class MarketDataRepository : IReadOnlyList<MarketDataEntity>
    {
        private List<MarketDataEntity> _marketDataEntities = new List<MarketDataEntity>();

        public MarketDataEntity this[int index] => ((IReadOnlyList<MarketDataEntity>)_marketDataEntities)[index];

        public int Count => ((IReadOnlyList<MarketDataEntity>)_marketDataEntities).Count;

        public IEnumerator<MarketDataEntity> GetEnumerator()
        {
            return ((IReadOnlyList<MarketDataEntity>)_marketDataEntities).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<MarketDataEntity>)_marketDataEntities).GetEnumerator();
        }

        public void LoadFromCsv()
        {
            var provider = new Helpers.FileProviderCsv()
            {
                WorkingSubFolder = "MarketDataWithIsins",
                Separator = "\t"
            };
            foreach (var line in provider.Load("2018-08-24-17-39.csv"))
            {
                _marketDataEntities.Add(Converter.ToMarketDataEntity(line));
            }
        }
    }
}

using Screener.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Screener.Repositories
{
    public    class AnalysesRepository : IDictionary<string, ShareAnalysis>
    {
        private readonly Dictionary<string, ShareAnalysis> _analyses = new Dictionary<string, ShareAnalysis>();
        private readonly Dictionary<string, RegistryItem> _registryData = new Dictionary<string, RegistryItem>();
        string _workingDirectory = @"C:\Users\arnol\Desktop\StockExchange";

        public ShareAnalysis this[string key] { get => _analyses[key]; set => _analyses[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, ShareAnalysis>)_analyses).Keys;

        public ICollection<ShareAnalysis> Values => ((IDictionary<string, ShareAnalysis>)_analyses).Values;

        public int Count => _analyses.Count;

        public bool IsReadOnly => ((IDictionary<string, ShareAnalysis>)_analyses).IsReadOnly;

        public void Add(string key, ShareAnalysis value)
        {
            _analyses.Add(key, value);
        }

        public void Add(KeyValuePair<string, ShareAnalysis> item)
        {
            ((IDictionary<string, ShareAnalysis>)_analyses).Add(item);
        }

        public void AddRegistryData(string isin, RegistryItem registryItem)
        {
            _registryData.Add(isin, registryItem);
        }

        public void Clear()
        {
            _analyses.Clear();
        }

        public bool Contains(KeyValuePair<string, ShareAnalysis> item)
        {
            return ((IDictionary<string, ShareAnalysis>)_analyses).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _analyses.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, ShareAnalysis>[] array, int arrayIndex)
        {
            ((IDictionary<string, ShareAnalysis>)_analyses).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, ShareAnalysis>> GetEnumerator()
        {
            return ((IDictionary<string, ShareAnalysis>)_analyses).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _analyses.Remove(key);
        }

        public bool Remove(KeyValuePair<string, ShareAnalysis> item)
        {
            return ((IDictionary<string, ShareAnalysis>)_analyses).Remove(item);
        }

        public void SaveIntoCsv()
        {
            var strings = new List<string> { "Name,ISIN,TAZ,P/E ratio,Errors" };
            foreach (var isin in _analyses.Keys)
            {
                var analysis = _analyses[isin];
                strings.Add(string.Join(",",
                    $"\"{(_registryData.ContainsKey(isin) ? _registryData[isin].Name : "Not found in registry")}\"",
                    isin,
                    analysis.Taz,
                    Math.Round(analysis.Indicators.PriceToEarningRatio, 2),
                    string.Join(" - ", analysis.Errors)));
            }

            File.WriteAllLines(
                Path.Combine(_workingDirectory, "Screener", "analyses.csv"),
                strings,
                Encoding.UTF8);
        }

        public bool TryGetValue(string key, out ShareAnalysis value)
        {
            return _analyses.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, ShareAnalysis>)_analyses).GetEnumerator();
        }
    }
}

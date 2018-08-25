using System;
using System.Collections;
using System.Collections.Generic;

namespace DataVendor.Models
{
    internal class Isins : IEnumerable<string>
    {
        /// <summary>
        /// Key: Company name; Value: ISIN
        /// </summary>
        private Dictionary<string, string> _isins = new Dictionary<string, string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Isins()
        {
        }

        public IEnumerator<string> GetEnumerator() => _isins.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal int Count => _isins.Count;

        internal string this[string name] => _isins[name];

        internal bool ContainsKey(string name) => _isins.ContainsKey(name);

        internal void Add(KeyValuePair<string, string> kvp)
        {
            var Name = String.IsNullOrEmpty(kvp.Key) 
                ? throw new ArgumentException($"ISIN cannot be added (name is null or empty).", nameof(kvp.Key)) 
                : kvp.Key;
             var Isin = String.IsNullOrEmpty(kvp.Value)
                ? throw new ArgumentException($"ISIN cannot be added (ISIN null or empty).", nameof(kvp.Value))
                : kvp.Value;

            if (!_isins.ContainsKey(Name))
            {
                _isins.Add(Name, Isin);
            }
        }
    }
}

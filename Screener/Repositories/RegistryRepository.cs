using Screener.Helpers;
using Screener.Models;
using System.Collections;
using System.Collections.Generic;

namespace Screener.Repositories
{
    public class RegistryRepository : IReadOnlyDictionary<string, RegistryItem>
    {
        private Dictionary<string, RegistryItem> _registry = new Dictionary<string, RegistryItem>();

        public RegistryItem this[string key] => ((IReadOnlyDictionary<string, RegistryItem>)_registry)[key];

        public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, RegistryItem>)_registry).Keys;

        public IEnumerable<RegistryItem> Values => ((IReadOnlyDictionary<string, RegistryItem>)_registry).Values;

        public int Count => ((IReadOnlyDictionary<string, RegistryItem>)_registry).Count;

        public bool ContainsKey(string key)
        {
            return ((IReadOnlyDictionary<string, RegistryItem>)_registry).ContainsKey(key);
        }

        public void LoadFromCsv()
        {
            var provider = new Helpers.FileProviderCsv()
            {
                WorkingSubFolder = "Screener"
            };
            foreach (var line in provider.Load("registry.csv"))
            {
                var item = Converter.ToRegistryItem(line, out string Isin);
                _registry.Add(Isin, item);
            }
        }

        public IEnumerator<KeyValuePair<string, RegistryItem>> GetEnumerator()
        {
            return ((IReadOnlyDictionary<string, RegistryItem>)_registry).GetEnumerator();
        }

        public bool TryGetValue(string key, out RegistryItem value)
        {
            return ((IReadOnlyDictionary<string, RegistryItem>)_registry).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyDictionary<string, RegistryItem>)_registry).GetEnumerator();
        }
    }
}

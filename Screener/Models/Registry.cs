using System;
using System.Collections.Generic;
using System.Text;

namespace Screener.Models
{
    public class Registry
    {
        private Dictionary<string, RegistryItem> _registry = new Dictionary<string, RegistryItem>();

        public RegistryItem this[string index]
        {
            get
            {
                if (String.IsNullOrEmpty(index))
                {
                    throw new ArgumentException("ISIN is null or empty.");
                }
                return _registry.ContainsKey(index) ? _registry[index] : throw new KeyNotFoundException($"ISIN ({index}) not found.");
            }
            set
            {
                _registry[index] = value;
            }
        }
    }
}
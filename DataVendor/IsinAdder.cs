using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataVendor.Models;
using DataVendor.Services.FileProviders;
using Microsoft.VisualBasic.FileIO;

namespace DataVendor
{
    public class IsinAdder
    {
        private MarketDataEntities _marketData;
        private Isins _isins = new Isins();
        private string _workingDirectory = @"C:\Users\arnol\Desktop\StockExchange";
        private HashSet<string> _missingCompanies = new HashSet<string>();

        private const string separator = ",";

        /// <summary>
        /// Returns true if assigning ISIN to any of the companies was unsuccesful.
        /// </summary>
        public bool MissingCompanyFound => _missingCompanies.Any();

        /// <summary>
        /// Returns a list of the companies to which ISIN could not been assigned.
        /// </summary>
        public IEnumerable<string> MissingCompanies => _missingCompanies.OrderBy(c => c);

        /// <summary>
        /// Reads in the market data info from a CSV file.
        /// </summary>
        public void ReadInLatestMarketData()
        {
            var fileName = "2018-08-24-17-39.csv";
            var filePath = Path.Combine(_workingDirectory, "MarketDataWithIsins", fileName);
            _marketData = (new FileProviderCsv(_workingDirectory)).Load(filePath);
            Console.WriteLine($"{_marketData.Count} new market data record(s) read from file.");
        }

        /// <summary>
        /// Reads in the ISINs of the companies from a CSV file.
        /// </summary>
        public void ReadInISIN()
        {
            var fileName = "isins.csv";
            var filePath = Path.Combine(_workingDirectory, "AddIsins", fileName);
            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(separator);

                RemoveHeader(parser);

                while (!parser.EndOfData)
                {
                    _isins.Add(parser.ReadFields().IsinParserFromCSV());
                }
            }

            Console.WriteLine($"{_isins.Count} new ISIN(s) read from file.");
        }

        /// <summary>
        /// Extends the market data info list with the appropriate ISINs. If to any of the entries no ISIN info available, registers the companies into a separate list of missing companies.
        /// </summary>
        public void AssignIsins() => _marketData.ForEach(AssignIsin);

        /// <summary>
        /// Saves the loaded market data into CSV file.
        /// </summary>
        public void SaveCsv()
        {
            var workingDirectory = Path.Combine(_workingDirectory, "MarketDataWithIsins");

            (new FileProviderCsv(workingDirectory)).Save(_marketData);
        }

        private static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();

        /// <summary>
        /// Extend the market data info with the ISIN. If no ISIN info available, registers the company name into the list of missing companies.
        /// </summary>
        private void AssignIsin(MarketDataEntity mde)
        {
            if (_isins.ContainsKey(mde.Name))
            {
                mde.Isin = _isins[mde.Name];
            }
            else
            {
                _missingCompanies.Add(mde.Name);
            }
        }
    }
}

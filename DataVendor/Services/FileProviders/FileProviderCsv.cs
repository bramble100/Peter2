using DataVendor.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataVendor.Services.FileProviders
{
    /// <summary>
    /// Provides access to the local file system.
    /// </summary>
    internal class FileProviderCsv : FileProvider
    {
        private const string separator = ",";
        private new const string _fileNameExtension = "csv";

        private readonly string[] header = new string[]
        {
            "Name",
            "ISIN",
            "Closing Price",
            "DateTime",
            "Volumen",
            "Previous Day Closing Price",
            "Stock Exchange"
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        internal FileProviderCsv(): base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workingDirectory"></param>
        internal FileProviderCsv(string workingDirectory) : base(workingDirectory)
        {
        }

        /// <summary>
        /// Saves the entity collection into CSV file.
        /// </summary>
        /// <param name="entities"></param>
        internal void Save(IEnumerable<MarketDataEntity> entities)
        {
            List<string> strings = AddHeader();

            strings.AddRange(entities.Select(e => e.FormatterForCSV(separator)));

            File.WriteAllLines(
                Path.Combine(WorkingDirectory, FileNameCreator(entities.Max(e => e.DateTime))),
                strings,
                Encoding.UTF8);
        }

        private List<string> AddHeader() => new List<string> { String.Join(separator, header) };

        /// <summary>
        /// Loads all the CSV files and returns their content in one collection.
        /// </summary>
        /// <returns></returns>
        internal MarketDataEntities LoadMany() => new MarketDataEntities(_filesToProcess.SelectMany(Load));

        /// <summary>
        /// Fills in an internal list with all the CSV file names to load the content from.
        /// </summary>
        /// <returns></returns>
        internal FileProviderCsv GetFilePaths()
        {
            base.FillFilesToProcessList(_fileNameExtension);
            return this;
        }

        /// <summary>
        /// Loads one CSV file and returns its content.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal MarketDataEntities Load(string filePath)
        {
            using (var parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.SetDelimiters(separator);

                RemoveHeader(parser);

                var entities = new MarketDataEntities();
                while (!parser.EndOfData)
                {
                    entities.Add(parser.ReadFields().ParserFromCSV());
                }
                return entities;
            }
        }

        private static void RemoveHeader(TextFieldParser parser) => parser.ReadLine();

        private static string FileNameCreator(DateTime dateTime)
        {
            return $"{dateTime.ToString(_dateFormat)}.{_fileNameExtension}";
        }

        protected string FileNameCreator(DateTime dateTime, string stockExchangeName)
        {
            return base.FileNameCreator(dateTime, stockExchangeName, _fileNameExtension);
        }
    }
}

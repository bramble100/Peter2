using DataVendor.Models;
using DataVendor.Models.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataVendor.Services.FileProviders
{
    internal class FileProviderHtml : FileProvider
    {
        private const string fileNameHtml = "exp.htm";
        private new const string _fileNameExtension = "htm";

        internal FileProviderHtml() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workingDirectory"></param>
        internal FileProviderHtml(string workingDirectory) : base(workingDirectory)
        {
        }

        /// <summary>
        /// Loads the html content of the given files into memory.
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        internal IEnumerable<string> LoadMany() => _filesToProcess.Select(file => Load(file));

        internal FileProviderHtml GetFilePaths()
        {
            base.FillFilesToProcessList(_fileNameExtension);
            return this;
        }

        internal string Load(string fileName = fileNameHtml)
        {
            return File.ReadAllText(
                Path.Combine(WorkingDirectory, fileName),
                Encoding.UTF8);
        }

        internal void SaveMany(StockExchangesHtmls stockExchangesHtmls, DateTime dateTime)
        {
            foreach (string key in stockExchangesHtmls.Keys)
            {
                Save(stockExchangesHtmls[key], key, dateTime);
            }
        }

        internal void Save(
            string html,
            string stockName,
            DateTime dateTime)
        {
            File.WriteAllText(
                Path.Combine(WorkingDirectory, FileNameCreator(dateTime, stockName)),
                html);
        }

        protected string FileNameCreator(DateTime dateTime, string stockExchangeName)
        {
            return base.FileNameCreator(dateTime, stockExchangeName, _fileNameExtension);
        }
    }
}

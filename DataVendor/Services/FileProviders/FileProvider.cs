using System;
using System.Collections.Generic;
using System.IO;

namespace DataVendor.Services.FileProviders
{
    internal abstract class FileProvider
    {
        protected const string _dateFormat = "yyyy-MM-dd-HH-mm";
        protected readonly string _fileNameExtension;

        protected string _workingDirectory = @"C:\Users\arnol\Desktop\StockExchange";
        protected HashSet<string> _filesToProcess;

        /// <summary>
        /// The directory in which the provider works.
        /// </summary>
        internal string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new Exception($"Invalid directory specified ({value})");
                }
                _workingDirectory = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        internal FileProvider()
        {
            _filesToProcess = new HashSet<string>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workingDirectory"></param>
        internal FileProvider(string workingDirectory) : this()
        {
            WorkingDirectory = workingDirectory;
        }

        /// <summary>
        /// Fills in an internal list with all the file names of the relevant type to load the content from.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        internal FileProvider GetFilePaths(string extension)
        {
            FillFilesToProcessList(extension);
            return this;
        }

        protected void FillFilesToProcessList(string extension)
        {
            _filesToProcess = new HashSet<string>(Directory.GetFiles(_workingDirectory, $"*.{extension}"));
        }

        protected string FileNameCreator(DateTime dateTime, string stockExchangeName, string fileNameExtension)
        {
            return $"{dateTime.ToString(_dateFormat)} {stockExchangeName}.{fileNameExtension}";
        }
    }
}

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Screener.Helpers
{
    public class FileProviderCsv
    {
        private const string _fileNameExtension = "csv";
        private const string _workingDirectory = @"C:\Users\arnol\Desktop\StockExchange";

        public string WorkingSubFolder { get; set; }
        public string Separator { get; set; } = ",";

        public IEnumerable<Dictionary<string,string>> Load(string fileName)
        {
            using (var parser = new TextFieldParser(Path.Combine(_workingDirectory, WorkingSubFolder, fileName), Encoding.UTF8))
            {
                parser.SetDelimiters(Separator);

                var headers = new List<string>(parser.ReadFields());
                if (headers.Count > (new HashSet<string>(headers).Count))
                {
                    throw new ArgumentException("CSV headers must be unique.");
                }

                var content = new List<Dictionary<string, string>>();

                while (!parser.EndOfData)
                {
                    var lineDict = new Dictionary<string, string>();
                    parser
                        .ReadFields().Select((item, index) => new KeyValuePair<string, string>(headers[index], item))
                        .ToList()
                        .ForEach(p => lineDict.Add(p.Key, p.Value));
                    content.Add(lineDict);
                }

                return content;
            }
        }
    }
}

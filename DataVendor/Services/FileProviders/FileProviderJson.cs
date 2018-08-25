using DataVendor.Models;
using DataVendor.Models.Implementations;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace DataVendor.Services.FileProviders
{
    /// <summary>
    /// Provides access to the local file system.
    /// </summary>
    internal class FileProviderJson : FileProvider
    {
        // https://www.newtonsoft.com/json/help/html/Introduction.htm

        private const string fileNameJSON = "exp.json";

        internal FileProviderJson() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workingDirectory"></param>
        internal FileProviderJson(string workingDirectory) : base(workingDirectory)
        {
        }

        internal StockExchangesHtmls Load(
            string fileName = fileNameJSON)
        {
            return JsonConvert.DeserializeObject<StockExchangesHtmls>(
                File.ReadAllText(
                    Path.Combine(WorkingDirectory, fileName),
                    Encoding.UTF8));
        }

        internal void Save(
            StockExchangesHtmls stockExchangesHtml,
            string fileName = fileNameJSON)
        {
            File.WriteAllText(
                Path.Combine(WorkingDirectory, fileName),
                JsonConvert.SerializeObject(stockExchangesHtml),
                Encoding.UTF8);
        }
    }
}

using DataVendor.Models;
using DataVendor.Services.FileProviders;
using DataVendor.Services.Html;

namespace DataVendor
{
    public class DataVendor
    {
        private readonly MarketDataEntities _entities;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataVendor()
        {
            _entities = new MarketDataEntities();
        }

        /// <summary>
        /// Downloads all the market info from the data vendor web pages.
        /// </summary>
        public void DownloadFromWeb()
        {
            _entities.AddRange(HtmlDownloader
                .DownloadAll()
                .GetMarketDataEntities());
            _entities.Sort();
        }

        /// <summary>
        /// Loads all the market info from the available CSV files.
        /// </summary>
        public void LoadFromCsvs()
        {
            _entities.AddRange(
                (new FileProviderCsv(@"C:\Users\arnol\Desktop\StockExchange\RawDownload"))
                    .GetFilePaths()
                    .LoadMany());
            _entities.Sort();
        }

        /// <summary>
        /// Saves all the load-in market info into a CSV file.
        /// </summary>
        public void SaveCsv()
        {
            _entities.Sort();
            (new FileProviderCsv(@"C:\Users\arnol\Desktop\StockExchange\RawDownload"))
                .Save(_entities);
        }
    }
}
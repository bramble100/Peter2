using DataVendor;
using Screener;
using Screener.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLParser
{
    public class Controller
    {
        private static readonly Dictionary<string, Action> commandDispatcher = new Dictionary<string, Action>
        {
            { "--addisins", AddIsins },
            { "--screen", Screen },
            { "--web2csv", WebToCsv},
            //{ "--csv2csv", CsvToCsv },
            //{ "--htmls2csv", HtmlToCsv },
            //{ "--web2htmls", WebToHtmls }
        };

        public const string HelpString =
            "usage: peter [--web2csv]\n" +
            "             Downloads and parses actual data from the web, loads the\n" +
            "             content of the available csv files and saves all the data\n" +
            "             into one csv file.\n\n" +

            "             [--web2htmls]\n" +
            "             Downloads the data vendor pages from the web and saves the html\n" +
            "             files into the working directory (NOT IMPLEMENTED).\n\n" +

            "             [--htmls2csv]\n" +
            "             Loads and parses the local html files and saves all the data into\n" +
            "             one csv file.\n\n" +

            "             [--csv2csv]\n" +
            "             Loads the content of the available csv files and saves all the data\n" +
            "             into one csv file.";

        public void Execute(string[] inputCommands)
        {
            CheckCommands(inputCommands);
            commandDispatcher[inputCommands[0]].Invoke();
        }

        private void CheckCommands(string[] inputCommands)
        {
            if (!inputCommands.Any())
            {
                throw new Exception("No command given.");
            }
            else if (inputCommands.Count() > 1)
            {
                throw new Exception("Too many command given.");
            }
            else if (!commandDispatcher.Keys.Contains(inputCommands[0]))
            {
                throw new Exception("Invalid command(s).");
            }
        }

        private static void AddIsins()
        {
            var id = new IsinAdder();
            id.ReadInLatestMarketData();
            id.ReadInISIN();
            id.AssignIsins();
            if (id.MissingCompanyFound)
            {
                Console.WriteLine("Company(ies) with unknown ISIN:");
                id.MissingCompanies.ToList().ForEach(Console.WriteLine);
            }

            id.SaveCsv();
        }

        private static void Screen()
        {
            var sc = new ScreenerService();
            try
            {
                sc.AnalyseShares();
                sc.SaveAnalysis();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine(sc.ToString());
            Console.ReadKey();
        }

        private static void WebToCsv()
        {
            var dv = new DataVendor.DataVendor();
            dv.LoadFromCsvs();
            dv.DownloadFromWeb();
            dv.SaveCsv();
        }

        //    var entities = htmls.GetMarketDataEntities();
        //    var provider = new FileProviderHtml();
        //    provider.SaveMany(htmls, entities.MaxDateTime());
        //    // TODO: finish
        //}

        //private static void HtmlToCsv()
        //{
        //    var entities = CommandCenterHelper.LoadFromHtmls();
        //    CommandCenterHelper.ReportLoadedData(entities);
        //    entities.Sort();
        //    CommandCenterHelper.SaveCsv(entities);
        //    Console.WriteLine(entities.Count());
        //}

        //private static void CsvToCsv()
        //{
        //    var entities = CommandCenterHelper.LoadFromCsvs();
        //    CommandCenterHelper.ReportLoadedData(entities);
        //    entities.Sort();
        //    CommandCenterHelper.SaveCsv(entities);
        //    Console.WriteLine(entities.Count());
        //}
    }
}

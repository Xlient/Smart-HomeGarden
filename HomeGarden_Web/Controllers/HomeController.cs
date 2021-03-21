using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeGarden_Web.Models;
using HomeGarden_Web.Services;
using Microsoft.AspNetCore.Hosting;



namespace HomeGarden_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILogger<CsvParseService> _csvlogger;
        private readonly ILogger<BigQueryExportService> _bqlogger;
        private readonly IWebHostEnvironment _environment;
        private CsvParseService _csvParse;
        public HomeController(ILogger<HomeController> logger, ILogger<CsvParseService> csvlogger,
            ILogger<BigQueryExportService> bqLogger, IWebHostEnvironment environment)

        {
            _logger = logger;
            _csvlogger = csvlogger;
            _bqlogger = bqLogger;
            _environment = environment;
            _csvParse = new CsvParseService(_environment, _csvlogger);
        }



        public IActionResult Index()
        {
            _ = new BigQueryExportService(_bqlogger);

            var cloudStorage = new CloudStorageDownloadService(_environment);

           
            cloudStorage.StartDownload();
            
             _csvParse.ParseAllCsv();

            List<IChartData> models = new List<IChartData>
            {
                _csvParse.SoilMoistureChart,
                _csvParse.LightResistanceChart,
                _csvParse.TemperatureChart,
                _csvParse.HumidityChart
            };

            return View(models);
        }

       




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
 

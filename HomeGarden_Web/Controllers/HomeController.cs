using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        ILogger<BigQueryExportService> _bqlogger;
        private IWebHostEnvironment _environment;
        public HomeController(ILogger<HomeController> logger, ILogger<CsvParseService> csvlogger ,
            ILogger<BigQueryExportService> bqLogger, IWebHostEnvironment environment)

        {
            _logger = logger;
            _csvlogger = csvlogger;
            _bqlogger = bqLogger;
            _environment = environment;
        }


        public IActionResult Index( )
        {
            var exportService = new BigQueryExportService(_bqlogger);
            var cloudStorage = new CloudStorageDownloadService(_environment);

            // exportService.StartQuery();
           // cloudStorage.StartDownload();
            var csvParse = new CsvParseService(_environment, _csvlogger);
            csvParse.ParseAllCsv();

            ViewBag.dataSource = csvParse.SoilMoistureChart;
            
           // create a class to rep senor data
            

            return View();
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
 

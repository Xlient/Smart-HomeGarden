using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using HomeGarden_Web.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using static HomeGarden_Web.SensorData;

namespace HomeGarden_Web.Services
{
    public class CsvParseService
    {
       
        private readonly ILogger<CsvParseService> _logger;
        public IWebHostEnvironment WebHostEnvironment { get; }
        public List<SoilMoistureChartData> SoilMoistureChart { get; private set; }

        public List<TemperatureChartData> TemperatureChart { get; private set; }
         
        public List<LightResistanceChartData> LightResistanceChart { get; private set; }
        public List<HumidityChartData> HumidityChart{ get; private set; }

        public CsvParseService(IWebHostEnvironment environment, ILogger<CsvParseService> logger)
        {
            WebHostEnvironment = environment;
            _logger = logger;
        }


        public void ParseAllCsv()
        {
           
            foreach (SENSOR_DATA_TYPE type in Enum.GetValues(typeof(SENSOR_DATA_TYPE)))
            {
                switch (type)
                {
                    case SENSOR_DATA_TYPE.SOIL_MOISTURE:
                        Parse("sm-data-000000000001.csv", SENSOR_DATA_TYPE.SOIL_MOISTURE);
                        
                      
                        break;
                    case SENSOR_DATA_TYPE.LIGHT:
                        Parse("lr-data-000000000001.csv", SENSOR_DATA_TYPE.SOIL_MOISTURE);
                        
                        break;
                    case SENSOR_DATA_TYPE.TEMPERATURE:
                        Parse("tmp-data-000000000000.csv", SENSOR_DATA_TYPE.SOIL_MOISTURE);
                       
                        
                        break;
                    case SENSOR_DATA_TYPE.HUMIDITY:
                        Parse("hm-data-000000000001.csv", SENSOR_DATA_TYPE.SOIL_MOISTURE);
                      
                        
                        break;
                    default:
                        break;
                }

               
               
            }



        }

        /// <summary>
        ///  Parses through the csv file and coverts the data into the appropiate model
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <returns> Model to the  sensor data</returns>
        private void Parse(string filename, SENSOR_DATA_TYPE type)
        {
            List<SoilMoistureChartData> soilMoisturesList = new List<SoilMoistureChartData>();
            List<TemperatureChartData> temperaturesList = new List<TemperatureChartData>();
            List<LightResistanceChartData> lightResistancesList = new List<LightResistanceChartData>();
            List<HumidityChartData> humidityList = new List<HumidityChartData>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(Path.Combine(WebHostEnvironment.WebRootPath, "data", filename)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        string dateString = fields[1] + fields[2];
                        DateTime date;

                        if (type == SENSOR_DATA_TYPE.SOIL_MOISTURE)
                        {
                            dateString = fields[2] + fields[3];
                            DateTime.TryParse(dateString, out date);
                            soilMoisturesList.Add(new SoilMoistureChartData(int.Parse(fields[0]), int.Parse(fields[1]), date));

                        }

                        else if (type == SENSOR_DATA_TYPE.LIGHT)
                        {
                            DateTime.TryParse(dateString, out date);
                            lightResistancesList.Add(new LightResistanceChartData(int.Parse(fields[0]), date));
                        }
                        else if (type == SENSOR_DATA_TYPE.TEMPERATURE)
                        {
                            DateTime.TryParse(dateString, out date);
                            temperaturesList.Add(new TemperatureChartData(int.Parse(fields[0]), date));
                        }

                        else
                        {
                            DateTime.TryParse(dateString, out date);
                            humidityList.Add(new HumidityChartData(int.Parse(fields[0]), date));
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(@" \n --------------\n
                                        Somthing went wrong in {0}. Message: {1}  Stack trace: {3}   Inner Exception {4}", ex.Source, ex.Message, ex.StackTrace, ex.StackTrace);
            }

            SoilMoistureChart = soilMoisturesList;
            LightResistanceChart = lightResistancesList;
            TemperatureChart = temperaturesList;
            HumidityChart = humidityList;



        }





    }
}

    

    






    


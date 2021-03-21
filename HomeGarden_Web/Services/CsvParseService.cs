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
        public SoilMoistureChartData SoilMoistureChart { get; private set; }

        public TemperatureChartData TemperatureChart { get; private set; }
         
        public LightResistanceChartData LightResistanceChart { get; private set; }
        public HumidityChartData HumidityChart{ get; private set; }

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
                        Parse("sm-data-000000000000.csv", SENSOR_DATA_TYPE.SOIL_MOISTURE);
                        
                      
                        break;
                    case SENSOR_DATA_TYPE.LIGHT:
                        Parse("lr-data-000000000000.csv", SENSOR_DATA_TYPE.LIGHT);
                        
                        break;
                    case SENSOR_DATA_TYPE.TEMPERATURE:
                        Parse("tmp-data-000000000000.csv", SENSOR_DATA_TYPE.TEMPERATURE);
                       
                        
                        break;
                    case SENSOR_DATA_TYPE.HUMIDITY:
                        Parse("hm-data-000000000000.csv", SENSOR_DATA_TYPE.HUMIDITY);
                      
                        
                        break;
                    default:
                        break;
                }

            }

        }

        /// <summary>
        /// Takes a file name and Sensor data type and parses through the the csv file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        ///
        private void Parse(string filename, SENSOR_DATA_TYPE type)
        {
            List<int> soilMoisturesList = new List<int>();
            List<int> temperaturesList = new List<int>();
            List<int> lightResistancesList = new List<int>();
            List<int> humidityList = new List<int>();
            List<DateTime> dateTimes = new List<DateTime>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(Path.Combine(WebHostEnvironment.WebRootPath, "data", filename)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        string dateString = fields[1]+"T" + fields[2] + "Z";
                        DateTime date;

                        if (type == SENSOR_DATA_TYPE.SOIL_MOISTURE)
                        {
                            dateString = fields[2] + "T" + fields[3] + "Z";
                            date = DateTime.Parse(dateString).ToUniversalTime();
                            soilMoisturesList.Add(int.Parse(fields[0]));
                            dateTimes.Add(date);

                        }

                        else if (type == SENSOR_DATA_TYPE.LIGHT)
                        {
                            date = DateTime.Parse(dateString).ToUniversalTime();
                            lightResistancesList.Add(int.Parse(fields[0]));
                            dateTimes.Add(date);
                        }
                        else if (type == SENSOR_DATA_TYPE.TEMPERATURE)
                        {
                            date = DateTime.Parse(dateString).ToUniversalTime();
                            temperaturesList.Add(int.Parse(fields[0]));
                            dateTimes.Add(date);
                        }

                        else
                        {
                            date = DateTime.Parse(dateString).ToUniversalTime();
                            humidityList.Add(int.Parse(fields[0]));
                            dateTimes.Add(date);
                        }

                    }


                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Somthing went wrong in {ex.Source}. Message: {ex.Message}");
                _logger.LogError($"Inner Exception : {ex.InnerException}");
                _logger.LogError(ex.StackTrace);
            }

            switch (type)
            {
                case SENSOR_DATA_TYPE.SOIL_MOISTURE:
                    UpdateModels(soilMoisturesList, dateTimes, type);
                    break;
                case SENSOR_DATA_TYPE.LIGHT:
                    UpdateModels(lightResistancesList, dateTimes, type);
                    break;
                case SENSOR_DATA_TYPE.TEMPERATURE:
                    UpdateModels(temperaturesList, dateTimes, type);
                    break;
                case SENSOR_DATA_TYPE.HUMIDITY:
                    UpdateModels(humidityList, dateTimes, type);
                    break;
                default:
                    break;
            }


        }

      
        private void UpdateModels(List<int> dataList, List<DateTime> dates, SENSOR_DATA_TYPE sensorType) 
        {
            switch (sensorType)
            {
                case SENSOR_DATA_TYPE.SOIL_MOISTURE:
                    SoilMoistureChart = new SoilMoistureChartData {
                        Data_Axis_Y = dataList,
                          Data_Axis_X = dates,
                    };
                        
                    break;
                case SENSOR_DATA_TYPE.LIGHT:
                    LightResistanceChart = new LightResistanceChartData
                    {
                        Data_Axis_Y = dataList,
                        Data_Axis_X = dates,
                    };
                    break;
                case SENSOR_DATA_TYPE.TEMPERATURE:
                    TemperatureChart = new TemperatureChartData
                    {
                        Data_Axis_Y = dataList,
                        Data_Axis_X = dates,
                    };
                    break;
                case SENSOR_DATA_TYPE.HUMIDITY:
                    HumidityChart = new HumidityChartData
                    {
                        Data_Axis_Y = dataList,
                        Data_Axis_X = dates,
                    };
                    break;
                default:
                    break;
            }
        }


    }
}

    

    






    


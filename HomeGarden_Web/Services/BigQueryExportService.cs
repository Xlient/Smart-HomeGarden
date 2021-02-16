using System;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Microsoft.Extensions.Logging;
using static HomeGarden_Web.SensorData;

namespace HomeGarden_Web.Services
{
    public class BigQueryExportService
    {
       
        private readonly string _projectId = "smartgarden-iot";
        private readonly ILogger<BigQueryExportService> _logger;
        public BigQueryExportService(ILogger<BigQueryExportService> logger)
        {
            _logger = logger;
        }

        public void StartQuery() 
        {
            foreach (SENSOR_DATA_TYPE type in Enum.GetValues(typeof(SENSOR_DATA_TYPE))) 
            {
                CreateExportQuery(type);
            }
        }
        private async void CreateExportQuery(SENSOR_DATA_TYPE type) 
        {
            try
            {
                var credential = GoogleCredential.FromFile("C:\\Users\\lakal\\OneDrive\\Documents\\auth-keys.json");
                BigQueryClient client = BigQueryClient.Create(_projectId, credential);
                string table_name = "", destination_uri = " ";

                switch (type)
                {
                    case SENSOR_DATA_TYPE.SOIL_MOISTURE:
                        table_name = "`smartgarden-iot.sm_gardenplant_data.plant_Soil_Moisture`";
                        destination_uri = "gs://bq-query-export-data/sm-data-*.csv";
                        break;
                    case SENSOR_DATA_TYPE.LIGHT:
                        table_name = "`smartgarden-iot.sm_gardenplant_data.plant_light_resistance`";
                        destination_uri = "gs://bq-query-export-data/lr-data-*.csv";
                        break;
                    case SENSOR_DATA_TYPE.TEMPERATURE:
                        table_name = "`smartgarden-iot.sm_gardenplant_data.plant_temperature`";
                        destination_uri = "gs://bq-query-export-data/tmp-data-*.csv";
                        break;
                    case SENSOR_DATA_TYPE.HUMIDITY:
                        table_name = "`smartgarden-iot.sm_gardenplant_data.plant_humidity`";
                        destination_uri = "gs://bq-query-export-data/hm-data-*.csv";
                        break;
                    default:
                        break;
                }

                string sql = string.Format(@"EXPORT DATA OPTIONS(uri='{0}', format='CSV', overwrite=true)
                                             AS SELECT * FROM {1} WHERE TIME_DIFF(time_received, CURRENT_TIME('UTC-5'),HOUR) = 3", destination_uri, table_name);

                BigQueryParameter[] parameters = null;

                BigQueryResults _ = await client.ExecuteQueryAsync(sql, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(@" \n --------------\n
                                    Somthing went wrong in {0}. \n Message: {1} \n \n Stack trace: {3}  \n Inner Exception {4}", ex.Source, ex.Message, ex.StackTrace, ex.StackTrace);
                
            }
     

            
            
        }
    }

}

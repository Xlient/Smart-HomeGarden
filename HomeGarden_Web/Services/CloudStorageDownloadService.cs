using System;
using Google.Cloud.Storage.V1;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using static HomeGarden_Web.SensorData;

namespace HomeGarden_Web.Services
{
    public class CloudStorageDownloadService
    {
        private readonly string _storageBucket = "bq-query-export-data";
        public IWebHostEnvironment WebHostEnvironment { get; }
        public CloudStorageDownloadService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public async void StartDownload() 
        {
            foreach (SENSOR_DATA_TYPE type in Enum.GetValues(typeof(SENSOR_DATA_TYPE))) 
            {
                switch (type)
                {
                    case SENSOR_DATA_TYPE.SOIL_MOISTURE: 
                        DownloadTableData(_storageBucket, "sm-data-000000000001.csv");
                        break;
                    case SENSOR_DATA_TYPE.LIGHT:
                       DownloadTableData(_storageBucket, "lr-data-000000000001.csv");
                        break;
                    case SENSOR_DATA_TYPE.TEMPERATURE:
                        DownloadTableData(_storageBucket, "tmp-data-000000000000.csv");
                        break;
                    case SENSOR_DATA_TYPE.HUMIDITY:
                        DownloadTableData(_storageBucket, "hm-data-000000000000.csv");
                        break;
                    default:
                        break;
                }
            }
        }

           private async void DownloadTableData( string bucketName, string objectName)
        { 
            var credential = GoogleCredential.FromFile("C:\\Users\\lakal\\OneDrive\\Documents\\auth-keys.json");
            //var credential = GoogleCredential.GetApplicationDefault();
            StorageClient storageClient = await StorageClient.CreateAsync(credential);
            using (var fstream = new FileStream(Path.Combine(WebHostEnvironment.WebRootPath, "data", objectName), FileMode.Create))
            {
                 storageClient.DownloadObject(bucketName, objectName, fstream);
            }
        }
    }
}

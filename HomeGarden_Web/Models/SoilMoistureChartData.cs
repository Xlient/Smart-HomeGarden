using System;


namespace HomeGarden_Web.Models
{
    public class SoilMoistureChartData
    {
        public int SoilMoisture1;
        public int SoilMoisture2;
        public DateTime datetime;
        public SoilMoistureChartData(int soilmoisture1, int soilmoisture2, DateTime dateTime)
        {
            SoilMoisture1 = soilmoisture1;
            SoilMoisture2 = soilmoisture2;
            datetime = dateTime;
        }


    }
}

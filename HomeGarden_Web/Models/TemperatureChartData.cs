using System;


namespace HomeGarden_Web.Models
{
    public class TemperatureChartData
    {
        public int Temperature;
        public DateTime datetime;
        public TemperatureChartData(int temperature, DateTime dateTime)
        {
            Temperature = temperature;
            datetime = dateTime;
        }
    }
}

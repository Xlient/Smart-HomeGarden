using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeGarden_Web.Models
{
    public class HumidityChartData
    {
        public int Humidity;
        public DateTime datetime;
        public HumidityChartData(int humidity, DateTime dateTime)
        {
            Humidity = humidity;
            datetime = dateTime;
        }
        
    }
}
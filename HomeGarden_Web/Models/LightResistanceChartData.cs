using System;

namespace HomeGarden_Web.Models
{
    public class LightResistanceChartData
    {
        public int LightResistance;
        public DateTime datetime;


        public LightResistanceChartData(int light, DateTime dateTime)
        {
            LightResistance = light;
            datetime = dateTime;
        }

        
    }
}

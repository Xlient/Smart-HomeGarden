using System;
using System.Collections.Generic;

namespace HomeGarden_Web.Models
{
    public class SoilMoistureChartData : IChartData
    {
        public SoilMoistureChartData()
        {

        }
        public List<int> Data_Axis_Y { get; set; }
        public List<DateTime> Data_Axis_X{ get; set; }
    }
}

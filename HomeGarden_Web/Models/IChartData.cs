using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeGarden_Web.Models
{
    public interface IChartData
    {
        public List<int> Data_Axis_Y { get; set; }
        public List<DateTime> Data_Axis_X { get; set; }
    }
}

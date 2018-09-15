using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricAnalyzerSoftwareQuality.Class.Table
{
    public class MyTableInfo_OfAllParameters
    {
        public MyTableInfo_OfAllParameters() { }
        public MyTableInfo_OfAllParameters(int number, string name)
        {
            this.Number = number;
            this.Name = name;
        }
        public int Number { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricAnalyzerSoftwareQuality.Class
{
    public class MyTableInfo_OfAllMetrics
    {
        public MyTableInfo_OfAllMetrics() { }
        public MyTableInfo_OfAllMetrics(int number, string name, int max, int min, int count)
        {
            this.Number = number;
            this.Name = name;
            this.MAX = max;
            this.MIN = min;
            this.Count = count;
        }
        public int Number { get; set; }
        public string Name { get; set; }
        public int MAX { get; set; }
        public int MIN { get; set; }
        public int Count { get; set; }
    }
}

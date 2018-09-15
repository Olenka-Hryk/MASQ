using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality
{
public abstract class MetricsQualitySoftware
    {
        private double forSaveAllResults_OfEachFunction = 0;      //змінна для збереження сум всіх функцій для отримання кінцевого результату метрики
        public String Name;                      // назва метрики
        protected String Description;            // опис метрики
        public int PermissibleMINvalue;          // min допустиме значення метрики
        public int PermisibleMAXvalue;           // max допустиме значення метрики
        public int ParametersCount;              // кількість параметрів

        public List<Parameter> Metric = new List<Parameter>();

        public struct Parameter
        {
            public int Number { get; set; }          //номер параметра
            public String Name { get; set; }         //назва параметра
            public double Value { get; set; }        //значення параметра     

            public double GetValue()
            {
                return Value; 
            }

            public void SetValue(double value)
            {
                Value = value;
            }

            public String GetName()
            {
                return Name;
            }
        }

        public MetricsQualitySoftware()                                  // екземпляр конструктора без аргументів
        {
            this.Name = "";
            this.Description = "";
            this.PermissibleMINvalue = 0;
            this.PermisibleMAXvalue = 0;
            this.ParametersCount = 0;
        }

        public MetricsQualitySoftware(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount)                // екземпляр конструктора з аргументами
        {
            this.Name = name;
            this.Description = description;
            this.PermisibleMAXvalue = permissibleMIN;
            this.PermissibleMINvalue = permissibleMAXvalue;
            this.ParametersCount = parametersCount;
        }

        public void ChangeValue_OfParameter(int item, double value)            //функція встановлення значення параметра[item = порядковий номер метрики; value = значення параметра]
        {
            var parameter = Metric[item];
            parameter.SetValue(value);
            Metric[item] = parameter;
        }

        public double FindFinalResult_forSomeMetric(int countOfAllFunction, int numberOfFunction, double resultOfSomeFunction)          //функція знаходження кінцевого результату метрики, шляхом сумування всіх (countOfAllFunction) функцій за їхніми результатами (resultOfSomeFunction)
        {
            if(numberOfFunction == countOfAllFunction)
            {
                forSaveAllResults_OfEachFunction += resultOfSomeFunction;
                var result = forSaveAllResults_OfEachFunction;
                forSaveAllResults_OfEachFunction = 0;
                return result; 
            }
            else
            {
                forSaveAllResults_OfEachFunction += resultOfSomeFunction;
                return forSaveAllResults_OfEachFunction;
            }           
        }


        public double FindLOC_forSomeFunction(double LOC_better, double LOC_worse, double LOC_probable)
        {
            double result = (LOC_better + LOC_worse + 4 * LOC_probable) / 6;
            int intRes = (int)(result * 1000);
            result = (double)intRes / 1000.0;
            return result;
        }

        public string GetNameOfParameter (int item)
        {
            return Metric[item].GetName();
        }

        public abstract void SetInformation_OfMetric();                                   //встановлення інформації метрики
        public abstract void SetAllParametersWithDefaultValue_OfMetric();                 //встановлення параметрів метрики з дефолтним (нульовим - 0) значеннями
        public abstract String ShowDescription_OfMetric();                                //вивід опису метрики
        public abstract String ShowInformationOfParameters_and_SolutionOfMetric();        //вивід інформації про параметри метрики та спосіб знаходження метрики
        public abstract double FindMetric();                                              //функція знаходження метрики
        public abstract void ClearAllParameters_OfMetric();                               //функція очищення всіх параметрів
    }
}

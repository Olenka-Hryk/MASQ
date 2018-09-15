using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Predicted_Value
{
    public class SCCmetric : MetricsQualitySoftware
    {
        public int CountOfFunction;
        private MainWindow _mainWindow;

        public struct TableForFunction_SCC
        {
            public int Number { get; set; }                    // номер функції
            public double LOC { get; set; }                    // кількість рядків функції
            public double Cost { get; set; }                   // вартість рядка функції     
            public double DevelopmentCost { get; set; }        // вартість розроблення функції
            public double GetLOC()
            {
                return LOC;
            }
            public void SetLOC(double loc)
            {
                LOC = loc;
            }
            public double GetCost()
            {
                return Cost;
            }
            public void SetCost(double cost)
            {
                Cost = cost;
            }
            public double GetDevelopmentCost()
            {
                return DevelopmentCost;
            }
            public void SetDevelopmentCost(double developmentcost)
            {
                DevelopmentCost = developmentcost;
            }
        }

        public SCCmetric() : base()
        {
            Console.WriteLine();
        }
        public SCCmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, int countOfFunction, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this.CountOfFunction = countOfFunction;
            this._mainWindow = win;
        }
        public SCCmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика очікуваної вартості розроблення ПЗ";
            this.Description = "Очікувана вартість процесу розроблення і-ої функції: \nB(i) = LOC(оч i)*ВАРТІСТЬ(рядка і),   і=1,m";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 20000;
            this.ParametersCount = 2;
            this.CountOfFunction = 1;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Очікувана кількість рядків вихідного коду функції", Value = 4670 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Очікувана вартість розроблення рядка функції*", Value = 2 };
            Metric.Add(secondParameter);

            this._mainWindow.InputTableInfoParameters_SCC_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": \n" + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "\nLOCоч - " + Metric[0].GetName() + "\n" +
                   "ВАРТІСТЬ рядка - " + Metric[1].GetName() + "\n" +
                   "ВАРТІСТЬ = LOCоч * ВАРТІСТЬ рядка (гривень)  SCCє[" + PermissibleMINvalue + ";0..." + PermisibleMAXvalue + "] \n";
        }

        public String ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(int numberFunction)    //вивід інформації по кожній функції 
        {
            return "--->>>> ФУНКЦІЯ " + numberFunction +" :\n" +
                   "LOCоч(" +numberFunction+")= " + Metric[0].GetValue() + "\n" +
                   "ВАРТІСТЬ рядка (" + numberFunction + ")= " + Metric[1].GetValue() + "\n" +
                   "ВАРТІСТЬ = LOCоч("+numberFunction+") * ВАРТІСТЬ рядка(" + numberFunction +")= "+Metric[0].GetValue()+" * "+Metric[1].GetValue()+" = " + FindMetric()+"(гривень)\n\n";
        }

        public override double FindMetric()
        {
            try
            {
                double result = (double)Metric[0].GetValue() * (double)Metric[1].GetValue();
                if ((result <= PermisibleMAXvalue) && (result >= PermissibleMINvalue))
                {
                    int intRes = (int)(result * 1000);
                    result = (double)intRes / 1000.0;
                    return result;
                }
                else
                {
                    MessageBox.Show("Некоректні вхідні дані, адже значення метрики (SCC = " + result + " ) виходить за допустимі межі [-1,0..20000]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + e, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public override void ClearAllParameters_OfMetric()
        {
            MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
            this._mainWindow.OutputTableInfoParameters_SCC_dg.Items.Refresh();
        }
    }
}

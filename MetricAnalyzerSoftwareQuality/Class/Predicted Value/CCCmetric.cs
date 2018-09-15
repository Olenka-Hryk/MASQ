using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Predicted_Value
{
    public class CCCmetric : MetricsQualitySoftware
    {
        public int CountOfFunction;
        private MainWindow _mainWindow;

        public struct TableForFunction_CCC
        {
            public int Number { get; set; }                    // номер функції
            public double LOC { get; set; }                    // кількість рядків функції
            public double Productivity { get; set; }           // продуктивність розроблення функції     
            public double Costs { get; set; }                  // витрати на реалізацію функції    
            public double GetLOC()
            {
                return LOC;
            }
            public void SetLOC(double loc)
            {
                LOC = loc;
            }
            public double GetProductivity()
            {
                return Productivity;
            }
            public void SetProductivity(double productivity)
            {
                Productivity = productivity;
            }
            public double GetCosts()
            {
                return Costs;
            }
            public void SetCosts(double costs)
            {
                Costs = costs;
            }
        }

        public CCCmetric() : base()
        {
            Console.WriteLine();
        }
        public CCCmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, int countOfFunction, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this.CountOfFunction = countOfFunction;
            this._mainWindow = win;
        }
        public CCCmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика прогнозування витрат на реалізацію програмного коду";
            this.Description = "Прогнозовані витрати на розроблення і-ої функції: \nBИТРАТИ(i) = LOC(оч i) / ПРОДУКТИВНІСТЬ(і),   і=1,m";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 70000;
            this.ParametersCount = 2;
            this.CountOfFunction = 1;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Очікувана кількість рядків вихідного коду функції", Value = 4670 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Прогнозована продуктивність розроблення ПЗ", Value = 3 };
            Metric.Add(secondParameter);

            this._mainWindow.InputTableInfoParameters_CCC_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": \n" + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "\nLOCоч - " + Metric[0].GetName() + "\n" +
                   "ПРОДУКТИВНІСТЬ - " + Metric[1].GetName() + "\n" +
                   "ВИТРАТИ = LOCоч / ПРОДУКТИВНІСТЬ (гривень)    CCCє[" + PermissibleMINvalue + ";0..." + PermisibleMAXvalue + "] \n";
        }

        public String ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(int numberFunction)    //вивід інформації по кожній функції 
        {
            return "--->>>> ФУНКЦІЯ " + numberFunction + " :\n" +
                   "LOCоч(" + numberFunction + ")= " + Metric[0].GetValue() + "\n" +
                   "ПРОДУКТИВНІСТЬ (" + numberFunction + ")= " + Metric[1].GetValue() + "\n" +
                   "ВИТРАТИ = LOCоч(" + numberFunction + ") / ПРОДУКТИВНІСТЬ(" + numberFunction + ")= " + Metric[0].GetValue() + " / " + Metric[1].GetValue() + " = " + FindMetric() + "(гривень)\n\n";
        }

        public override double FindMetric()
        {
            try
            {
                double result = (double)Metric[0].GetValue() / (double)Metric[1].GetValue();
                if ((result <= PermisibleMAXvalue) && (result >= PermissibleMINvalue))
                {
                    int intRes = (int)(result * 1000);
                    result = (double)intRes / 1000.0;
                    return result;
                }
                else
                {
                    MessageBox.Show("Некоректні вхідні дані, адже значення метрики (CCC = " + result + " ) виходить за допустимі межі [-1,0..70000]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
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
            this._mainWindow.OutputTableInfoParameters_CCC_dg.Items.Refresh();
        }
    }
}


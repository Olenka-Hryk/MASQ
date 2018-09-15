using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Predicted_Value
{
    public class CPTmetric : MetricsQualitySoftware
    {
        public int CountOfFunction;
        private MainWindow _mainWindow;

        public struct TableForFunction_CPT
        {
            public int Number { get; set; }                          // номер функції
            public double LOC_expected { get; set; }                 // очікувана кількість рядків вихідного коду функції
            public double LOC_similar { get; set; }                  // очікувана кількість рядків вихідного коду в аналогічній функції   
            public double Productivity_similar { get; set; }         // продуктивність процесу розроблення аналогічної функції
            public double Productivity { get; set; }                 // прогнозована продуктивність розроблення функції

            public double GetLOC_expected()
            {
                return LOC_expected;
            }
            public void SetLOC_expected(double lOC_expected)
            {
                LOC_expected = lOC_expected;
            }
            public double GetLOC_similar()
            {
                return LOC_similar;
            }
            public void SetLOC_similar(double lOC_similar)
            {
                LOC_similar = lOC_similar;
            }
            public double GetProductivity_similar()
            {
                return Productivity_similar;
            }
            public void SetProductivity_similar(double productivity_similar)
            {
                Productivity_similar = productivity_similar;
            }
            public double GetProductivity()
            {
                return Productivity;
            }
            public void SetProductivity(double productivity)
            {
                Productivity = productivity;
            }
        }

        public CPTmetric() : base()
        {
            Console.WriteLine();
        }
        public CPTmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, int countOfFunction, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this.CountOfFunction = countOfFunction;
            this._mainWindow = win;
        }
        public CPTmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика прогнозування продуктивності розроблення ПЗ";
            this.Description = "На основі продуктивності аналогічних функцій в аналогічних програмних продуктах. \nПрогноозована продуктивність процесу розроблення і-ої функції: \nПРОДУКТИВНІСТЬ(i) = ПРОДУКТИВНІСТЬ(ан i)* ( LOC(ан і) / LOC(оч i) ),   і=1,m";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 5;
            this.ParametersCount = 3;
            this.CountOfFunction = 1;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Очікувана кількість рядків вихідного коду функції", Value = 4670 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Очікувана кількість рядків вихідного коду в аналогічній функції", Value = 3980 };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "Продуктивність процесу розроблення аналогічної функції", Value = 2 };
            Metric.Add(thirdParameter);

            this._mainWindow.InputTableInfoParameters_CPT_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": \n" + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "\nLOC(оч) - " + Metric[0].GetName() + "\n" +
                   "LOC(ан) - " + Metric[1].GetName() + "\n" +
                   "ПРОДУКТИВНІСТЬ(ан) - " + Metric[2].GetName() + "\n" +
                   "ПРОДУКТИВНІСТЬ = ПРОДУКТИВНІСТЬ(ан)*(LOC(ан)/LOC(оч)) (хв на 1рядок коду) \n CPTє[" + PermissibleMINvalue + ";0..." + PermisibleMAXvalue + "] \n";
        }

        public String ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(int numberFunction)    //вивід інформації по кожній функції 
        {
            return "--->>>> ФУНКЦІЯ " + numberFunction + " :\n" +
                   "LOCоч(" + numberFunction + ")= " + Metric[0].GetValue() + "\n" +
                   "LOCан(" + numberFunction + ")= " + Metric[1].GetValue() + "\n" +
                   "ПРОДУКТИВНІСТЬан(" + numberFunction + ")= " + Metric[2].GetValue() + "\n" +
                   "ПРОДУКТИВНІСТЬ = ПРОДУКТИВНІСТЬан(" + numberFunction + ") * ( LOCан(" + numberFunction + ") / LOCоч(" + numberFunction + ") ) =\n = " + Metric[2].GetValue() + " * (" + Metric[1].GetValue() +" / "+ Metric[0].GetValue()+") = " + FindMetric() + " (хв/1рядок коду)\n\n";
        }

        public override double FindMetric()
        {
            try
            {
                double result = (double)Metric[2].GetValue() * ( (double)Metric[1].GetValue()/(double)Metric[0].GetValue() );
                if ((result <= PermisibleMAXvalue) && (result >= PermissibleMINvalue))
                {
                    int intRes = (int)(result * 1000);
                    result = (double)intRes / 1000.0;
                    return result;
                }
                else
                {
                    MessageBox.Show("Некоректні вхідні дані, адже значення метрики (CPT = " + result + " ) виходить за допустимі межі [-1,0..5]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
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
            this._mainWindow.OutputTableInfoParameters_CPT_dg.Items.Refresh();
        }
    }
}

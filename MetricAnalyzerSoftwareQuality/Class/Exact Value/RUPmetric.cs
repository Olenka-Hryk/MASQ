using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class
{
 public class RUPmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;

        public RUPmetric() : base()
        {
            Console.WriteLine();
        }
        public RUPmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }
        public RUPmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика звертання до глобальних змінних";
            this.Description = "Пара (модуль, глобальна змінна) – пара (р, r), де р – модуль, який має  \n доступ до глобальної змінної r. Залежно від наявності реального звертання до змінної r формуються 2 типи \n пар (р, r) – фактичні та можливі.";
            this.PermissibleMINvalue = 0;
            this.PermisibleMAXvalue = 1;
            this.ParametersCount = 2;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Cкільки разів модуль дійсно отримає доступ до глобальної змінної", Value = 10 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Cкільки разів модуль міг би отримати доступ до глобальної змінної", Value = 15 };
            Metric.Add(secondParameter);

            this._mainWindow.TableInfoParametrs_RUP_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return  " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "Aup - " + "   " + Metric[0].GetValue() + "\n" +
                   "Pup - " + "   " + Metric[1].GetValue() + "\n" +
                   "Rup = Aup/Pup = " + Metric[0].GetValue() + " / " + Metric[1].GetValue() + " = " + FindMetric();
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
                    MessageBox.Show("Некоректні вхідні дані, адже значення метрики (RUP = " + result +" ) виходить за допустимі межі [1,0..1]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
            }
            catch(Exception e)
            {
               MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + e, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
               return 0;
            }
        }

        public override void ClearAllParameters_OfMetric()
        {
            for (int i=0; i<ParametersCount; i++)
            {
                ChangeValue_OfParameter(i,0);
            }
            //((MainWindow)App.MainWin).TableInfoParametrs_RUP_dg.ItemsSource = Metric;
            this._mainWindow.TableInfoParametrs_RUP_dg.ItemsSource = Metric;
            this._mainWindow.TableInfoParametrs_RUP_dg.Items.Refresh();
        }
    }
}

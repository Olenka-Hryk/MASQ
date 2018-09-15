using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Exact_Value
{
    public class CPPmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;
        public CPPmetric() : base()
        {
            Console.WriteLine();
        }
        public CPPmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }
        public CPPmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика зчеплення";
            this.Description = " Зовнішня характеристика програмного модуля, яку бажано і слід зменшувати.\nЦе міра взаємозалежності модулів за даними.";
            this.PermissibleMINvalue = 0;
            this.PermisibleMAXvalue = 9;
            this.ParametersCount = 6;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Один модуль викликає інший модуль, всі вхідні і вихідні \nпараметри модуля, що викликаються - прості елементи даних.", Value = 1 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "В якості вхідних і вихідних параметрів використовується \nструктура даних.", Value = 3 };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "Один модуль явно керує функціюванням іншого модуля за \nдопомогою прапорців або перемикачів і надсилає йому \nкеруючі дані.", Value = 4 };
            Metric.Add(thirdParameter);
            Parameter fourthParameter = new Parameter() { Number = 4, Name = "Модулі посилаються на один і той же глобальний елемент \nданих.", Value = 5 };
            Metric.Add(fourthParameter);
            Parameter fifthParameter = new Parameter() { Number = 5, Name = "Модулі поділяють одну й ту ж глобальну структуру даних.", Value = 7 };
            Metric.Add(fifthParameter);
            Parameter sixthParameter = new Parameter() { Number = 6, Name = "Один модуль прямо посилається на вміст іншого модуля \nне через його точку входу.", Value = 9 };
            Metric.Add(sixthParameter);

            this._mainWindow.NameParameter_CPP_tb1.Text = firstParameter.GetName();
            this._mainWindow.NameParameter_CPP_tb2.Text = secondParameter.GetName();
            this._mainWindow.NameParameter_CPP_tb3.Text = thirdParameter.GetName();
            this._mainWindow.NameParameter_CPP_tb4.Text = fourthParameter.GetName();
            this._mainWindow.NameParameter_CPP_tb5.Text = fifthParameter.GetName();
            this._mainWindow.NameParameter_CPP_tb6.Text = sixthParameter.GetName();
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            int result = (int)FindMetric();
            string description = "";
            for (int i = 0; i < ParametersCount; i++)
            {
                if (Metric[i].GetValue() == result)
                {
                    description = Metric[i].GetName();
                }
            }
            return "C3ч = " + result + "\n" + description;
        }

        public override double FindMetric()
        {
            if (this._mainWindow.Value_1_CPP_rb.IsChecked == true)
            {
                return Metric[0].GetValue();
            }
            if (this._mainWindow.Value_2_CPP_rb.IsChecked == true)
            {
                return Metric[1].GetValue();
            }
            if (this._mainWindow.Value_3_CPP_rb.IsChecked == true)
            {
                return Metric[2].GetValue();
            }
            if (this._mainWindow.Value_4_CPP_rb.IsChecked == true)
            {
                return Metric[3].GetValue();
            }
            if (this._mainWindow.Value_5_CPP_rb.IsChecked == true)
            {
                return Metric[4].GetValue();
            }
            if (this._mainWindow.Value_6_CPP_rb.IsChecked == true)
            {
                return Metric[5].GetValue();
            }
            else
            {
                return 1;
            }
        }

        public override void ClearAllParameters_OfMetric()
        {
            this._mainWindow.Value_1_CPP_rb.IsChecked = false;
            this._mainWindow.Value_2_CPP_rb.IsChecked = false;
            this._mainWindow.Value_3_CPP_rb.IsChecked = false;
            this._mainWindow.Value_4_CPP_rb.IsChecked = false;
            this._mainWindow.Value_5_CPP_rb.IsChecked = false;
            this._mainWindow.Value_6_CPP_rb.IsChecked = false;

            MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

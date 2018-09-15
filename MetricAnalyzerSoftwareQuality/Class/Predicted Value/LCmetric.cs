using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Predicted_Value
{
    public class LCmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;
        public LCmetric() : base()
        {
            Console.WriteLine();
        }
        public LCmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }
        public LCmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика прогнозування оцінки трудовитрат за моделлю Боема";
            this.Description = "Трудовитрати на розробку програмних продуктів \nзростають швидше, ніж розмір додатків. Для представлення даного співвідношення використовується \nекспоненційна функція зі значенням показника, близьким до 1,12.";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 394;
            this.ParametersCount = 3;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            int _default = 0;

            Parameter firstParameter = new Parameter() { Number = 1, Name = "Кількість тисяч рядків програмного коду", Value = 4 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Коефіцієнт СОСОМО (а)", Value = _default };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "Коефіцієнт СОСОМО (b)", Value = _default };
            Metric.Add(thirdParameter);

            this._mainWindow.TableInfoParametrs_LC_dg.ItemsSource = Metric;
        }

        public void SetAllParametersWithSelectedFactor_OfMetric(int ChoiceOfCoefficients)    // встановлення коефіцієнтів СОСОМО для певних параметрів метрики за певним вибором типу програмного проекту; ChoiceOfCoefficients- (може бути 1,2,3) - варіант, за яким обираються коефіцієнти СОСОМО
        {
            if (ChoiceOfCoefficients == 1)
            {
                Parameter firstParameter = new Parameter() { Number = 1, Name = "Кількість тисяч рядків програмного коду", Value = 0 };
                Metric.Add(firstParameter);
                Parameter secondParameter = new Parameter() { Number = 2, Name = "Коефіцієнт СОСОМО (а)", Value = 2.4 };
                Metric.Add(secondParameter);
                Parameter thirdParameter = new Parameter() { Number = 3, Name = "Коефіцієнт СОСОМО (b)", Value = 1.05 };
                Metric.Add(thirdParameter);

                this._mainWindow.TableInfoParametrs_LC_dg.ItemsSource = Metric;
            }
            if (ChoiceOfCoefficients == 2)
            {
                Parameter firstParameter = new Parameter() { Number = 1, Name = "Кількість тисяч рядків програмного коду", Value = 0 };
                Metric.Add(firstParameter);
                Parameter secondParameter = new Parameter() { Number = 2, Name = "Коефіцієнт СОСОМО (а)", Value = 3.6 };
                Metric.Add(secondParameter);
                Parameter thirdParameter = new Parameter() { Number = 3, Name = "Коефіцієнт СОСОМО (b)", Value = 1.20 };
                Metric.Add(thirdParameter);

                this._mainWindow.TableInfoParametrs_LC_dg.ItemsSource = Metric;
            }
            if (ChoiceOfCoefficients == 3)
            {
                Parameter firstParameter = new Parameter() { Number = 1, Name = "Кількість тисяч рядків програмного коду", Value = 0};
                Metric.Add(firstParameter);
                Parameter secondParameter = new Parameter() { Number = 2, Name = "Коефіцієнт СОСОМО (а)", Value = 3.0 };
                Metric.Add(secondParameter);
                Parameter thirdParameter = new Parameter() { Number = 3, Name = "Коефіцієнт СОСОМО (b)", Value = 1.12 };
                Metric.Add(thirdParameter);

                this._mainWindow.TableInfoParametrs_LC_dg.ItemsSource = Metric;
            }
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "KLOC - " + "   " + Metric[0].GetValue() + "\n" +
                   "a - " + "   " + Metric[1].GetValue() + "\n" +
                   "b - " + "   " + Metric[2].GetValue() + "\n" +
                   "LC = a * KLOC^b = " + Metric[1].GetValue() + " * " + Metric[0].GetValue() +"^"+ Metric[2].GetValue() + " = " + FindMetric() +"   (людиномісяців)" ;
        }

        public override double FindMetric()
        {
            try
            {
                double result = (double)Metric[1].GetValue() * Math.Pow((double)Metric[0].GetValue(),(double)Metric[2].GetValue());
                if ((result <= PermisibleMAXvalue) && (result >= PermissibleMINvalue))
                {
                    int intRes = (int)(result * 1000);
                    result = (double)intRes / 1000.0;
                    return result;
                }
                else
                {
                    MessageBox.Show("Некоректні вхідні дані, адже значення метрики (LC = " + result + " ) виходить за допустимі межі [-1,0..394]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
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
            for (int i = 0; i < ParametersCount; i++)
            {
                ChangeValue_OfParameter(i, 0);
            }
            Metric.RemoveRange(0, 3);
            this._mainWindow.TableInfoParametrs_LC_dg.ItemsSource = Metric;
            this._mainWindow.TableInfoParametrs_LC_dg.Items.Refresh();
        }
    }
}

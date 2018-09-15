using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Exact_Value
{
    public class MBQmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;
        public MBQmetric() : base()
        {
            Console.WriteLine();
        }
        public MBQmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }
        public MBQmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика загальної кількості знайдених помилок при інспектуванні моделей та прототипів модулів";
            this.Description = "\nКількість знайдених помилок при інспектуванні моделей та прототипів підсистем, модулів, функцій, вимог та \nгустота поміток (кількість помилок на одну підсистему, модуль, фуекцію, вимогу) - вказує на проблемну підсистему, \nмодуль, функцію, вимогу.";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 5000;
            this.ParametersCount = 2;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Кількість помилок модуля", Value = 8 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Кількість модулів", Value = 24 };
            Metric.Add(secondParameter);

            this._mainWindow.TableInfoParametrs_MBQ_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "Qbm = " + Metric[0].GetValue() + "- " + Metric[0].GetName() + "\n" +
                   "Qm = " + Metric[1].GetValue() + "- " + Metric[1].GetName() + "\n" +
                   "MBQ=f(Qbm,Qm)   MBQє[" + PermissibleMINvalue + ";0..." + PermisibleMAXvalue + "]" + "\nОскільки, в класичній літературі немає чітко вираженої формули знаходження цієї \nметрики - результат метрики становить 0\n";
        }

        public override double FindMetric()
        {
            //формула для MBQ ??? не досліджено
            return 0;
        }

        public override void ClearAllParameters_OfMetric()
        {
            for (int i = 0; i < ParametersCount; i++)
            {
                ChangeValue_OfParameter(i, 0);
            }
            this._mainWindow.TableInfoParametrs_MBQ_dg.ItemsSource = Metric;
            this._mainWindow.TableInfoParametrs_MBQ_dg.Items.Refresh();
        }
    }
}

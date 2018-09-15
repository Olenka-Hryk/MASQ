using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Exact_Value
{
    public class MMTmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;
        public MMTmetric() : base()
        {
            Console.WriteLine();
        }
        public MMTmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }
        public MMTmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика часу модифікації моделей";
            this.Description = "Метрика процесу розроблення програмного забезпечення. \nВимірюється в робочих днях.";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 46;
            this.ParametersCount = 3;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Кількість рядків програмного коду", Value = 4670 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Тривалість програмного проекту", Value = 345 };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "Частина етапу проектування в ЖЦ проекту", Value = 2 };
            Metric.Add(thirdParameter);

            this._mainWindow.TableInfoParametrs_MMT_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "Qcl = " + Metric[0].GetValue() + "- " + Metric[0].GetName() + "\n" +
                   "Pd = " + Metric[1].GetValue() + "- " + Metric[1].GetName() + "\n" +
                   "Sdslc = " + Metric[2].GetValue() + "- " + Metric[2].GetName() + "\n" +
                   "MMT=f(Qcl,Pd,Sdslc) (робочих днів)   MMTє[" +PermissibleMINvalue+";0..."+PermisibleMAXvalue+"]"+"\nОскільки, в класичній літературі немає чітко вираженої формули знаходження цієї \nметрики - результат метрики становить 0\n";
        }

        public override double FindMetric()
        {
            //формула для MMT ??? не досліджено
            return 0;
        }

        public override void ClearAllParameters_OfMetric()
        {
            for (int i = 0; i < ParametersCount; i++)
            {
                ChangeValue_OfParameter(i, 0);
            }
            this._mainWindow.TableInfoParametrs_MMT_dg.ItemsSource = Metric;
            this._mainWindow.TableInfoParametrs_MMT_dg.Items.Refresh();
        }
    }
}

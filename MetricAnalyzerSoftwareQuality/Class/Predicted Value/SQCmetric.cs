using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Predicted_Value
{
    public class SQCmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;
        public SQCmetric() : base()
        {
            Console.WriteLine();
        }
        public SQCmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }
        public SQCmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика прогнозування вартості перевірки якості ПЗ";
            this.Description = "\nМетрика процесу розроблення програмного забезпечення.";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 20000;
            this.ParametersCount = 4;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Частина етапу верифікації, валідації та тестування у ЖЦ ПЗ", Value = 1 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Частина стадії перевірки якості етапу верифікації, валідації та тестування", Value = 2 };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "Кількість рядків програмного коду", Value = 4670 };
            Metric.Add(thirdParameter);
            Parameter fourthParameter = new Parameter() { Number = 4, Name = "Очікувана вартість рядка", Value = 10 };
            Metric.Add(fourthParameter);

            this._mainWindow.TableInfoParametrs_SQC_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "Svvtqlc = " + Metric[0].GetValue() + "- " + Metric[0].GetName() + "\n" +
                   "Sqavvtq = " + Metric[1].GetValue() + "- " + Metric[1].GetName() + "\n" +
                   "Qcl = " + Metric[2].GetValue() + "- " + Metric[2].GetName() + "\n" +
                   "Col = " + Metric[3].GetValue() + "- " + Metric[3].GetName() + "\n" +
                   "SQC=f(Svvtqlc,Sqavvtq,Qcl,Col) (грн., $США)   SQCє[" + PermissibleMINvalue + ";0..." + PermisibleMAXvalue + "]" + "\nОскільки, в класичній літературі немає чітко вираженої формули знаходження цієї \nметрики - результат метрики становить 0\n";
        }

        public override double FindMetric()
        {
            //формула для SQC ??? не досліджено
            return 0;
        }

        public override void ClearAllParameters_OfMetric()
        {
            for (int i = 0; i < ParametersCount; i++)
            {
                ChangeValue_OfParameter(i, 0);
            }
            this._mainWindow.TableInfoParametrs_SQC_dg.ItemsSource = Metric;
            this._mainWindow.TableInfoParametrs_SQC_dg.Items.Refresh();
        }
    }
}

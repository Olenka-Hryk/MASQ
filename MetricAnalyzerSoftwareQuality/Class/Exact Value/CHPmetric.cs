using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Exact_Value
{
    public class CHPmetric : MetricsQualitySoftware
    {
        private MainWindow _mainWindow;
        public CHPmetric() : base()
        {
            Console.WriteLine();
        }
        public CHPmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this._mainWindow = win;
        }

        public CHPmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика зв'язності";
            this.Description = " Внутрішня характеристика програмного модуля - залежить від типу модуля або проекту.\n Недолік: чим вища зв'язність модуля з іншими модулями, тим вища чутливість до внесення змін, тобто \n високий ступінь зв'язності модуля з іншими модулями.";
            this.PermissibleMINvalue = 0;
            this.PermisibleMAXvalue = 10;
            this.ParametersCount = 7;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "Модуль - одинична проблемно-орієнтована функція \n(частина модуля разом реалізують одну функцію)", Value = 10 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "Дії всередині модуля зв'язані даними, і порядок дій всередені \nмодуля важливий (вихідні дані однієї частини використовуються як \nвхідні дані в іншій частині модуля)", Value = 9 };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "Дії всередині модуля зв'язані даними, і порядок дій всередині \nмодуля не має жодного значення (частини модуля зв'язані даними - \nпрацюють з однією структурою даних)", Value = 7 };
            Metric.Add(thirdParameter);
            Parameter fourthParameter = new Parameter() { Number = 4, Name = "Дії всередині модуля зв'язані потоком керування, і порядок дій \nвсередині модуля важливий (частини модуля зв'язані порядком \nвиконуваних ним дій, які реалізують деякий сценарій поведінки)", Value = 5 };
            Metric.Add(fourthParameter);
            Parameter fifthParameter = new Parameter() { Number = 5, Name = "Дії всередині модуля зв'язані потоком керування, і порядок дій \nвсередині модуля не має жодного значення (частини модуля не \nзв'язані, але необхідні в один і той же момент роботи системи) ", Value = 3 };
            Metric.Add(fifthParameter);
            Parameter sixthParameter = new Parameter() { Number = 6, Name = "Дії всередині модуля зовсім не зв'язані, але належать до \nоднієї категорії (частини модуля об'єднані за принципом \nфункційної подібності)", Value = 1 };
            Metric.Add(sixthParameter);
            Parameter seventhParameter = new Parameter() { Number = 7, Name = "Дії всередені модуля зовсім не зв'язані, а також не \nналежать до однієї категорії (в модулі відсутні явно виражені \nвнутрішні зв'язки)", Value = 0 };
            Metric.Add(seventhParameter);

            this._mainWindow.NameParameter_CHP_tb1.Text = firstParameter.GetName();
            this._mainWindow.NameParameter_CHP_tb2.Text = secondParameter.GetName();
            this._mainWindow.NameParameter_CHP_tb3.Text = thirdParameter.GetName();
            this._mainWindow.NameParameter_CHP_tb4.Text = fourthParameter.GetName();
            this._mainWindow.NameParameter_CHP_tb5.Text = fifthParameter.GetName();
            this._mainWindow.NameParameter_CHP_tb6.Text = sixthParameter.GetName();
            this._mainWindow.NameParameter_CHP_tb7.Text = seventhParameter.GetName();
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": " + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            int result = (int)FindMetric();
            string description = "";
            for (int i=0;  i<ParametersCount; i++)
            {
                if(Metric[i].GetValue() == result)
                {
                    description = Metric[i].GetName();
                }
            }
            return "C3 = " + result + "\n " + description;
        }

        public override double FindMetric()
        {
            if (this._mainWindow.Value_1_CHP_rb.IsChecked == true)
            {
                return Metric[0].GetValue();
            }
            if (this._mainWindow.Value_2_CHP_rb.IsChecked == true)
            {
                return Metric[1].GetValue();
            }
            if (this._mainWindow.Value_3_CHP_rb.IsChecked == true)
            {
                return Metric[2].GetValue();
            }
            if (this._mainWindow.Value_4_CHP_rb.IsChecked == true)
            {
                return Metric[3].GetValue();
            }
            if (this._mainWindow.Value_5_CHP_rb.IsChecked == true)
            {
                return Metric[4].GetValue();
            }
            if (this._mainWindow.Value_6_CHP_rb.IsChecked == true)
            {
                return Metric[5].GetValue();
            }
            if (this._mainWindow.Value_7_CHP_rb.IsChecked == true)
            {
                return Metric[6].GetValue();
            }
            else
            {
                return 0;
            }
        }

        public override void ClearAllParameters_OfMetric()
        {
            this._mainWindow.Value_1_CHP_rb.IsChecked = false;
            this._mainWindow.Value_2_CHP_rb.IsChecked = false;
            this._mainWindow.Value_3_CHP_rb.IsChecked = false;
            this._mainWindow.Value_4_CHP_rb.IsChecked = false;
            this._mainWindow.Value_5_CHP_rb.IsChecked = false;
            this._mainWindow.Value_6_CHP_rb.IsChecked = false;

            MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

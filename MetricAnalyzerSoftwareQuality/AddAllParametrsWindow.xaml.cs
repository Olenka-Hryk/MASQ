using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Exact_Value;
using MetricAnalyzerSoftwareQuality.Class.Predicted_Value;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MetricAnalyzerSoftwareQuality.MetricsQualitySoftware;

namespace MetricAnalyzerSoftwareQuality
{
    public partial class AddAllParametrsWindow : Window
    {
        public List<Parameter> ListParameters = new List<Parameter>();
        MMTmetric mmtMetric = new MMTmetric(App.MainWin);
        SCTmetric sctMetric = new SCTmetric(App.MainWin);
        SDTmetric sdtMetric = new SDTmetric(App.MainWin);
        SQCmetric sqcMetric = new SQCmetric(App.MainWin);


        public AddAllParametrsWindow()
        {
            InitializeComponent();
            titleBar.MouseLeftButtonDown += (o, e) => DragMove();

            Parameter Parameter1 = new Parameter() { Number = 1, Name = "Кількість рядків програмного коду", Value = 0 };
            ListParameters.Add(Parameter1);
            Parameter Parameter2 = new Parameter() { Number = 2, Name = "Тривалість програмного проекту", Value = 0 };
            ListParameters.Add(Parameter2);
            Parameter Parameter3 = new Parameter() { Number = 3, Name = "Частина етапу проектування в ЖЦ проекту", Value = 0 };
            ListParameters.Add(Parameter3);

            AllValueParametrs_dataGrid.ItemsSource = ListParameters;
            AllValueParametrs_dataGrid.Items.Refresh();
            mmtMetric.SetAllParametersWithDefaultValue_OfMetric();
            sctMetric.SetAllParametersWithDefaultValue_OfMetric();
            sdtMetric.SetAllParametersWithDefaultValue_OfMetric();
            sqcMetric.SetAllParametersWithDefaultValue_OfMetric();
        }

        //< CancelAddAllParametrs_Button_Click > -> скасування змін у таблиці всіх параметрів метрик
        private void CancelAddAllParametrs_Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBox.Show("Ви дійсно хочете скасувати всі зміни без збереження даних занесених до значень вхідних параметрів?", "Скасування:", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                MainWindow window = new MainWindow();
                window.Information_ProgramDo.Text = "Готово";
                window.Information_UserDo.Text = "Список значень всіх параметрів метрик змінено";
                this.Close();
            }
            else
            { }
        }

        //< SaveAddAllParametrs_Button_Click > -> зберегти всі параметри метрик
        private void SaveAddAllParametrs_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cel1 = DataGridHelper.GetCell(AllValueParametrs_dataGrid, 0, 1);
                var content1 = cel1.Content as TextBlock;
                var parameter1 = content1.Text;
                mmtMetric.ChangeValue_OfParameter(0, double.Parse(parameter1, CultureInfo.InvariantCulture));
                sctMetric.ChangeValue_OfParameter(0, double.Parse(parameter1, CultureInfo.InvariantCulture));
                sdtMetric.ChangeValue_OfParameter(0, double.Parse(parameter1, CultureInfo.InvariantCulture));
                sqcMetric.ChangeValue_OfParameter(2, double.Parse(parameter1, CultureInfo.InvariantCulture));

                var cel2 = DataGridHelper.GetCell(AllValueParametrs_dataGrid, 1, 1);
                var content2 = cel2.Content as TextBlock;
                var parameter2 = content2.Text;
                mmtMetric.ChangeValue_OfParameter(1, double.Parse(parameter2, CultureInfo.InvariantCulture));
                sctMetric.ChangeValue_OfParameter(1, double.Parse(parameter2, CultureInfo.InvariantCulture));
                sdtMetric.ChangeValue_OfParameter(1, double.Parse(parameter2, CultureInfo.InvariantCulture));

                var cel3 = DataGridHelper.GetCell(AllValueParametrs_dataGrid, 2, 1);
                var content3 = cel3.Content as TextBlock;
                var parameter3 = content3.Text;
                mmtMetric.ChangeValue_OfParameter(2, double.Parse(parameter3, CultureInfo.InvariantCulture));
                sdtMetric.ChangeValue_OfParameter(2, double.Parse(parameter3, CultureInfo.InvariantCulture));

                ((MainWindow)App.MainWin).TableInfoParametrs_MMT_dg.Items.Refresh();
                ((MainWindow)App.MainWin).TableInfoParametrs_SCT_dg.Items.Refresh();
                ((MainWindow)App.MainWin).TableInfoParametrs_SDT_dg.Items.Refresh();
                ((MainWindow)App.MainWin).TableInfoParametrs_SQC_dg.Items.Refresh();
                
                if (MessageBox.Show("Основні параметри метрик успішно внесені та збережені! Продовжіть свою роботу та доповніть метрики необхідними даними!", "Інформація:", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    this.Close();
                }
                else
                { }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Помилка збереження значень параметрів!" + exc, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // < TableInfoParametrs_PreviewTextInput > - функція блокування вводу літер та символів для коректного вводу значень для параметрів (лише цифри)
        private void TableInfoParametrs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}

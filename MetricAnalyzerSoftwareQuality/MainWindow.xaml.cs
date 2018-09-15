using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Exact_Value;
using MetricAnalyzerSoftwareQuality.Class.Predicted_Value;
using MetricAnalyzerSoftwareQuality.Class.Table;
using Microsoft.Win32;
using System;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MetricAnalyzerSoftwareQuality.Class.Predicted_Value.CCCmetric;
using static MetricAnalyzerSoftwareQuality.Class.Predicted_Value.CPTmetric;
using static MetricAnalyzerSoftwareQuality.Class.Predicted_Value.FPmetric;
using static MetricAnalyzerSoftwareQuality.Class.Predicted_Value.SCCmetric;
using static MetricAnalyzerSoftwareQuality.MetricsQualitySoftware;

namespace MetricAnalyzerSoftwareQuality
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<MyTableInfo_allResultsMetrics> _tableResult;

        List<MyTableInfoCharacteristic_forMetricFP> listOfCharacteristics = new List<MyTableInfoCharacteristic_forMetricFP>(0);
        List<int> listOfValueCharacteristics = new List<int>() { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        public Func<ChartPoint, string> MetricsAllPie { get; set; }
        public List<MyTableInfo_allResultsMetrics> TableResult
        {
            get
            {
                return this._tableResult;
            }

            set
            {
                this._tableResult = value;
                NotifyPropertyChanged("TableResult");
            }
        }
        int countOfOperationCalculation_OfMetric = 1;
        int countOfFunction_SCC = 1;
        int countOfFunction_CPT = 1;
        int countOfFunction_CCC = 1;
        int countOfFunction_FP = 1;

        private bool _specialRequirement = false;
        int koefEI, koefEO, koefEIN, koefILF, koefELF;
        private bool EI_INFO_showORhide = false, EO_INFO_showORhide = false, EIN_INFO_showORhide = false, ILF_INFO_showORhide = false, ELF_INFO_showORhide = false;

        private bool _statusBar_start_info = true;
        TreeViewItem item_Metric_exact_value, item_Metric_predicted_value;
        TreeViewItem item_Metric_CHP, item_Metric_CPP, item_Metric_RUP, item_Metric_MMT, item_Metric_MBQ,
                     item_Metric_SCT, item_Metric_SDT, item_Metric_SCC, item_Metric_SQC, item_Metric_CPT, item_Metric_CCC, item_Metric_FP, item_Metric_LC, item_Metric_DP;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        //--метрики з точним значенням
        CHPmetric chpMetric;
        CPPmetric cppMetric;
        RUPmetric rupMetric;
        MMTmetric mmtMetric;
        MBQmetric mbqMetric;

        //--метрики з прогнозованим значення
        SCTmetric sctMetric;
        SDTmetric sdtMetric;
        SCCmetric sccMetric;
        SQCmetric sqcMetric;
        CPTmetric cptMetric;
        CCCmetric cccMetric;
        FPmetric fpMetric;
        LCmetric lcMetric;
        DPmetric dpMetric;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            TableResult = new List<MyTableInfo_allResultsMetrics>()
            {
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 1,
                    Value =  0,
                    ID = "CHP",
                    Name = "Метрика зв'язності",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 2,
                    Value =  0,
                    ID = "CPP",
                    Name = "Метрика зчеплення",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 3,
                    Value =  0,
                    ID = "RUP",
                    Name = "Метрика звертання до глобальних змінних",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 4,
                    Value =  0,
                    ID = "MMT",
                    Name = "Метрика часу модифікації моделей",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 5,
                    Value =  0,
                    ID = "MBQ",
                    Name = "Метрика загальної кількості знайдених помилок при інспектуванні моделей та прототипів модулів",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 6,
                    Value =  0,
                    ID = "SCT",
                    Name = "Метрика прогнозування загального часу розроблення ПЗ",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 7,
                    Value =  0,
                    ID = "SDT",
                    Name = "Метрика часу виконання робіт процесу проектування ПЗ",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 8,
                    Value =  0,
                    ID = "SCC",
                    Name = "Метрика очікуваної вартості розроблення ПЗ",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 9,
                    Value =  0,
                    ID = "SQC",
                    Name = "Метрика прогнозування вартості перевірки якості ПЗ",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 10,
                    Value =  0,
                    ID = "CPT",
                    Name = "Метрика прогнозування продуктивності розроблення ПЗ",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 11,
                    Value =  0,
                    ID = "CCC",
                    Name = "Метрика прогнозування витрат на реалізацію програмного коду",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 12,
                    Value =  0,
                    ID = "FP",
                    Name = "Метрика прогнозування функційного розміру",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 13,
                    Value =  0,
                    ID = "LC",
                    Name = "Метрика прогнозування оцінки трудовитрат за моделлю Боема",
                },
                new MyTableInfo_allResultsMetrics()
                {
                    Number = 14,
                    Value =  0,
                    ID = "DP",
                    Name = "Метрика прогнозування оцінки тривалості проекту за моделлю Боема",
                }
            };

            this.chpMetric = new CHPmetric(this);
            this.cppMetric = new CPPmetric(this);
            this.rupMetric = new RUPmetric(this);
            this.mmtMetric = new MMTmetric(this);
            this.mbqMetric = new MBQmetric(this);

            this.sctMetric = new SCTmetric(this);
            this.sdtMetric = new SDTmetric(this);
            this.sccMetric = new SCCmetric(this);
            this.sqcMetric = new SQCmetric(this);
            this.cptMetric = new CPTmetric(this);
            this.cccMetric = new CCCmetric(this);
            this.fpMetric = new FPmetric(this);
            this.lcMetric = new LCmetric(this);
            this.dpMetric = new DPmetric(this);    
           
            TreeView_findItem();
            item_Metric_CHP.Focus();
            Result_OfAllMetric_ti.Focus();
            AllMetrics_TabControl.Visibility = Visibility.Visible;
            AnalyzeOfAllResults_Grid.Visibility = Visibility.Hidden;
            AnalyzeOfAllResults_ScrollViewerForThisTab.Visibility = Visibility.Hidden;

            //CHP
            chpMetric.SetInformation_OfMetric();
            chpMetric.SetAllParametersWithDefaultValue_OfMetric();
            //CPP
            cppMetric.SetInformation_OfMetric();
            cppMetric.SetAllParametersWithDefaultValue_OfMetric();
            //RUP
            rupMetric.SetInformation_OfMetric();
            rupMetric.SetAllParametersWithDefaultValue_OfMetric();
            //MMT
            mmtMetric.SetInformation_OfMetric();
            mmtMetric.SetAllParametersWithDefaultValue_OfMetric();
            //MBQ
            mbqMetric.SetInformation_OfMetric();
            mbqMetric.SetAllParametersWithDefaultValue_OfMetric();
            //SCT
            sctMetric.SetInformation_OfMetric();
            sctMetric.SetAllParametersWithDefaultValue_OfMetric();
            //SDT
            sdtMetric.SetInformation_OfMetric();
            sdtMetric.SetAllParametersWithDefaultValue_OfMetric();
            //SCC
            sccMetric.SetInformation_OfMetric();
            sccMetric.SetAllParametersWithDefaultValue_OfMetric();
            //SQC
            sqcMetric.SetInformation_OfMetric();
            sqcMetric.SetAllParametersWithDefaultValue_OfMetric();
            //CPT
            cptMetric.SetInformation_OfMetric();
            cptMetric.SetAllParametersWithDefaultValue_OfMetric();
            //CCC
            cccMetric.SetInformation_OfMetric();
            cccMetric.SetAllParametersWithDefaultValue_OfMetric();
            //FP
            fpMetric.SetInformation_OfMetric();
            fpMetric.SetAllParametersWithDefaultValue_OfMetric();
            //LC
            lcMetric.SetInformation_OfMetric();
            //DP
            dpMetric.SetInformation_OfMetric();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "CHP",
                    Values = new ChartValues<double> {
                    TableResult[0].Value,
                    }
                },
                 new ColumnSeries
                {
                    Title = "CPP",
                    Values = new ChartValues<double> {
                    TableResult[1].Value,
                    }
                },
                  new ColumnSeries
                {
                    Title = "RUP",
                    Values = new ChartValues<double> {
                    TableResult[2].Value,
                    }
                },
                   new ColumnSeries
                {
                    Title = "MMT",
                    Values = new ChartValues<double> {
                    TableResult[3].Value,
                    }
                },
                   new ColumnSeries
                {
                    Title = "MBQ",
                    Values = new ChartValues<double> {
                    TableResult[4].Value,
                    }
                },
                    new ColumnSeries
                {
                    Title = "SCT",
                    Values = new ChartValues<double> {
                    TableResult[5].Value,
                    }
                },
                    new ColumnSeries
                {
                    Title = "SDT",
                    Values = new ChartValues<double> {
                    TableResult[6].Value,
                    }
                },
                    new ColumnSeries
                {
                    Title = "SCC",
                    Values = new ChartValues<double> {
                    TableResult[7].Value,
                    }
                },
                    new ColumnSeries
                {
                    Title = "SQC",
                    Values = new ChartValues<double> {
                    TableResult[8].Value,
                    }
                },
                    new ColumnSeries
                {
                    Title = "CPT",
                    Values = new ChartValues<double> {
                    TableResult[9].Value,
                    }
                },
                     new ColumnSeries
                {
                    Title = "CCC",
                    Values = new ChartValues<double> {
                    TableResult[10].Value,
                    }
                },
                     new ColumnSeries
                {
                    Title = "FP",
                    Values = new ChartValues<double> {
                    TableResult[11].Value,
                    }
                },
                     new ColumnSeries
                {
                    Title = "LC",
                    Values = new ChartValues<double> {
                    TableResult[12].Value,
                    }
                },
                     new ColumnSeries
                {
                    Title = "DP",
                    Values = new ChartValues<double> {
                    TableResult[13].Value,
                    }
                }
            };

            Labels = new[] { "CHP", "CPP", "RUP", "MMT", "MBQ", "SCT", "SDT", "SCC", "SQC", "CPT", "CCC", "FP", "LC", "DP"};
            Formatter = value => value.ToString("N");
            DataContext = this;

            Pie.Series.Add(new PieSeries { Title = "CHP", Values = new ChartValues<double> { TableResult[0].Value } });
            Pie.Series.Add(new PieSeries { Title = "CPP", Values = new ChartValues<double> { TableResult[1].Value } });
            Pie.Series.Add(new PieSeries { Title = "RUP", Values = new ChartValues<double> { TableResult[2].Value } });
            Pie.Series.Add(new PieSeries { Title = "MMT", Values = new ChartValues<double> { TableResult[3].Value } });
            Pie.Series.Add(new PieSeries { Title = "MBQ", Values = new ChartValues<double> { TableResult[4].Value } });
            Pie.Series.Add(new PieSeries { Title = "SCT", Values = new ChartValues<double> { TableResult[5].Value } });
            Pie.Series.Add(new PieSeries { Title = "SDT", Values = new ChartValues<double> { TableResult[6].Value } });
            Pie.Series.Add(new PieSeries { Title = "SCC", Values = new ChartValues<double> { TableResult[7].Value } });
            Pie.Series.Add(new PieSeries { Title = "SQC", Values = new ChartValues<double> { TableResult[8].Value } });
            Pie.Series.Add(new PieSeries { Title = "CPT", Values = new ChartValues<double> { TableResult[9].Value } });
            Pie.Series.Add(new PieSeries { Title = "CCC", Values = new ChartValues<double> { TableResult[10].Value } });
            Pie.Series.Add(new PieSeries { Title = "FP", Values = new ChartValues<double> { TableResult[11].Value } });
            Pie.Series.Add(new PieSeries { Title = "LC", Values = new ChartValues<double> { TableResult[12].Value } });
            Pie.Series.Add(new PieSeries { Title = "DP", Values = new ChartValues<double> { TableResult[13].Value } });
            DataContext = this;
        }


        //------------------------------------------------------------------------------------------------
        // ОСНОВНИЙ СПИСОК ФУНКЦІЙ ДЛЯ МЕНЮ
        //------------------------------------------------------------------------------------------------
        // < Search_parameter_Click > - пошук певного параметра
        private void Search_parameter_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(5);
            Information_UserDo.Text = "Пошук параметра ...";
            Information_ProgramDo.Text = "Готово";
            WindowSearchMetricOrParameter windowSearchParameter = new WindowSearchMetricOrParameter();
            windowSearchParameter.Show();
            windowSearchParameter.SearchMetricOrParameter_tabControl.SelectedIndex = 1;
        }

        // < Search_metric_Click > - пошук певної метрики
        private void Search_metric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(5);
            Information_UserDo.Text = "Пошук метрики ...";
            Information_ProgramDo.Text = "Готово";
            WindowSearchMetricOrParameter windowSearchMetric = new WindowSearchMetricOrParameter();
            windowSearchMetric.Show();
            windowSearchMetric.SearchMetricOrParameter_tabControl.SelectedIndex = 0;
        }


        // < Information_aboutProgram_Click > - функція виводу інформації про програму
        private void Information_aboutProgram_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(5);
            Information_UserDo.Text = "Вивід інформації про програму ...";
            Information_ProgramDo.Text = "Готово";
            WindowInfoAboutProgram windowAboutProgram = new WindowInfoAboutProgram();
            windowAboutProgram.Show();
        }

        // < Information_aboutAuthor_Click > - функція виводу інформації про автора
        private void Information_aboutAuthor_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(5);
            Information_UserDo.Text = "Вивід інформації про автора ...";
            Information_ProgramDo.Text = "Готово";
            WindowInfoAboutAuthor windowAboutAuthor = new WindowInfoAboutAuthor();
            windowAboutAuthor.Show();
        }


        //------------------------------------------------------------------------------------------------
        // ОСНОВНИЙ СПИСОК ФУНКЦІЙ ДЛЯ ПАНЕЛІ ІНСТРУМЕНТІВ
        //------------------------------------------------------------------------------------------------
        // < Search_parameter_OR_metric_Click > - пошук певного параметра чи метрики
        private void Search_parameter_OR_metric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(5);
            Information_UserDo.Text = "Пошук ...";
            Information_ProgramDo.Text = "Готово";
            WindowSearchMetricOrParameter windowSearchMetricOrParameter = new WindowSearchMetricOrParameter();
            windowSearchMetricOrParameter.Show();
        }

        // < Report_Click > - функція звітності по метриках
        private void Report_Click(object sender, RoutedEventArgs e)
        {
            Title.Content = "Аналіз отриманих результатів";
            AllMetrics_TabControl.Visibility = Visibility.Hidden;
            AnalyzeOfAllResults_ScrollViewerForThisTab.Visibility = Visibility.Visible;
            AnalyzeOfAllResults_Grid.Visibility = Visibility.Visible;
            TimeOfAnalyze.Content = System.DateTime.Today.ToString("dd/MM/yy") + " " + DateTime.Now.ToString("hh:mm:ss tt");
            TableInfoResult_all_dg.Items.Refresh();
            TextOfAnalyze_txtBlock.Text = AnalyzeOfAllReceiveValue_OfMetrics();
            Analyze_OfAllMetric_TextBox.Text = AnalyzeOfAllReceiveValue_OfMetrics();
            TextOfShnm_txtBlock.Text = Construct_SHNM();
            Pie.Height = 300;
            Cartesian.Height = 300;

            MetricsAllPie = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);


            SeriesCollection[0].Values = new ChartValues<double> { TableResult[0].Value };
            SeriesCollection[1].Values = new ChartValues<double> { TableResult[1].Value };
            SeriesCollection[2].Values = new ChartValues<double> { TableResult[2].Value };
            SeriesCollection[3].Values = new ChartValues<double> { TableResult[3].Value };
            SeriesCollection[4].Values = new ChartValues<double> { TableResult[4].Value };
            SeriesCollection[5].Values = new ChartValues<double> { TableResult[5].Value };
            SeriesCollection[6].Values = new ChartValues<double> { TableResult[6].Value };
            SeriesCollection[7].Values = new ChartValues<double> { TableResult[7].Value };
            SeriesCollection[8].Values = new ChartValues<double> { TableResult[8].Value };
            SeriesCollection[9].Values = new ChartValues<double> { TableResult[9].Value };
            SeriesCollection[10].Values = new ChartValues<double> { TableResult[10].Value };
            SeriesCollection[11].Values = new ChartValues<double> { TableResult[11].Value };
            SeriesCollection[12].Values = new ChartValues<double> { TableResult[12].Value };
            SeriesCollection[13].Values = new ChartValues<double> { TableResult[13].Value };


            Pie.Series[0] = new PieSeries { Title = "CHP", Values = new ChartValues<double> { TableResult[0].Value } };
            Pie.Series[1] = new PieSeries { Title = "CPP", Values = new ChartValues<double> { TableResult[1].Value } };
            Pie.Series[2] = new PieSeries { Title = "RUP", Values = new ChartValues<double> { TableResult[2].Value } };
            Pie.Series[3] = new PieSeries { Title = "MMT", Values = new ChartValues<double> { TableResult[3].Value } };
            Pie.Series[4] = new PieSeries { Title = "MBQ", Values = new ChartValues<double> { TableResult[4].Value } };
            Pie.Series[5] = new PieSeries { Title = "SCT", Values = new ChartValues<double> { TableResult[5].Value } };
            Pie.Series[6] = new PieSeries { Title = "SDT", Values = new ChartValues<double> { TableResult[6].Value } };
            Pie.Series[7] = new PieSeries { Title = "SCC", Values = new ChartValues<double> { TableResult[7].Value } };
            Pie.Series[8] = new PieSeries { Title = "SQC", Values = new ChartValues<double> { TableResult[8].Value } };
            Pie.Series[9] = new PieSeries { Title = "CPT", Values = new ChartValues<double> { TableResult[9].Value } };
            Pie.Series[10] = new PieSeries { Title = "CCC", Values = new ChartValues<double> { TableResult[10].Value } };
            Pie.Series[11] = new PieSeries { Title = "FP", Values = new ChartValues<double> { TableResult[11].Value } };
            Pie.Series[12] = new PieSeries { Title = "LC", Values = new ChartValues<double> { TableResult[12].Value } };
            Pie.Series[13] = new PieSeries { Title = "DP", Values = new ChartValues<double> { TableResult[13].Value } };
        }


        private void ChartPie_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;
            

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        // < AnalyzeOfAllReceiveValue_OfMetrics > - функція, що будує аналіз отриманих значень метрик
        private string AnalyzeOfAllReceiveValue_OfMetrics ()
        {
            string textOfresults = " ";
            textOfresults += " Чим вища зв'язність модуля, тим кращим є результат проектування. ";
            if ((TableResult[0].Value == 0) || (TableResult[0].Value == 1) || (TableResult[0].Value == 3))
            {
                textOfresults += "Оскільки, значення метрики зв'язності рівне " + TableResult[0].Value + ", це свідчить про результат невірного планування архітектури досліджуваного програмного забезпечення. ";
            }
            if (TableResult[0].Value == 5)
            {
                textOfresults += "Оскільки, значення метрики зв'язності рівне " + TableResult[0].Value + ", це свідчить про результат недбалого планування архітектури досліджуваного програмного забезпечення. ";
            }
            if ((TableResult[0].Value == 7) || (TableResult[0].Value == 9))
            {
                textOfresults += "Оскільки, значення метрики зв'язності рівне " + TableResult[0].Value + ", це свідчить про високий результат планування архітектури досліджуваного програмного забезпечення. ";
            }
            if (TableResult[0].Value == 10)
            {
                textOfresults += "Оскільки, значення метрики зв'язності рівне " + TableResult[0].Value + ", це свідчить про найкращий результат планування архітектури досліджуваного програмного забезпечення. ";
            }

            textOfresults += "Метрика зчеплення є зовнішньою характеристикою модуля, яку бажано і слід зменшувати. ";
            if ((TableResult[1].Value == 9) || (TableResult[1].Value == 7) || (TableResult[1].Value == 5))
            {
                textOfresults += "За обчисленнями, значення метрики зчеплення рівне " + TableResult[1].Value + ", що вказує на можливу наявність проблем та подальшу складність в майбутній реалізації цього ПЗ. ";
            }
            if ((TableResult[1].Value == 4) || (TableResult[1].Value == 3) || (TableResult[1].Value == 1))
            {
                textOfresults += "За обчисленнями, значення метрики зчеплення рівне " + TableResult[1].Value + ", що вказує на успішність і легкість в майбутній реалізації цього ПЗ. ";
            }

            textOfresults += "Метрика звернення до глобальних змінних є наближеною ймовірністю посилання довільного модуля на довільну глобальну змінну. ";
            if ((TableResult[2].Value >= 0) && (TableResult[2].Value < 0.5))
            {
                textOfresults += "Значення цієї ймовірності рівне " + TableResult[2].Value + ", що є задовільною. Оскільки ймовірність не є високою, тому є нижча ймовірність 'несанкційної' зміни певної глобальної змінної, що в результаті не призведе до ускладнення модифікації програми. ";
            }
            if ((TableResult[2].Value >= 0.5) && (TableResult[2].Value <= 1))
            {
                textOfresults += "Значення цієї ймовірності рівне " + TableResult[2].Value + ", що є незадовільною. Оскільки чим вища така ймовірність, тим вище ймовірність 'несанкційної' зміни певної глобальної змінної, що призводить до ускладнення модифікації програми. ";
            }

            textOfresults += "Очікувана вартість розроблення ПЗ за обчисленнями становить " + TableResult[7].Value + ". ";
            textOfresults += "Прогнозована продуктивність розроблення ПЗ є " + TableResult[9].Value + " хвилин на один рядок коду. ";
            textOfresults += "За прогнозами необхідно " + TableResult[10].Value + " грн. витрат на розроблення всієї функціональності досліджуваного ПЗ. ";
            textOfresults += "Функційний розмір використовується як відносна метрика для порівняння з попередніми проектами. За його допомогою можна обчислити кількість рядків коду, що дозволяє визначити загальну трудомісткість та терміни проекту. За вхідними даними, суть можливостей майбутньої програми становить " + TableResult[11].Value + ". ";
            textOfresults += "Модель Боема прогнозує " + TableResult[12].Value + " людиномісяців на оцінку трудовитрат та " + TableResult[13].Value + " місяців займає прогнозована тривалість проекту. ";
            textOfresults += "\n\n   Штучна нейронна мережа здійснює апрокисмацію метрик ПЗ етапу проектування та надає кількісну оцінку складності та якості проекту та значення прогнозу характеристик складності та якості розроблюваного ПЗ. Вхідними даними для ШНМ є множина метрик етапу проектування з точним значенням ТМР={-1, -1, -1, -1, "+ TableResult[0].Value + ", "+ TableResult[1].Value + ", "+ TableResult[2].Value + ", "+ TableResult[3].Value + ", "+ TableResult[4].Value + "} та множина метрик етапу проектування з прогнозованим значенням PMP= {-1, -1, -1, -1, -1, -1, "+ TableResult[5].Value + ", "+ TableResult[6].Value + ", "+ TableResult[7].Value + ", "+ TableResult[8].Value + ", "+ TableResult[9].Value + ", "+ TableResult[10].Value + ", "+ TableResult[11].Value + ", "+ TableResult[12].Value + ", "+ TableResult[13].Value +"}. Якщо певна метрика не визначалась (в нашому випадку всі метрики складності), то відповідний елемент множини дорівнюватиме -1.";

            return textOfresults;
        }

        // < Construct_SHNM > - функція, що знаходить множину для ШНМ
        private string Construct_SHNM()
        {
            string list_SHNM = "";
            list_SHNM = "ТМР={-1, -1, -1, -1, " + TableResult[0].Value + ", " + TableResult[1].Value + ", " + TableResult[2].Value + ", " + TableResult[3].Value + ", " + TableResult[4].Value + "} \n PMP= {-1, -1, -1, -1, -1, -1, " + TableResult[5].Value + ", " + TableResult[6].Value + ", " + TableResult[7].Value + ", " + TableResult[8].Value + ", " + TableResult[9].Value + ", " + TableResult[10].Value + ", " + TableResult[11].Value + ", " + TableResult[12].Value + ", " + TableResult[13].Value + "}.";
            return list_SHNM;
        }
        
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------


            // < BackToFindMetric_btn_Click > - функція обробник кнопки "НАЗАД"
            private void BackToFindMetric_btn_Click(object sender, RoutedEventArgs e)
        {
            AllMetrics_TabControl.Visibility = Visibility.Visible;
            AnalyzeOfAllResults_ScrollViewerForThisTab.Visibility = Visibility.Hidden;
            AnalyzeOfAllResults_Grid.Visibility = Visibility.Hidden;
        }


        //------------------------------------------------------------------------------------------------
        // ОСНОВНИЙ СПИСОК ФУНКЦІЙ ДЛЯ МЕНЮ ТА ПАНЕЛІ ІНСТРУМЕНТІВ (СПІЛЬНІ ФУНКЦІЇ)
        //------------------------------------------------------------------------------------------------
        // < Clean_allParameters_Click > - очищує всі задані параметри
        private void Clean_allParameters_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви дійсно бажаєте очистити всі дані?", "Видалення:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                TimeForDoIt_ProgressBar(15);
                //CHP
                Value_1_CHP_rb.IsChecked = false;
                Value_2_CHP_rb.IsChecked = false;
                Value_3_CHP_rb.IsChecked = false;
                Value_4_CHP_rb.IsChecked = false;
                Value_5_CHP_rb.IsChecked = false;
                Value_6_CHP_rb.IsChecked = false;
                Value_7_CHP_rb.IsChecked = false;
                ResultAnalysisMetric_CHP_lb.Content = 0;
                CalculatInformationAboutThisMetric_CHP_TextBox.Text = "";
                CalculationForMetric_CHP_btn.Visibility = Visibility.Hidden;
                CalculateMetric_CHP_btn.Visibility = Visibility.Hidden;
                //CPP
                Value_1_CPP_rb.IsChecked = false;
                Value_2_CPP_rb.IsChecked = false;
                Value_3_CPP_rb.IsChecked = false;
                Value_4_CPP_rb.IsChecked = false;
                Value_5_CPP_rb.IsChecked = false;
                Value_6_CPP_rb.IsChecked = false;
                ResultAnalysisMetric_CPP_lb.Content = 0;
                CalculatInformationAboutThisMetric_CPP_TextBox.Text = "";
                CalculationForMetric_CPP_btn.Visibility = Visibility.Hidden;
                CalculateMetric_CPP_btn.Visibility = Visibility.Hidden;
                //RUP
                rupMetric.ClearAllParameters_OfMetric();
                TableInfoParametrs_RUP_dg.Items.Refresh();
                ResultAnalysisMetric_RUP_lb.Content = 0;
                CalculatInformationAboutThisMetric_RUP_TextBox.Text = "";
                CalculationForMetric_RUP_btn.Visibility = Visibility.Hidden;
                CalculateMetric_RUP_btn.Visibility = Visibility.Hidden;
                //MMT
                mmtMetric.ClearAllParameters_OfMetric();
                TableInfoParametrs_MMT_dg.Items.Refresh();
                ResultAnalysisMetric_MMT_lb.Content = 0;
                CalculatInformationAboutThisMetric_MMT_TextBox.Text = "";
                CalculationForMetric_MMT_btn.Visibility = Visibility.Hidden;
                CalculateMetric_MMT_btn.Visibility = Visibility.Hidden;
                //MBQ
                mbqMetric.ClearAllParameters_OfMetric();
                TableInfoParametrs_MBQ_dg.Items.Refresh();
                ResultAnalysisMetric_MBQ_lb.Content = 0;
                CalculatInformationAboutThisMetric_MBQ_TextBox.Text = "";
                CalculationForMetric_MBQ_btn.Visibility = Visibility.Hidden;
                CalculateMetric_MBQ_btn.Visibility = Visibility.Hidden;
                //SCT
                sctMetric.ClearAllParameters_OfMetric();
                TableInfoParametrs_SCT_dg.Items.Refresh();
                ResultAnalysisMetric_SCT_lb.Content = 0;
                CalculatInformationAboutThisMetric_SCT_TextBox.Text = "";
                CalculationForMetric_SCT_btn.Visibility = Visibility.Hidden;
                CalculateMetric_SCT_btn.Visibility = Visibility.Hidden;
                //SDT
                sdtMetric.ClearAllParameters_OfMetric();
                TableInfoParametrs_SDT_dg.Items.Refresh();
                ResultAnalysisMetric_SDT_lb.Content = 0;
                CalculatInformationAboutThisMetric_SDT_TextBox.Text = "";
                CalculationForMetric_SDT_btn.Visibility = Visibility.Hidden;
                CalculateMetric_SDT_btn.Visibility = Visibility.Hidden;
                //SCC
                countOfFunction_SCC = 1;
                sccMetric.SetAllParametersWithDefaultValue_OfMetric();
                CountOfFunctions_SCC_label.Visibility = Visibility;
                CountOfFunctions_SCC_tb.Visibility = Visibility;
                SetCountOfFunctions_SCC_button.Visibility = Visibility;
                ImageResult_OfMetrica_SCC.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_SCC_lb.Visibility = Visibility.Hidden;
                InfoForStepSetOfFunctions_SCC_label.Visibility = Visibility.Hidden;
                InfoAboutParameters1_SCC_label.Visibility = Visibility.Hidden;
                InfoAboutParameters2_SCC_label.Visibility = Visibility.Hidden;
                InfoAboutFind1_OfLOC_SCC_label.Visibility = Visibility.Hidden;
                InfoAboutFind2_OfLOC_SCC_label.Visibility = Visibility.Hidden;
                InfoAboutFind_OfLOC_one_SCC_label.Visibility = Visibility.Hidden;
                InfoFinish_SCC_label.Visibility = Visibility.Hidden;
                One_SCC_label.Visibility = Visibility.Hidden;
                LOC_better_SCC_label.Visibility = Visibility.Hidden;
                LOC_worse_SCC_label.Visibility = Visibility.Hidden;
                LOC_probable_SCC_label.Visibility = Visibility.Hidden;
                DisplayResult1_SCC_lb.Visibility = Visibility.Hidden;
                DisplayResult2_SCC_lb.Visibility = Visibility.Hidden;
                CalculationForMetric_SCC_btn.Visibility = Visibility.Hidden;
                NewParametrs_SCC_btn.Visibility = Visibility.Hidden;
                CleanParametrs_SCC_btn.Visibility = Visibility.Hidden;
                FindFormula_OfLOC_SCC_btn.Visibility = Visibility.Hidden;
                GO_Next_SCC_btn.Visibility = Visibility.Hidden;
                READY_SCC_btn.Visibility = Visibility.Hidden;
                FindLOC_SCC_btn.Visibility = Visibility.Hidden;
                InputTableInfoParameters_SCC_dg.Visibility = Visibility.Hidden;
                OutputTableInfoParameters_SCC_dg.Visibility = Visibility.Hidden;
                InformationAboutThisMetric_SCC_TextBox.Text = "";
                CalculatInformationAboutThisMetric_SCC_TextBox.Text = "";
                CountOfFunctions_SCC_tb.Text = "0";
                ResultAnalysisMetric_SCC_lb.Content = 0;
                for (int i = 0; i < InputTableInfoParameters_SCC_dg.Items.Count; i++)
                {
                    sccMetric.ChangeValue_OfParameter(i, 0);
                }
                InputTableInfoParameters_SCC_dg.Items.Refresh();
                OutputTableInfoParameters_SCC_dg.Items.Clear();
                OutputTableInfoParameters_SCC_dg.Items.Refresh();
                DisplayResult2_SCC_lb.Content = "        всього ПЗ становить: ";
                CalculatInformationAboutThisMetric_SCC_TextBox.Text = "";
                CalculationForMetric_SCC_btn.Visibility = Visibility.Hidden;
                //SQC
                sqcMetric.ClearAllParameters_OfMetric();
                TableInfoParametrs_SQC_dg.Items.Refresh();
                ResultAnalysisMetric_SQC_lb.Content = 0;
                CalculatInformationAboutThisMetric_SQC_TextBox.Text = "";
                CalculationForMetric_SQC_btn.Visibility = Visibility.Hidden;
                CalculateMetric_SQC_btn.Visibility = Visibility.Hidden;
                //CPT
                cccMetric.SetAllParametersWithDefaultValue_OfMetric();
                countOfFunction_CPT = 1;
                CountOfFunctions_CPT_label.Visibility = Visibility;
                CountOfFunctions_CPT_tb.Visibility = Visibility;
                SetCountOfFunctions_CPT_button.Visibility = Visibility;
                ImageResult_OfMetrica_CPT.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_CPT_lb.Visibility = Visibility.Hidden;
                InfoForStepSetOfFunctions_CPT_label.Visibility = Visibility.Hidden;
                InfoAboutFind1_OfLOC_CPT_label.Visibility = Visibility.Hidden;
                InfoAboutFind2_OfLOC_CPT_label.Visibility = Visibility.Hidden;
                InfoAboutFind_OfLOC_one_CPT_label.Visibility = Visibility.Hidden;
                InfoFinish_CPT_label.Visibility = Visibility.Hidden;
                One_CPT_label.Visibility = Visibility.Hidden;
                LOC_better_CPT_label.Visibility = Visibility.Hidden;
                LOC_worse_CPT_label.Visibility = Visibility.Hidden;
                LOC_probable_CPT_label.Visibility = Visibility.Hidden;
                DisplayResult1_CPT_lb.Visibility = Visibility.Hidden;
                DisplayResult2_CPT_lb.Visibility = Visibility.Hidden;
                CalculationForMetric_CPT_btn.Visibility = Visibility.Hidden;
                NewParametrs_CPT_btn.Visibility = Visibility.Hidden;
                CleanParametrs_CPT_btn.Visibility = Visibility.Hidden;
                FindFormula_OfLOC_CPT_btn.Visibility = Visibility.Hidden;
                GO_Next_CPT_btn.Visibility = Visibility.Hidden;
                READY_CPT_btn.Visibility = Visibility.Hidden;
                FindLOC_CPT_btn.Visibility = Visibility.Hidden;
                InputTableInfoParameters_CPT_dg.Visibility = Visibility.Hidden;
                OutputTableInfoParameters_CPT_dg.Visibility = Visibility.Hidden;
                InformationAboutThisMetric_CPT_TextBox.Text = "";
                CalculatInformationAboutThisMetric_CPT_TextBox.Text = "";
                CountOfFunctions_CPT_tb.Text = "0";
                ResultAnalysisMetric_CPT_lb.Content = 0;
                for (int i = 0; i < InputTableInfoParameters_CPT_dg.Items.Count; i++)
                {
                    cptMetric.ChangeValue_OfParameter(i, 0);
                }
                InputTableInfoParameters_CPT_dg.Items.Refresh();
                OutputTableInfoParameters_CPT_dg.Items.Clear();
                OutputTableInfoParameters_CPT_dg.Items.Refresh();
                DisplayResult2_CPT_lb.Content = "        всього ПЗ становить: ";
                CalculatInformationAboutThisMetric_CPT_TextBox.Text = "";
                CalculationForMetric_CPT_btn.Visibility = Visibility.Hidden;
                //CCC
                countOfFunction_CCC = 1;
                CountOfFunctions_CCC_label.Visibility = Visibility;
                CountOfFunctions_CCC_tb.Visibility = Visibility;
                SetCountOfFunctions_CCC_button.Visibility = Visibility;
                UseProductivity_CCC_chbx.Visibility = Visibility;
                UseProductivity_CCC_chbx.IsChecked = false;
                UseProductivity_CCC_lbl.Visibility = Visibility;
                ImageResult_OfMetrica_CCC.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_CCC_lb.Visibility = Visibility.Hidden;
                InfoForStepSetOfFunctions_CCC_label.Visibility = Visibility.Hidden;
                InfoAboutFind1_OfLOC_CCC_label.Visibility = Visibility.Hidden;
                InfoAboutFind2_OfLOC_CCC_label.Visibility = Visibility.Hidden;
                InfoAboutFind_OfLOC_one_CCC_label.Visibility = Visibility.Hidden;
                InfoFinish_CCC_label.Visibility = Visibility.Hidden;
                One_CCC_label.Visibility = Visibility.Hidden;
                LOC_better_CCC_label.Visibility = Visibility.Hidden;
                LOC_worse_CCC_label.Visibility = Visibility.Hidden;
                LOC_probable_CCC_label.Visibility = Visibility.Hidden;
                DisplayResult1_CCC_lb.Visibility = Visibility.Hidden;
                DisplayResult2_CCC_lb.Visibility = Visibility.Hidden;
                CalculationForMetric_CCC_btn.Visibility = Visibility.Hidden;
                NewParametrs_CCC_btn.Visibility = Visibility.Hidden;
                CleanParametrs_CCC_btn.Visibility = Visibility.Hidden;
                FindFormula_OfLOC_CCC_btn.Visibility = Visibility.Hidden;
                GO_Next_CCC_btn.Visibility = Visibility.Hidden;
                READY_CCC_btn.Visibility = Visibility.Hidden;
                FindLOC_CCC_btn.Visibility = Visibility.Hidden;
                InputTableInfoParameters_CCC_dg.Visibility = Visibility.Hidden;
                OutputTableInfoParameters_CCC_dg.Visibility = Visibility.Hidden;
                InformationAboutThisMetric_CCC_TextBox.Text = "";
                CalculatInformationAboutThisMetric_CCC_TextBox.Text = "";
                CountOfFunctions_CCC_tb.Text = "0";
                ResultAnalysisMetric_CCC_lb.Content = 0;
                for (int i = 0; i < InputTableInfoParameters_CCC_dg.Items.Count; i++)
                {
                    cccMetric.ChangeValue_OfParameter(i, 0);
                }
                InputTableInfoParameters_CCC_dg.Items.Refresh();
                OutputTableInfoParameters_CCC_dg.Items.Clear();
                OutputTableInfoParameters_CCC_dg.Items.Refresh();
                DisplayResult2_CCC_lb.Content = "        всього програмного коду становить: ";
                //FP
                countOfFunction_FP = 1;
                CountOfFunctions_FP_label.Visibility = Visibility;
                CountOfFunctions_FP_tb.Visibility = Visibility;
                SetCountOfFunctions_FP_button.Visibility = Visibility;
                NoneSpecialReq_FP_chbx.IsChecked = false;
                ExistSpecialReq_FP_chbx.IsChecked = false;
                ImageResult_OfMetrica_FP.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_FP_lb.Visibility = Visibility.Hidden;
                InfoForStepSetOfFunctions_FP_label.Visibility = Visibility.Hidden;
                DisplayResult_FP_lb.Visibility = Visibility.Hidden;
                CalculationForMetric_FP_btn.Visibility = Visibility.Hidden;
                NewParametrs_FP_btn.Visibility = Visibility.Hidden;
                CleanParametrs_FP_btn.Visibility = Visibility.Hidden;
                GO_Next_FP_btn.Visibility = Visibility.Hidden;
                READY_FP_btn.Visibility = Visibility.Hidden;
                InputTableInfoParameters_FP_dg.Visibility = Visibility.Hidden;
                OutputTableInfoParameters_FP_dg.Visibility = Visibility.Hidden;
                InformationAboutThisMetric_FP_TextBox.Text = "";
                CalculatInformationAboutThisMetric_FP_TextBox.Text = "";
                CountOfFunctions_FP_tb.Text = "0";
                ResultAnalysisMetric_FP_lb.Content = 0;
                for (int i = 0; i < InputTableInfoParameters_FP_dg.Items.Count; i++)
                {
                    fpMetric.ChangeValue_OfParameter(i, 0);
                }
                InputTableInfoParameters_FP_dg.Items.Refresh();
                OutputTableInfoParameters_FP_dg.Items.Clear();
                OutputTableInfoParameters_FP_dg.Items.Refresh();
                CalculatInformationAboutThisMetric_FP_TextBox.Text = "";
                CalculationForMetric_FP_btn.Visibility = Visibility.Hidden;
                //LC
                lcMetric.SetAllParametersWithDefaultValue_OfMetric();
                TableInfoParametrs_LC_dg.Items.Refresh();
                ResultAnalysisMetric_LC_lb.Content = 0;
                TableInfoParametrs_LC_dg.Items.Refresh();
                SetKoefCOCOMO_LC_button.Visibility = Visibility;
                KoefCOCOMO_LC_label.Visibility = Visibility;
                ListKoef1_cmbx.Visibility = Visibility;
                ListKoef2_cmbx.Visibility = Visibility;
                ListKoef3_cmbx.Visibility = Visibility;
                ForIndependent_LC_chbx.Visibility = Visibility;
                ForIndependent_LC_chbx.IsChecked = false;
                ForEmbedded_LC_chbx.Visibility = Visibility;
                ForEmbedded_LC_chbx.IsChecked = false;
                ForIntermediate_LC_chbx.Visibility = Visibility;
                ForIntermediate_LC_chbx.IsChecked = false;
                TableInfoParametrs_LC_dg.Visibility = Visibility.Hidden;
                CalculateMetric_LC_btn.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_LC_image.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_LC_lb.Visibility = Visibility.Hidden;
                CleanParametrs_LC_btn.Visibility = Visibility.Hidden;
                RefreshParametrs_LC_btn.Visibility = Visibility.Hidden;
                NewParametrs_LC_btn.Visibility = Visibility.Hidden;
                CalculationForMetric_LC_btn.Visibility = Visibility.Hidden;
                lcMetric.ClearAllParameters_OfMetric();
                CalculatInformationAboutThisMetric_LC_TextBox.Text = "";
                //DP
                dpMetric.SetAllParametersWithDefaultValue_OfMetric();
                TableInfoParametrs_DP_dg.Items.Refresh();
                ResultAnalysisMetric_DP_lb.Content = 0;
                TableInfoParametrs_DP_dg.Items.Refresh();
                SetKoefCOCOMO_DP_button.Visibility = Visibility;
                KoefCOCOMO_DP_label.Visibility = Visibility;
                ListKoef1_DP_cmbx.Visibility = Visibility;
                ListKoef2_DP_cmbx.Visibility = Visibility;
                ListKoef3_DP_cmbx.Visibility = Visibility;
                ForIndependent_DP_chbx.Visibility = Visibility;
                ForIndependent_DP_chbx.IsChecked = false;
                ForEmbedded_DP_chbx.Visibility = Visibility;
                ForEmbedded_DP_chbx.IsChecked = false;
                ForIntermediate_DP_chbx.Visibility = Visibility;
                ForIntermediate_DP_chbx.IsChecked = false;
                TableInfoParametrs_DP_dg.Visibility = Visibility.Hidden;
                CalculateMetric_DP_btn.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_DP_image.Visibility = Visibility.Hidden;
                ResultAnalysisMetric_DP_lb.Visibility = Visibility.Hidden;
                CleanParametrs_DP_btn.Visibility = Visibility.Hidden;
                RefreshParametrs_DP_btn.Visibility = Visibility.Hidden;
                NewParametrs_DP_btn.Visibility = Visibility.Hidden;
                CalculationForMetric_DP_btn.Visibility = Visibility.Hidden;
                dpMetric.ClearAllParameters_OfMetric();
                CalculatInformationAboutThisMetric_DP_TextBox.Text = "";
                Result_OfAllMetric_TextBox.Text = "№   Метрика                                                                Результат              Час виконання";

                MessageBox.Show("Значення параметрів всіх метрик успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                Information_UserDo.Text = "Очищення значень параметрів всіх метрик";
                Information_ProgramDo.Text = "Готово";
                Information_UserDo.Text = "Значення параметрів всіх метрик успішно очищено";
                Information_ProgramDo.Text = "Готово";

                Image img = new Image();
                img.Source = new BitmapImage(new Uri("Images/IconMenu/icon_clean_full.png", UriKind.RelativeOrAbsolute));
                Clean_tb_button.Content = img;
            }
            else
            { }
        }

        // < Save_all_Click > - зберігає необхідну інформацію (таблицю метрик та їх значень)
        private void Save_all_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Result_OfAllMetric_TextBox.Text == "№   Метрика                                                                Результат              Час виконання")
                {
                    MessageBox.Show("Немає данних для збереження!", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Metrics (*.txt)|*.txt";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (StreamWriter saveInFile = new StreamWriter(saveFileDialog.OpenFile(), System.Text.Encoding.Default))
                        {
                            string txt = Result_OfAllMetric_TextBox.Text;
                            string[] listOfMetrics = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < listOfMetrics.Count(); i++)
                            {
                                saveInFile.WriteLine(listOfMetrics[i]);
                            }
                            saveInFile.Close();
                            MessageBox.Show("Інформація знайдених метрик успішно збережена у файл!", "Збереження:", MessageBoxButton.OK, MessageBoxImage.Information);
                            Information_UserDo.Text = "Успішно збережено дані у файл!";
                            Information_ProgramDo.Text = "Готово";
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show("Неможливо зберегти дані у файл!", "Помилка:"+ exception, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // < Exit_Click > - функція виходу з програми
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви дійсно бажаєте завершити роботу?", "Вихід:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else
            { }
        }

        // < Settings_Click > - функція налаштування програми
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Налаштування програми...";
            Information_ProgramDo.Text = "Очікування ...";
            WindowOfSettings windowSettings = new WindowOfSettings();
            windowSettings.Show();
        }

        // < Help_Click > - функція допомоги користувачу
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"D:\DIPLOM\MetricAnalyzerSoftwareQuality\MetricAnalyzerSoftwareQuality\Інструкція користувача MASQ (Metric Analyzer Software Quality).pdf");
            }
            catch
            {
                MessageBox.Show("Пошкоджено шлях до файлу інстукції користувача або файлу не існує!\n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // < View_allParameters_Click > - функція перегляду всіх параметрів
        private void View_allParameters_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Процес перегляду усіх параметрів множини метрик якості програмного забезпечення...";
            Information_ProgramDo.Text = "Очікування ...";
            ViewWindowOfParameter viewParameters = new ViewWindowOfParameter();
            viewParameters.Show();
        }

        // < View_allMetrics_Click > - функція перегляду всіх метрик
        private void View_allMetrics_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Процес перегляду множини метрик якості програмного забезпечення...";
            Information_ProgramDo.Text = "Очікування ...";
            ViewWindowOfMetric viewMetrics = new ViewWindowOfMetric();
            viewMetrics.Show();
        }

        // < Refresh_allParameters_Click > - функція оновлення всіх параметрів
        private void Refresh_allParameters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TableInfoParametrs_CHP_dg.Items.Refresh();
                TableInfoParametrs_CPP_dg.Items.Refresh();
                TableInfoParametrs_RUP_dg.Items.Refresh();
                TableInfoParametrs_MMT_dg.Items.Refresh();
                TableInfoParametrs_MBQ_dg.Items.Refresh();
                TableInfoParametrs_SCT_dg.Items.Refresh();
                TableInfoParametrs_SDT_dg.Items.Refresh();
                InputTableInfoParameters_SCC_dg.Items.Refresh();
                TableInfoParametrs_SQC_dg.Items.Refresh();
                InputTableInfoParameters_CPT_dg.Items.Refresh();
                InputTableInfoParameters_CCC_dg.Items.Refresh();
                InputTableInfoParameters_FP_dg.Items.Refresh();
                TableInfoParametrs_LC_dg.Items.Refresh();
                TableInfoParametrs_DP_dg.Items.Refresh();
                MessageBox.Show("Дані успішно оновлено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                TimeForDoIt_ProgressBar(10);
                Information_UserDo.Text = "Успішне завершення оновлення всіх вхідних даних метрик";
                Information_ProgramDo.Text = "Готово";
            }
            catch
            {
                MessageBox.Show("Неможливо оновити параметри, оскільки відбувається незавершене редагування таблиці! Завершіть редагування та повторіть спробу! \n ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // < New_forStart_Click > - функція обробник, викликає нову форму для заповнення даними спільних параметрів всіх метрик
        private void New_forStart_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Заповнення значеннями спільних вхідних параметрів метрик ...";
            Information_ProgramDo.Text = "Очікування ...";
            AddAllParametrsWindow newAllParametrs = new AddAllParametrsWindow();
            newAllParametrs.Show();
        }


        //------------------------------------------------------------------------------------------------
        // ФУНКЦІЇ ВИВОДУ ІНФОРМАЦІЇ ПРО ПЕВНУ МЕТРИКУ ТА ЗГОРТАННЯ ЦІЄЇ ІНФОРМАЦІЇ
        //------------------------------------------------------------------------------------------------
        // < ShowInformationForThisMetric_Click > - функція виводу інформації про метрику в GroupBox 
        private void ShowInformationForThisMetric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Вивід інформації метрики...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        InformationAboutThisMetric_CHP_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_CHP_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_CHP_GroupBox.Height = 100;
                        InformationAboutThisMetric_CHP_TextBox.Height = 100;
                        gridForTabArea_CHP.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_CHP_TextBox.Text = chpMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_CHP_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        InformationAboutThisMetric_CPP_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_CPP_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_CPP_GroupBox.Height = 100;
                        InformationAboutThisMetric_CPP_TextBox.Height = 100;
                        gridForTabArea_CPP.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_CPP_TextBox.Text = cppMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_CPP_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        InformationAboutThisMetric_RUP_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_RUP_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_RUP_GroupBox.Height = 100;
                        InformationAboutThisMetric_RUP_TextBox.Height = 100;
                        gridForTabArea_RUP.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_RUP_TextBox.Text = rupMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_RUP_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        InformationAboutThisMetric_MMT_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_MMT_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_MMT_GroupBox.Height = 100;
                        InformationAboutThisMetric_MMT_TextBox.Height = 100;
                        gridForTabArea_MMT.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_MMT_TextBox.Text = mmtMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_MMT_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        InformationAboutThisMetric_MBQ_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_MBQ_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_MBQ_GroupBox.Height = 110;
                        InformationAboutThisMetric_MBQ_TextBox.Height = 110;
                        gridForTabArea_MBQ.RowDefinitions[1].Height = new GridLength(128);
                        InformationAboutThisMetric_MBQ_TextBox.Text = mbqMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_MBQ_ButtonHide.Margin = new Thickness(8, 112, 20, 0);
                    } break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        InformationAboutThisMetric_SCT_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_SCT_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_SCT_GroupBox.Height = 90;
                        InformationAboutThisMetric_SCT_TextBox.Height = 90;
                        gridForTabArea_SCT.RowDefinitions[1].Height = new GridLength(115);
                        InformationAboutThisMetric_SCT_TextBox.Text = sctMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_SCT_ButtonHide.Margin = new Thickness(8, 93, 20, 0);
                    } break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        InformationAboutThisMetric_SDT_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_SDT_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_SDT_GroupBox.Height = 90;
                        InformationAboutThisMetric_SDT_TextBox.Height = 90;
                        gridForTabArea_SDT.RowDefinitions[1].Height = new GridLength(115);
                        InformationAboutThisMetric_SDT_TextBox.Text = sdtMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_SDT_ButtonHide.Margin = new Thickness(8, 93, 20, 0);
                    } break;
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        InformationAboutThisMetric_SCC_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_SCC_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_SCC_GroupBox.Height = 100;
                        InformationAboutThisMetric_SCC_TextBox.Height = 100;
                        gridForTabArea_SCC.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_SCC_TextBox.Text = sccMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_SCC_ButtonHide.Margin = new Thickness(8, 110, 20, 0);
                    } break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        InformationAboutThisMetric_SQC_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_SQC_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_SQC_GroupBox.Height = 90;
                        InformationAboutThisMetric_SQC_TextBox.Height = 90;
                        gridForTabArea_SQC.RowDefinitions[1].Height = new GridLength(115);
                        InformationAboutThisMetric_SQC_TextBox.Text = sqcMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_SQC_ButtonHide.Margin = new Thickness(8, 93, 20, 0);
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        InformationAboutThisMetric_CPT_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_CPT_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_CPT_GroupBox.Height = 100;
                        InformationAboutThisMetric_CPT_TextBox.Height = 100;
                        gridForTabArea_CPT.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_CPT_TextBox.Text = cptMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_CPT_ButtonHide.Margin = new Thickness(8, 110, 20, 0);
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        InformationAboutThisMetric_CCC_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_CCC_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_CCC_GroupBox.Height = 100;
                        InformationAboutThisMetric_CCC_TextBox.Height = 100;
                        gridForTabArea_CCC.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_CCC_TextBox.Text = cccMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_CCC_ButtonHide.Margin = new Thickness(8, 110, 20, 0);
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        InformationAboutThisMetric_FP_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_FP_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_FP_GroupBox.Height = 100;
                        InformationAboutThisMetric_FP_TextBox.Height = 100;
                        gridForTabArea_FP.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_FP_TextBox.Text = fpMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_FP_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        InformationAboutThisMetric_LC_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_LC_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_LC_GroupBox.Height = 100;
                        InformationAboutThisMetric_LC_TextBox.Height = 100;
                        gridForTabArea_LC.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_LC_TextBox.Text = lcMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_LC_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        InformationAboutThisMetric_DP_GroupBox.Visibility = Visibility;
                        InformationAboutThisMetric_DP_ButtonHide.Visibility = Visibility;
                        InformationAboutThisMetric_DP_GroupBox.Height = 100;
                        InformationAboutThisMetric_DP_TextBox.Height = 100;
                        gridForTabArea_DP.RowDefinitions[1].Height = new GridLength(120);
                        InformationAboutThisMetric_DP_TextBox.Text = dpMetric.ShowDescription_OfMetric();
                        InformationAboutThisMetric_DP_ButtonHide.Margin = new Thickness(8, 102, 20, 0);
                    } break;
                default: break;
            }
        }


        // < InformationAboutThisMetric_ButtonHide_Click > - функція згортання інформації про метрику
        private void InformationAboutThisMetric_ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Приховування інформації про метрику ...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        InformationAboutThisMetric_CHP_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_CHP_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_CHP.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        InformationAboutThisMetric_CPP_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_CPP_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_CPP.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        InformationAboutThisMetric_RUP_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_RUP_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_RUP.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        InformationAboutThisMetric_MMT_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_MMT_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_MMT.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        InformationAboutThisMetric_MBQ_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_MBQ_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_MBQ.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        InformationAboutThisMetric_SCT_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_SCT_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_SCT.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        InformationAboutThisMetric_SDT_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_SDT_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_SDT.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        InformationAboutThisMetric_SCC_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_SCC_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_SCC.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        InformationAboutThisMetric_SQC_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_SQC_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_SQC.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        InformationAboutThisMetric_CPT_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_CPT_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_CPT.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        InformationAboutThisMetric_CCC_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_CCC_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_CCC.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        InformationAboutThisMetric_FP_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_FP_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_FP.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        InformationAboutThisMetric_LC_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_LC_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_LC.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        InformationAboutThisMetric_DP_GroupBox.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_DP_ButtonHide.Visibility = Visibility.Hidden;
                        gridForTabArea_DP.RowDefinitions[1].Height = new GridLength(1);
                    } break;
                default: break;
            }
        }


        //------------------------------------------------------------------------------------------------
        // ФУНКЦІЇ ВИВОДУ ОБЧИСЛЮВАЛЬНОЇ ІНФОРМАЦІЇ ПРО ПЕВНУ МЕТРИКУ ТА ЗГОРТАННЯ ЦІЄЇ ІНФОРМАЦІЇ
        //------------------------------------------------------------------------------------------------
        // < ShowCalculationInformationForThisMetric_Click > - функція виводу обчислювальної інформації про метрику в GroupBox 
        private void ShowCalculationInformationForThisMetric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Вивід обчислювальної інформації метрики...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        CalculateMetric_CHP_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CHP_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CHP_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CHP_GroupBox.Height = 135;
                        CalculatInformationAboutThisMetric_CHP_TextBox.Height = 135;
                        CalculatInformationAboutThisMetric_CHP_TextBox.Text = chpMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_CHP_ButtonHide.Margin = new Thickness(80, 137, 10, 10);
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        CalculateMetric_CPP_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CPP_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CPP_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CPP_GroupBox.Height = 135;
                        CalculatInformationAboutThisMetric_CPP_TextBox.Height = 135;
                        CalculatInformationAboutThisMetric_CPP_TextBox.Text = cppMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_CPP_ButtonHide.Margin = new Thickness(80, 137, 10, 10);
                    } break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        CalculateMetric_RUP_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_RUP_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_RUP_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_RUP_GroupBox.Height = 100;
                        CalculatInformationAboutThisMetric_RUP_TextBox.Height = 100;
                        CalculatInformationAboutThisMetric_RUP_TextBox.Text = rupMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_RUP_ButtonHide.Margin = new Thickness(80, 102, 10, 10);
                    } break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        CalculateMetric_MMT_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_MMT_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_MMT_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_MMT_GroupBox.Height = 155;
                        CalculatInformationAboutThisMetric_MMT_TextBox.Height = 155;
                        CalculatInformationAboutThisMetric_MMT_TextBox.Text = mmtMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_MMT_ButtonHide.Margin = new Thickness(80, 162, 10, 10);
                    } break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        CalculateMetric_MBQ_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_MBQ_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_MBQ_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_MBQ_GroupBox.Height = 155;
                        CalculatInformationAboutThisMetric_MBQ_TextBox.Height = 155;
                        CalculatInformationAboutThisMetric_MBQ_TextBox.Text = mbqMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_MBQ_ButtonHide.Margin = new Thickness(80, 162, 10, 10);
                    } break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        CalculateMetric_SCT_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SCT_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SCT_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SCT_GroupBox.Height = 155;
                        CalculatInformationAboutThisMetric_SCT_TextBox.Height = 155;
                        CalculatInformationAboutThisMetric_SCT_TextBox.Text = sctMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_SCT_ButtonHide.Margin = new Thickness(80, 162, 10, 10);
                    } break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        CalculateMetric_SDT_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SDT_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SDT_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SDT_GroupBox.Height = 155;
                        CalculatInformationAboutThisMetric_SDT_TextBox.Height = 155;
                        CalculatInformationAboutThisMetric_SDT_TextBox.Text = sdtMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_SDT_ButtonHide.Margin = new Thickness(80, 162, 10, 10);
                    } break;
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        DisplayResult1_SCC_lb.Visibility = Visibility.Hidden;
                        DisplayResult2_SCC_lb.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SCC_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SCC_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SCC_TextBox.Text += sccMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_SCC_ButtonHide.Margin = new Thickness(80, 0, 10, 10);
                        CalculatInformationAboutThisMetric_SCC_GroupBox.Margin = new Thickness(80, 10, 10, 10);
                    } break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        CalculateMetric_SQC_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SQC_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SQC_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_SQC_GroupBox.Height = 175;
                        CalculatInformationAboutThisMetric_SQC_TextBox.Height = 175;
                        CalculatInformationAboutThisMetric_SQC_TextBox.Text = sqcMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_SQC_ButtonHide.Margin = new Thickness(80, 187, 10, 10);
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        DisplayResult1_CPT_lb.Visibility = Visibility.Hidden;
                        DisplayResult2_CPT_lb.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CPT_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CPT_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CPT_TextBox.Text += cptMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_CPT_ButtonHide.Margin = new Thickness(80, 0, 10, 10);
                        CalculatInformationAboutThisMetric_CPT_GroupBox.Margin = new Thickness(80, 10, 10, 10);
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        DisplayResult1_CCC_lb.Visibility = Visibility.Hidden;
                        DisplayResult2_CCC_lb.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CCC_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CCC_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_CCC_TextBox.Text += cccMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_CCC_ButtonHide.Margin = new Thickness(80, 0, 10, 10);
                        CalculatInformationAboutThisMetric_CCC_GroupBox.Margin = new Thickness(80, 10, 10, 10);
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        DisplayResult_FP_lb.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_FP_scrollView.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_FP_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_FP_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_FP_TextBox.Text += fpMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_FP_ButtonHide.Margin = new Thickness(80, 0, 10, 10);
                        CalculatInformationAboutThisMetric_FP_GroupBox.Margin = new Thickness(80, 10, 10, 10);
                    } break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        CalculateMetric_LC_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_LC_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_LC_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_LC_GroupBox.Height = 155;
                        CalculatInformationAboutThisMetric_LC_TextBox.Height = 155;
                        CalculatInformationAboutThisMetric_LC_TextBox.Text = lcMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_LC_ButtonHide.Margin = new Thickness(80, 162, 10, 10);
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        CalculateMetric_DP_btn.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_DP_GroupBox.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_DP_ButtonHide.Visibility = Visibility;
                        CalculatInformationAboutThisMetric_DP_GroupBox.Height = 155;
                        CalculatInformationAboutThisMetric_DP_TextBox.Height = 155;
                        CalculatInformationAboutThisMetric_DP_TextBox.Text = dpMetric.ShowInformationOfParameters_and_SolutionOfMetric();
                        CalculatInformationAboutThisMetric_DP_ButtonHide.Margin = new Thickness(80, 162, 10, 10);
                    } break;
                default: break;
            }
        }


        // < CalculatInformationAboutThisMetric_ButtonHide_Click > - функція згортання обчислювальної інформації про метрику
        private void CalculatInformationAboutThisMetric_ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Приховування обчислювальної інформації метрики...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        CalculatInformationAboutThisMetric_CHP_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CHP_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_CHP_btn.Visibility = Visibility;
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        CalculatInformationAboutThisMetric_CPP_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CPP_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_CPP_btn.Visibility = Visibility;
                    } break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        CalculatInformationAboutThisMetric_RUP_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_RUP_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_RUP_btn.Visibility = Visibility;
                    } break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        CalculatInformationAboutThisMetric_MMT_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_MMT_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_MMT_btn.Visibility = Visibility;
                    } break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        CalculatInformationAboutThisMetric_MBQ_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_MBQ_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_MBQ_btn.Visibility = Visibility;
                    } break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        CalculatInformationAboutThisMetric_SCT_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SCT_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_SCT_btn.Visibility = Visibility;
                    } break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        CalculatInformationAboutThisMetric_SDT_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SDT_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_SDT_btn.Visibility = Visibility;
                    } break;
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        CalculatInformationAboutThisMetric_SCC_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SCC_ButtonHide.Visibility = Visibility.Hidden;
                        DisplayResult1_SCC_lb.Visibility = Visibility;
                        DisplayResult2_SCC_lb.Visibility = Visibility;
                    } break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        CalculatInformationAboutThisMetric_SQC_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_SQC_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_SQC_btn.Visibility = Visibility;
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        CalculatInformationAboutThisMetric_CPT_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CPT_ButtonHide.Visibility = Visibility.Hidden;
                        DisplayResult1_CPT_lb.Visibility = Visibility;
                        DisplayResult2_CPT_lb.Visibility = Visibility;
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        CalculatInformationAboutThisMetric_CCC_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_CCC_ButtonHide.Visibility = Visibility.Hidden;
                        DisplayResult1_CCC_lb.Visibility = Visibility;
                        DisplayResult2_CCC_lb.Visibility = Visibility;
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        CalculatInformationAboutThisMetric_FP_scrollView.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_FP_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_FP_ButtonHide.Visibility = Visibility.Hidden;
                        DisplayResult_FP_lb.Visibility = Visibility;
                    }
                    break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        CalculatInformationAboutThisMetric_LC_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_LC_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_LC_btn.Visibility = Visibility;
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        CalculatInformationAboutThisMetric_DP_GroupBox.Visibility = Visibility.Hidden;
                        CalculatInformationAboutThisMetric_DP_ButtonHide.Visibility = Visibility.Hidden;
                        CalculateMetric_DP_btn.Visibility = Visibility;
                    } break;
                default: break;
            }
        }


        //------------------------------------------------------------------------------------------------
        // ОСНОВНІ КНОПКИ (СПИСОК ФУНКЦІЙ) TabControlItem 
        //------------------------------------------------------------------------------------------------
        // < NewParametrsForThisMetric_Click > - функція задавання параметрів метрики
        private void NewParametrsForThisMetric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Задання нових параметрів для метрики ...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        CalculateMetric_CHP_btn.Visibility = Visibility;
                        RefreshParametrs_CHP_btn.Visibility = Visibility;
                        CleanParametrs_CHP_btn.Visibility = Visibility;
                        TimeForDoIt_ProgressBar(10);
                        Information_UserDo.Text = "Заповнення значеннями вхідних параметрів CHP метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        if ((Value_1_CHP_rb.IsChecked == false) && (Value_2_CHP_rb.IsChecked == false) && (Value_3_CHP_rb.IsChecked == false) && (Value_4_CHP_rb.IsChecked == false) && (Value_5_CHP_rb.IsChecked == false) && (Value_6_CHP_rb.IsChecked == false) && (Value_7_CHP_rb.IsChecked == false))
                        {
                            MessageBox.Show("Не обрано рівня зв'язності! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів CHP метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        CalculateMetric_CPP_btn.Visibility = Visibility;
                        RefreshParametrs_CPP_btn.Visibility = Visibility;
                        CleanParametrs_CPP_btn.Visibility = Visibility;
                        TimeForDoIt_ProgressBar(10);
                        Information_UserDo.Text = "Заповнення значеннями вхідних параметрів CPP метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        if ((Value_1_CPP_rb.IsChecked == false) && (Value_2_CPP_rb.IsChecked == false) && (Value_3_CPP_rb.IsChecked == false) && (Value_4_CPP_rb.IsChecked == false) && (Value_5_CPP_rb.IsChecked == false) && (Value_6_CPP_rb.IsChecked == false))
                        {
                            MessageBox.Show("Не обрано типу зчеплення! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів CPP метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                    } break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        try
                        {
                            CalculateMetric_RUP_btn.Visibility = Visibility;
                            RefreshParametrs_RUP_btn.Visibility = Visibility;
                            CleanParametrs_RUP_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів RUP метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_RUP_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_RUP_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                rupMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів RUP метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        try
                        {
                            CalculateMetric_MMT_btn.Visibility = Visibility;
                            RefreshParametrs_MMT_btn.Visibility = Visibility;
                            CleanParametrs_MMT_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів MMT метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_MMT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_MMT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                mmtMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            var qcl = DataGridHelper.GetCell(TableInfoParametrs_MMT_dg, 0, 1);
                            var cont = qcl.Content as TextBlock;
                            var _qcl = cont.Text;
                            if (Convert.ToInt32(_qcl) > 50000)
                            {
                                MessageBox.Show("Невірні вхідні дані! Кількість рядків коду перевищують допустиму межу (50 000)! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                                TimeForDoIt_ProgressBar(10);
                                Information_UserDo.Text = "Заповнення значеннями вхідних параметрів MMT метрики успішно завершено";
                                Information_ProgramDo.Text = "Готово";
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        try
                        {
                            CalculateMetric_MBQ_btn.Visibility = Visibility;
                            RefreshParametrs_MBQ_btn.Visibility = Visibility;
                            CleanParametrs_MBQ_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів MBQ метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_MBQ_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_MBQ_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                mbqMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів MBQ метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        try
                        {
                            CalculateMetric_SCT_btn.Visibility = Visibility;
                            RefreshParametrs_SCT_btn.Visibility = Visibility;
                            CleanParametrs_SCT_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів SCT метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_SCT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_SCT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sctMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів SCT метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        try
                        {
                            CalculateMetric_SDT_btn.Visibility = Visibility;
                            RefreshParametrs_SDT_btn.Visibility = Visibility;
                            CleanParametrs_SDT_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів SDT метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_SDT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_SDT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sdtMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів SDT метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        TimeForDoIt_ProgressBar(10);
                        countOfFunction_SCC = 1;
                        Information_UserDo.Text = "Підготовлювання робочої зони для SCC метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        CountOfFunctions_SCC_label.Visibility = Visibility;
                        CountOfFunctions_SCC_tb.Visibility = Visibility;
                        SetCountOfFunctions_SCC_button.Visibility = Visibility;
                        ImageResult_OfMetrica_SCC.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_SCC_lb.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutParameters1_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutParameters2_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind1_OfLOC_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind2_OfLOC_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind_OfLOC_one_SCC_label.Visibility = Visibility.Hidden;
                        InfoFinish_SCC_label.Visibility = Visibility.Hidden;
                        One_SCC_label.Visibility = Visibility.Hidden;
                        LOC_better_SCC_label.Visibility = Visibility.Hidden;
                        LOC_worse_SCC_label.Visibility = Visibility.Hidden;
                        LOC_probable_SCC_label.Visibility = Visibility.Hidden;
                        DisplayResult1_SCC_lb.Visibility = Visibility.Hidden;
                        DisplayResult2_SCC_lb.Visibility = Visibility.Hidden;
                        CalculationForMetric_SCC_btn.Visibility = Visibility.Hidden;
                        NewParametrs_SCC_btn.Visibility = Visibility.Hidden;
                        CleanParametrs_SCC_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_SCC_btn.Visibility = Visibility.Hidden;
                        GO_Next_SCC_btn.Visibility = Visibility.Hidden;
                        READY_SCC_btn.Visibility = Visibility.Hidden;
                        FindLOC_SCC_btn.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_SCC_dg.Visibility = Visibility.Hidden;
                        OutputTableInfoParameters_SCC_dg.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_SCC_TextBox.Text = "";
                        CalculatInformationAboutThisMetric_SCC_TextBox.Text = "";
                        CountOfFunctions_SCC_tb.Text = "0";
                        TimeForDoIt_ProgressBar(5);
                        Information_UserDo.Text = "Введіть кількість функцій для подальшого знаходження метрики";
                        Information_ProgramDo.Text = "Готово";
                        ResultAnalysisMetric_SCC_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_SCC_dg.Items.Count; i++)
                        {
                            sccMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_SCC_dg.Items.Refresh();
                        OutputTableInfoParameters_SCC_dg.Items.Clear();
                        OutputTableInfoParameters_SCC_dg.Items.Refresh();
                        DisplayResult2_SCC_lb.Content = "        всього ПЗ становить: ";
                    } break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        try
                        {
                            CalculateMetric_SQC_btn.Visibility = Visibility;
                            RefreshParametrs_SQC_btn.Visibility = Visibility;
                            CleanParametrs_SQC_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів SQC метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_SQC_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_SQC_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sqcMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів SQC метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        TimeForDoIt_ProgressBar(10);
                        countOfFunction_CPT = 1;
                        Information_UserDo.Text = "Підготовлювання робочої зони для CPT метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        CountOfFunctions_CPT_label.Visibility = Visibility;
                        CountOfFunctions_CPT_tb.Visibility = Visibility;
                        SetCountOfFunctions_CPT_button.Visibility = Visibility;
                        ImageResult_OfMetrica_CPT.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_CPT_lb.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_CPT_label.Visibility = Visibility.Hidden;
                        InfoAboutFind1_OfLOC_CPT_label.Visibility = Visibility.Hidden;
                        InfoAboutFind2_OfLOC_CPT_label.Visibility = Visibility.Hidden;
                        InfoAboutFind_OfLOC_one_CPT_label.Visibility = Visibility.Hidden;
                        InfoFinish_CPT_label.Visibility = Visibility.Hidden;
                        One_CPT_label.Visibility = Visibility.Hidden;
                        LOC_better_CPT_label.Visibility = Visibility.Hidden;
                        LOC_worse_CPT_label.Visibility = Visibility.Hidden;
                        LOC_probable_CPT_label.Visibility = Visibility.Hidden;
                        DisplayResult1_CPT_lb.Visibility = Visibility.Hidden;
                        DisplayResult2_CPT_lb.Visibility = Visibility.Hidden;
                        CalculationForMetric_CPT_btn.Visibility = Visibility.Hidden;
                        NewParametrs_CPT_btn.Visibility = Visibility.Hidden;
                        CleanParametrs_CPT_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_CPT_btn.Visibility = Visibility.Hidden;
                        GO_Next_CPT_btn.Visibility = Visibility.Hidden;
                        READY_CPT_btn.Visibility = Visibility.Hidden;
                        FindLOC_CPT_btn.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_CPT_dg.Visibility = Visibility.Hidden;
                        OutputTableInfoParameters_CPT_dg.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_CPT_TextBox.Text = "";
                        CalculatInformationAboutThisMetric_CPT_TextBox.Text = "";
                        CountOfFunctions_CPT_tb.Text = "0";
                        TimeForDoIt_ProgressBar(5);
                        Information_UserDo.Text = "Введіть кількість функцій для подальшого знаходження метрики";
                        Information_ProgramDo.Text = "Готово";
                        ResultAnalysisMetric_CPT_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_CPT_dg.Items.Count; i++)
                        {
                            cptMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_CPT_dg.Items.Refresh();
                        OutputTableInfoParameters_CPT_dg.Items.Clear();
                        OutputTableInfoParameters_CPT_dg.Items.Refresh();
                        DisplayResult2_CPT_lb.Content = "        всього ПЗ становить: ";
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        TimeForDoIt_ProgressBar(10);
                        countOfFunction_CCC = 1;
                        Information_UserDo.Text = "Підготовлювання робочої зони для CCC метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        CountOfFunctions_CCC_label.Visibility = Visibility;
                        CountOfFunctions_CCC_tb.Visibility = Visibility;
                        SetCountOfFunctions_CCC_button.Visibility = Visibility;
                        UseProductivity_CCC_chbx.Visibility = Visibility;
                        UseProductivity_CCC_chbx.IsChecked = false;
                        UseProductivity_CCC_lbl.Visibility = Visibility;
                        ImageResult_OfMetrica_CCC.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_CCC_lb.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_CCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind1_OfLOC_CCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind2_OfLOC_CCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind_OfLOC_one_CCC_label.Visibility = Visibility.Hidden;
                        InfoFinish_CCC_label.Visibility = Visibility.Hidden;
                        One_CCC_label.Visibility = Visibility.Hidden;
                        LOC_better_CCC_label.Visibility = Visibility.Hidden;
                        LOC_worse_CCC_label.Visibility = Visibility.Hidden;
                        LOC_probable_CCC_label.Visibility = Visibility.Hidden;
                        DisplayResult1_CCC_lb.Visibility = Visibility.Hidden;
                        DisplayResult2_CCC_lb.Visibility = Visibility.Hidden;
                        CalculationForMetric_CCC_btn.Visibility = Visibility.Hidden;
                        NewParametrs_CCC_btn.Visibility = Visibility.Hidden;
                        CleanParametrs_CCC_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_CCC_btn.Visibility = Visibility.Hidden;
                        GO_Next_CCC_btn.Visibility = Visibility.Hidden;
                        READY_CCC_btn.Visibility = Visibility.Hidden;
                        FindLOC_CCC_btn.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_CCC_dg.Visibility = Visibility.Hidden;
                        OutputTableInfoParameters_CCC_dg.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_CCC_TextBox.Text = "";
                        CalculatInformationAboutThisMetric_CCC_TextBox.Text = "";
                        CountOfFunctions_CCC_tb.Text = "0";
                        TimeForDoIt_ProgressBar(5);
                        Information_UserDo.Text = "Введіть кількість функцій для подальшого знаходження метрики";
                        Information_ProgramDo.Text = "Готово";
                        ResultAnalysisMetric_CCC_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_CCC_dg.Items.Count; i++)
                        {
                            cccMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_CCC_dg.Items.Refresh();
                        OutputTableInfoParameters_CCC_dg.Items.Clear();
                        OutputTableInfoParameters_CCC_dg.Items.Refresh();
                        DisplayResult2_CCC_lb.Content = "        всього програмного коду становить: ";
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        TimeForDoIt_ProgressBar(10);
                        countOfFunction_FP = 1;
                        Information_UserDo.Text = "Підготовлювання робочої зони для FP метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        CountOfFunctions_FP_label.Visibility = Visibility;
                        CountOfFunctions_FP_tb.Visibility = Visibility;
                        SetCountOfFunctions_FP_button.Visibility = Visibility;
                        NoneSpecialReq_FP_chbx.IsChecked = false;
                        ExistSpecialReq_FP_chbx.IsChecked = false;
                        ImageResult_OfMetrica_FP.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_FP_lb.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_FP_label.Visibility = Visibility.Hidden;
                        DisplayResult_FP_lb.Visibility = Visibility.Hidden;
                        CalculationForMetric_FP_btn.Visibility = Visibility.Hidden;
                        NewParametrs_FP_btn.Visibility = Visibility.Hidden;
                        CleanParametrs_FP_btn.Visibility = Visibility.Hidden;
                        GO_Next_FP_btn.Visibility = Visibility.Hidden;
                        READY_FP_btn.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_FP_dg.Visibility = Visibility.Hidden;
                        OutputTableInfoParameters_FP_dg.Visibility = Visibility.Hidden;
                        InformationAboutThisMetric_FP_TextBox.Text = "";
                        CalculatInformationAboutThisMetric_FP_TextBox.Text = "";
                        CountOfFunctions_FP_tb.Text = "0";
                        TimeForDoIt_ProgressBar(5);
                        Information_UserDo.Text = "Введіть кількість функцій для подальшого знаходження метрики";
                        Information_ProgramDo.Text = "Готово";
                        ResultAnalysisMetric_FP_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_FP_dg.Items.Count; i++)
                        {
                            fpMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_FP_dg.Items.Refresh();
                        OutputTableInfoParameters_FP_dg.Items.Clear();
                        OutputTableInfoParameters_FP_dg.Items.Refresh();
                    } break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        try
                        {
                            CalculateMetric_LC_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів LC метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_LC_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_LC_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                lcMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            //перевірка на допустимість вхідних значень
                            var cel1 = DataGridHelper.GetCell(TableInfoParametrs_LC_dg, 1, 1);
                            var content1 = cel1.Content as TextBlock;
                            var a1 = content1.Text;
                            var cel2 = DataGridHelper.GetCell(TableInfoParametrs_LC_dg, 2, 1);
                            var content2 = cel2.Content as TextBlock;
                            var b2 = content2.Text;
                            if ((double.Parse(a1, CultureInfo.InvariantCulture) != 2.4 && double.Parse(a1, CultureInfo.InvariantCulture) != 3.6 && double.Parse(a1, CultureInfo.InvariantCulture) != 3.0) || (double.Parse(b2, CultureInfo.InvariantCulture) != 1.05 && double.Parse(b2, CultureInfo.InvariantCulture) != 1.20 && double.Parse(b2, CultureInfo.InvariantCulture) != 1.12))
                            {
                                MessageBox.Show("Некоректні коефіцієнти СОСОМО! Ви намагаєтесь змінити константні значення! Оновіть та попробуйте ще раз знайти метрику!! ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                                TimeForDoIt_ProgressBar(10);
                                Information_UserDo.Text = "Заповнення значеннями вхідних параметрів LC метрики успішно завершено";
                                Information_ProgramDo.Text = "Готово";
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        try
                        {
                            CalculateMetric_DP_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Заповнення значеннями вхідних параметрів DP метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_DP_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_DP_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                dpMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            //перевірка на допустимість вхідних значень
                            var cel1 = DataGridHelper.GetCell(TableInfoParametrs_DP_dg, 1, 1);
                            var content1 = cel1.Content as TextBlock;
                            var a1 = content1.Text;
                            var cel2 = DataGridHelper.GetCell(TableInfoParametrs_DP_dg, 2, 1);
                            var content2 = cel2.Content as TextBlock;
                            var b2 = content2.Text;
                            var cel3 = DataGridHelper.GetCell(TableInfoParametrs_DP_dg, 3, 1);
                            var content3 = cel3.Content as TextBlock;
                            var c3 = content3.Text;
                            var cel4 = DataGridHelper.GetCell(TableInfoParametrs_DP_dg, 4, 1);
                            var content4 = cel4.Content as TextBlock;
                            var d4 = content4.Text;
                            if ((double.Parse(a1, CultureInfo.InvariantCulture) != 2.4 && double.Parse(a1, CultureInfo.InvariantCulture) != 3.6 && double.Parse(a1, CultureInfo.InvariantCulture) != 3.0) 
                                || (double.Parse(b2, CultureInfo.InvariantCulture) != 1.05 && double.Parse(b2, CultureInfo.InvariantCulture) != 1.20 && double.Parse(b2, CultureInfo.InvariantCulture) != 1.12)
                                || (double.Parse(c3, CultureInfo.InvariantCulture) != 2.5 && double.Parse(c3, CultureInfo.InvariantCulture) != 2.5 && double.Parse(c3, CultureInfo.InvariantCulture) != 2.5)
                                || (double.Parse(d4, CultureInfo.InvariantCulture) != 0.38 && double.Parse(d4, CultureInfo.InvariantCulture) != 0.32 && double.Parse(d4, CultureInfo.InvariantCulture) != 0.35))
                            {
                                MessageBox.Show("Некоректні коефіцієнти СОСОМО! Ви намагаєтесь змінити константні значення! Оновіть та попробуйте ще раз знайти метрику!! ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Нові значення параметрів успішно внесені та змінені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                                TimeForDoIt_ProgressBar(10);
                                Information_UserDo.Text = "Заповнення значеннями вхідних параметрів DP метрики успішно завершено";
                                Information_ProgramDo.Text = "Готово";
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                default: break;
            }
        }



        // < RefreshParametrsForThisMetric_Click > - функція оновлення параметрів метрики
        private void RefreshParametrsForThisMetric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Оновлення параметрів для метрики ...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        CalculateMetric_CHP_btn.Visibility = Visibility;
                        TimeForDoIt_ProgressBar(10);
                        Information_UserDo.Text = "Оновлення значеннь вхідних параметрів CHP метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        if ((Value_1_CHP_rb.IsChecked == false) && (Value_2_CHP_rb.IsChecked == false) && (Value_3_CHP_rb.IsChecked == false) && (Value_4_CHP_rb.IsChecked == false) && (Value_5_CHP_rb.IsChecked == false) && (Value_6_CHP_rb.IsChecked == false) && (Value_7_CHP_rb.IsChecked == false))
                        {
                            MessageBox.Show("Не вдалось обновити! Не обрано рівня зв'язності! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення вхідних параметрів CHP метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        CalculateMetric_CPP_btn.Visibility = Visibility;
                        TimeForDoIt_ProgressBar(10);
                        Information_UserDo.Text = "Оновлення значеннь вхідних параметрів CPP метрики ...";
                        Information_ProgramDo.Text = "Очікування ...";
                        if ((Value_1_CPP_rb.IsChecked == false) && (Value_2_CPP_rb.IsChecked == false) && (Value_3_CPP_rb.IsChecked == false) && (Value_4_CPP_rb.IsChecked == false) && (Value_5_CPP_rb.IsChecked == false) && (Value_6_CPP_rb.IsChecked == false))
                        {
                            MessageBox.Show("Не вдалось обновити! Не обрано типу зчеплення! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення вхідних параметрів CPP метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                    } break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        try
                        {
                            CalculateMetric_RUP_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів RUP метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_RUP_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_RUP_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                rupMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів RUP метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Не вдалось оновити! Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        try
                        {
                            CalculateMetric_MMT_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів MMT метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_MMT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_MMT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                mmtMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            var qcl = DataGridHelper.GetCell(TableInfoParametrs_MMT_dg, 0, 1);
                            var cont = qcl.Content as TextBlock;
                            var _qcl = cont.Text;
                            if (Convert.ToInt32(_qcl) > 50000)
                            {
                                MessageBox.Show("Невірні вхідні дані! Кількість рядків коду перевищують допустиму межу (50 000)! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                                TimeForDoIt_ProgressBar(10);
                                Information_UserDo.Text = "Оновлення значеннь вхідних параметрів MMT метрики успішно завершено";
                                Information_ProgramDo.Text = "Готово";
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Не вдалось оновити! Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        try
                        {
                            CalculateMetric_MBQ_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів MBQ метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_MBQ_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_MBQ_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                mbqMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів MBQ метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Не вдалось оновити! Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        try
                        {
                            CalculateMetric_SCT_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів SCT метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_SCT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_SCT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sctMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів SCT метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Не вдалось оновити! Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        try
                        {
                            CalculateMetric_SDT_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів SDT метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_SDT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_SDT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sdtMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів SDT метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Не вдалось оновити! Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        try
                        {
                            CalculateMetric_SQC_btn.Visibility = Visibility;
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів SQC метрики ...";
                            Information_ProgramDo.Text = "Очікування ...";

                            for (int i = 0; i < TableInfoParametrs_SQC_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(TableInfoParametrs_SQC_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sqcMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }
                            MessageBox.Show("Нові значення параметрів успішно внесені та оновлені! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Оновлення значеннь вхідних параметрів SQC метрики успішно завершено";
                            Information_ProgramDo.Text = "Готово";
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Не вдалось оновити! Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + exception, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        CalculateMetric_LC_btn.Visibility = Visibility;
                        TableInfoParametrs_LC_dg.Items.Refresh();
                        SetKoefCOCOMO_LC_button.Visibility = Visibility;
                        KoefCOCOMO_LC_label.Visibility = Visibility;
                        ListKoef1_cmbx.Visibility = Visibility;
                        ListKoef2_cmbx.Visibility = Visibility;
                        ListKoef3_cmbx.Visibility = Visibility;
                        ForIndependent_LC_chbx.Visibility = Visibility;
                        ForIndependent_LC_chbx.IsChecked = false;
                        ForEmbedded_LC_chbx.Visibility = Visibility;
                        ForEmbedded_LC_chbx.IsChecked = false;
                        ForIntermediate_LC_chbx.Visibility = Visibility;
                        ForIntermediate_LC_chbx.IsChecked = false;
                        TableInfoParametrs_LC_dg.Visibility = Visibility.Hidden;
                        CalculateMetric_LC_btn.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_LC_image.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_LC_lb.Visibility = Visibility.Hidden;
                        CleanParametrs_LC_btn.Visibility = Visibility.Hidden;
                        RefreshParametrs_LC_btn.Visibility = Visibility.Hidden;
                        NewParametrs_LC_btn.Visibility = Visibility.Hidden;
                        CalculationForMetric_LC_btn.Visibility = Visibility.Hidden;
                        lcMetric.ClearAllParameters_OfMetric();
                    }
                    break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        CalculateMetric_DP_btn.Visibility = Visibility;
                        TableInfoParametrs_DP_dg.Items.Refresh();
                        SetKoefCOCOMO_DP_button.Visibility = Visibility;
                        KoefCOCOMO_DP_label.Visibility = Visibility;
                        ListKoef1_DP_cmbx.Visibility = Visibility;
                        ListKoef2_DP_cmbx.Visibility = Visibility;
                        ListKoef3_DP_cmbx.Visibility = Visibility;
                        ForIndependent_DP_chbx.Visibility = Visibility;
                        ForIndependent_DP_chbx.IsChecked = false;
                        ForEmbedded_DP_chbx.Visibility = Visibility;
                        ForEmbedded_DP_chbx.IsChecked = false;
                        ForIntermediate_DP_chbx.Visibility = Visibility;
                        ForIntermediate_DP_chbx.IsChecked = false;
                        TableInfoParametrs_DP_dg.Visibility = Visibility.Hidden;
                        CalculateMetric_DP_btn.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_DP_image.Visibility = Visibility.Hidden;
                        ResultAnalysisMetric_DP_lb.Visibility = Visibility.Hidden;
                        CleanParametrs_DP_btn.Visibility = Visibility.Hidden;
                        RefreshParametrs_DP_btn.Visibility = Visibility.Hidden;
                        NewParametrs_DP_btn.Visibility = Visibility.Hidden;
                        CalculationForMetric_DP_btn.Visibility = Visibility.Hidden;
                        dpMetric.ClearAllParameters_OfMetric();
                    } break;
                default: break;
            }
        }


        // < CleanParametrsForThisMetric_Click > - функція очищення параметрів метрики
        private void CleanParametrsForThisMetric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Очищення параметрів метрики ...";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        chpMetric.ClearAllParameters_OfMetric();
                        ResultAnalysisMetric_CHP_lb.Content = 0;
                    } break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        cppMetric.ClearAllParameters_OfMetric();
                        ResultAnalysisMetric_CPP_lb.Content = 0;
                    }
                    break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        rupMetric.ClearAllParameters_OfMetric();
                        TableInfoParametrs_RUP_dg.Items.Refresh();
                        ResultAnalysisMetric_RUP_lb.Content = 0;
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        mmtMetric.ClearAllParameters_OfMetric();
                        TableInfoParametrs_MMT_dg.Items.Refresh();
                        ResultAnalysisMetric_MMT_lb.Content = 0;
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        mbqMetric.ClearAllParameters_OfMetric();
                        TableInfoParametrs_MBQ_dg.Items.Refresh();
                        ResultAnalysisMetric_MBQ_lb.Content = 0;
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        sctMetric.ClearAllParameters_OfMetric();
                        TableInfoParametrs_SCT_dg.Items.Refresh();
                        ResultAnalysisMetric_SCT_lb.Content = 0;
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        sdtMetric.ClearAllParameters_OfMetric();
                        TableInfoParametrs_SDT_dg.Items.Refresh();
                        ResultAnalysisMetric_SDT_lb.Content = 0;
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        ResultAnalysisMetric_SCC_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_SCC_dg.Items.Count; i++)
                        {
                            sccMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_SCC_dg.Items.Refresh();
                        OutputTableInfoParameters_SCC_dg.Items.Clear();
                        OutputTableInfoParameters_SCC_dg.Items.Refresh();
                        DisplayResult2_SCC_lb.Content = "        всього ПЗ становить: ";
                        sccMetric.ClearAllParameters_OfMetric();
                    }
                    break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        sqcMetric.ClearAllParameters_OfMetric();
                        TableInfoParametrs_SQC_dg.Items.Refresh();
                        ResultAnalysisMetric_SQC_lb.Content = 0;
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        ResultAnalysisMetric_CPT_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_CPT_dg.Items.Count; i++)
                        {
                            cptMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_CPT_dg.Items.Refresh();
                        OutputTableInfoParameters_CPT_dg.Items.Clear();
                        OutputTableInfoParameters_CPT_dg.Items.Refresh();
                        DisplayResult2_CPT_lb.Content = "        всього ПЗ становить: ";
                        cptMetric.ClearAllParameters_OfMetric();
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        ResultAnalysisMetric_CCC_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_CCC_dg.Items.Count; i++)
                        {
                            cccMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_CCC_dg.Items.Refresh();
                        OutputTableInfoParameters_CCC_dg.Items.Clear();
                        OutputTableInfoParameters_CCC_dg.Items.Refresh();
                        DisplayResult2_CCC_lb.Content = "        всього програмного коду становить: ";
                        cccMetric.ClearAllParameters_OfMetric();
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        ResultAnalysisMetric_FP_lb.Content = 0;
                        for (int i = 0; i < InputTableInfoParameters_FP_dg.Items.Count; i++)
                        {
                            fpMetric.ChangeValue_OfParameter(i, 0);
                        }
                        InputTableInfoParameters_FP_dg.Items.Refresh();
                        OutputTableInfoParameters_FP_dg.Items.Clear();
                        OutputTableInfoParameters_FP_dg.Items.Refresh();
                        fpMetric.ClearAllParameters_OfMetric();
                        DisplayResult_FP_lb.Content = "";
                    } break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        lcMetric.ChangeValue_OfParameter(0, 0);
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                        TableInfoParametrs_LC_dg.Items.Refresh();
                        ResultAnalysisMetric_LC_lb.Content = 0;
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        dpMetric.ChangeValue_OfParameter(0, 0);
                        MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
                        TableInfoParametrs_DP_dg.Items.Refresh();
                        ResultAnalysisMetric_DP_lb.Content = 0;
                    } break;
                default: break;
            }
        }


        // < CalculateForThisMetric_Click > - функція обчислення параметрів метрики
        private void CalculateForThisMetric_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Знаходження значення метрики на основі введених параметрів ...";
            Information_ProgramDo.Text = "Очікування...";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 0:  //-----------------------------------------------------------------CHP 
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_CHP_btn.Visibility = Visibility;
                        ResultAnalysisMetric_CHP_lb.Content = chpMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика зв'язності                                                        " + chpMetric.FindMetric() + "                        " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_CHP_btn.Visibility = Visibility.Hidden;
                        TableResult[0].Value = chpMetric.FindMetric();
                    }
                    break;
                case 1:  //-----------------------------------------------------------------CPP 
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_CPP_btn.Visibility = Visibility;
                        ResultAnalysisMetric_CPP_lb.Content = cppMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика зчеплення                                                        " + cppMetric.FindMetric() + "                        " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_CPP_btn.Visibility = Visibility.Hidden;
                        TableResult[1].Value = cppMetric.FindMetric();
                    }
                    break;
                case 2:  //-----------------------------------------------------------------RUP 
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_RUP_btn.Visibility = Visibility;
                        ResultAnalysisMetric_RUP_lb.Content = rupMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика звернення до глобальної змінної                     " + rupMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_RUP_btn.Visibility = Visibility.Hidden;
                        TableResult[2].Value = rupMetric.FindMetric();
                    }
                    break;
                case 3:  //-----------------------------------------------------------------MMT
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_MMT_btn.Visibility = Visibility;
                        ResultAnalysisMetric_MMT_lb.Content = mmtMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика часу модифікації моделей                            " + mmtMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_MMT_btn.Visibility = Visibility.Hidden;
                        TableResult[3].Value = mmtMetric.FindMetric();
                    }
                    break;
                case 4:  //-----------------------------------------------------------------MBQ
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_MBQ_btn.Visibility = Visibility;
                        ResultAnalysisMetric_MBQ_lb.Content = mbqMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика к-сті зн. помилок при інспектуванні                  " + mbqMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_MBQ_btn.Visibility = Visibility.Hidden;
                        TableResult[4].Value = mbqMetric.FindMetric();
                    }
                    break;
                case 5:  //-----------------------------------------------------------------SCT
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_SCT_btn.Visibility = Visibility;
                        ResultAnalysisMetric_SCT_lb.Content = sctMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. загального часу розроблення ПЗ                  " + sctMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_SCT_btn.Visibility = Visibility.Hidden;
                        TableResult[5].Value = sctMetric.FindMetric();
                    }
                    break;
                case 6:  //-----------------------------------------------------------------SDT
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_SDT_btn.Visibility = Visibility;
                        ResultAnalysisMetric_SDT_lb.Content = sdtMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика часу виконання робіт процесу проектування ПЗ          " + sdtMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_SDT_btn.Visibility = Visibility.Hidden;
                        TableResult[6].Value = sdtMetric.FindMetric();
                    }
                    break;
                case 8:  //-----------------------------------------------------------------SQC
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_SQC_btn.Visibility = Visibility;
                        ResultAnalysisMetric_SQC_lb.Content = sqcMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. вартості перевірки якості ПЗ                     " + sqcMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_SQC_btn.Visibility = Visibility.Hidden;
                        TableResult[8].Value = sqcMetric.FindMetric();
                    }
                    break;
                case 12:  //-----------------------------------------------------------------LC
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_LC_btn.Visibility = Visibility;
                        ResultAnalysisMetric_LC_lb.Content = lcMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. оцінки трудовитрат (за моделлю Боема)             " + lcMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_LC_btn.Visibility = Visibility.Hidden;
                        TableResult[12].Value = lcMetric.FindMetric();
                    }
                    break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        Information_ProgramDo.Text = "Готово";
                        CalculationForMetric_DP_btn.Visibility = Visibility;
                        ResultAnalysisMetric_DP_lb.Content = dpMetric.FindMetric().ToString();
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. оцінки тривалості проекту (за моделлю Боема)      " + dpMetric.FindMetric() + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        CalculateMetric_DP_btn.Visibility = Visibility.Hidden;
                        TableResult[13].Value = dpMetric.FindMetric();
                    }
                    break;
                default: break;
            }
        }

        //------------------------------------------------------------------------------------------------
        // ФУНКЦІЇ ДЛЯ МЕТРИК - SCC, CPT, CCC, FP для покрокового знаходження метрики, із заданням кількості функцій та анімаційних ефектів 
        //------------------------------------------------------------------------------------------------
        // < SetCountOfFunctions_btn_Click > - функція встановлення кількості подальших функцій та перехід на наступний крок
        private void SetCountOfFunctions_btn_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Збереження кількості функції...";
            Information_ProgramDo.Text = "Очікування...";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        if (CountOfFunctions_SCC_tb.Text == "0")
                        {
                            MessageBox.Show("Не вдалось задати кількість функцій!Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            Information_ProgramDo.Text = "Готово";
                            InfoForStepSetOfFunctions_SCC_label.Content = "Введіть необхідні дані для 1-ої функції";
                            sccMetric.CountOfFunction = Convert.ToInt32(CountOfFunctions_SCC_tb.Text);
                            CountOfFunctions_SCC_label.Visibility = Visibility.Hidden;
                            CountOfFunctions_SCC_tb.Visibility = Visibility.Hidden;
                            SetCountOfFunctions_SCC_button.Visibility = Visibility.Hidden;
                            InfoForStepSetOfFunctions_SCC_label.Visibility = Visibility;
                            InfoAboutParameters1_SCC_label.Visibility = Visibility;
                            InfoAboutParameters2_SCC_label.Visibility = Visibility;
                            FindFormula_OfLOC_SCC_btn.Visibility = Visibility;
                            GO_Next_SCC_btn.Visibility = Visibility;
                            InputTableInfoParameters_SCC_dg.Visibility = Visibility;
                            if (countOfFunction_SCC == sccMetric.CountOfFunction)
                            {
                                GO_Next_SCC_btn.Visibility = Visibility.Hidden;
                                READY_SCC_btn.Visibility = Visibility.Visible;
                            }
                        }
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        if (CountOfFunctions_CPT_tb.Text == "0")
                        {
                            MessageBox.Show("Не вдалось задати кількість функцій!Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            Information_ProgramDo.Text = "Готово";
                            InfoForStepSetOfFunctions_CPT_label.Content = "Введіть необхідні дані для 1-ої функції";
                            cptMetric.CountOfFunction = Convert.ToInt32(CountOfFunctions_CPT_tb.Text);
                            CountOfFunctions_CPT_label.Visibility = Visibility.Hidden;
                            CountOfFunctions_CPT_tb.Visibility = Visibility.Hidden;
                            SetCountOfFunctions_CPT_button.Visibility = Visibility.Hidden;
                            InfoForStepSetOfFunctions_CPT_label.Visibility = Visibility;
                            FindFormula_OfLOC_CPT_btn.Visibility = Visibility;
                            GO_Next_CPT_btn.Visibility = Visibility;
                            InputTableInfoParameters_CPT_dg.Visibility = Visibility;
                            if (countOfFunction_CPT == cptMetric.CountOfFunction)
                            {
                                GO_Next_CPT_btn.Visibility = Visibility.Hidden;
                                READY_CPT_btn.Visibility = Visibility.Visible;
                            }
                        }
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        if (CountOfFunctions_CCC_tb.Text == "0")
                        {
                            MessageBox.Show("Не вдалось задати кількість функцій!Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            Information_ProgramDo.Text = "Готово";
                            InfoForStepSetOfFunctions_CCC_label.Content = "Введіть необхідні дані для 1-ої функції";
                            cccMetric.CountOfFunction = Convert.ToInt32(CountOfFunctions_CCC_tb.Text);
                            CountOfFunctions_CCC_label.Visibility = Visibility.Hidden;
                            CountOfFunctions_CCC_tb.Visibility = Visibility.Hidden;
                            SetCountOfFunctions_CCC_button.Visibility = Visibility.Hidden;
                            InfoForStepSetOfFunctions_CCC_label.Visibility = Visibility;
                            UseProductivity_CCC_chbx.Visibility = Visibility.Hidden;
                            UseProductivity_CCC_lbl.Visibility = Visibility.Hidden;
                            FindFormula_OfLOC_CCC_btn.Visibility = Visibility;
                            GO_Next_CCC_btn.Visibility = Visibility;
                            InputTableInfoParameters_CCC_dg.Visibility = Visibility;
                            if (countOfFunction_CCC == cccMetric.CountOfFunction)
                            {
                                GO_Next_CCC_btn.Visibility = Visibility.Hidden;
                                READY_CCC_btn.Visibility = Visibility.Visible;
                            }
                            if (UseProductivity_CCC_chbx.IsChecked == true)
                            {
                                if (ResultAnalysisMetric_CPT_lb.Content.ToString() == "0")
                                {
                                    MessageBox.Show("Прогнозована продуктивність розроблення ПЗ становить 0 або не була попередньо знайдена! \n ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    cccMetric.ChangeValue_OfParameter(0, 0);
                                    cccMetric.ChangeValue_OfParameter(1, double.Parse(ResultAnalysisMetric_CPT_lb.Content.ToString()));
                                    InputTableInfoParameters_CCC_dg.Items.Refresh();
                                }
                            }
                        }
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        if (CountOfFunctions_FP_tb.Text == "0")
                        {
                            MessageBox.Show("Не вдалось задати кількість функцій!Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            Information_ProgramDo.Text = "Готово";
                            CountOfFunctions_FP_label.Visibility = Visibility.Hidden;
                            CountOfFunctions_FP_tb.Visibility = Visibility.Hidden;
                            SetCountOfFunctions_FP_button.Visibility = Visibility.Hidden;
                            Availability_OfSpecialReq_FP_label.Visibility = Visibility;
                            NoneSpecialReq_FP_chbx.Visibility = Visibility;
                            ExistSpecialReq_FP_chbx.Visibility = Visibility;
                            SetAvailability_OfSpecialRequirements_FP_button.Visibility = Visibility;
                            fpMetric.CountOfFunction = Convert.ToInt32(CountOfFunctions_FP_tb.Text);
                        }
                    }
                    break;
            }
        }


        // < Go_Next_forSetInfo_ofFunction > - функція встановлення інформації для кожної функції, з паралельним знаходженням кінцевого значення метрики та історії дій, 
        private void Go_Next_forSetInfo_ofFunction(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Встановлення значень параметрів для функції ";
            Information_ProgramDo.Text = "Очікування...";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        InfoForStepSetOfFunctions_SCC_label.Content = "Введіть необхідні дані для " + (countOfFunction_SCC + 1) + "-ої функції";
                        if (countOfFunction_SCC == sccMetric.CountOfFunction - 1)
                        {
                            GO_Next_SCC_btn.Visibility = Visibility.Hidden;
                            READY_SCC_btn.Visibility = Visibility.Visible;
                        }
                        if (countOfFunction_SCC < sccMetric.CountOfFunction)
                        {
                            for (int i = 0; i < InputTableInfoParameters_SCC_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                sccMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            //заносимо дані до таблиці результатів: кількість рядків 
                            var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, 0, 1);
                            var content1 = cel1.Content as TextBlock;
                            var LOC1 = content1.Text;
                            //заносимо дані до таблиці результатів: вартість рядка
                            var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, 1, 1);
                            var content2 = cel2.Content as TextBlock;
                            var COST2 = content2.Text;

                            TableForFunction_SCC newrowInfoFunction = new TableForFunction_SCC() { Number = countOfFunction_SCC, LOC = double.Parse(LOC1, CultureInfo.InvariantCulture), Cost = double.Parse(COST2, CultureInfo.InvariantCulture), DevelopmentCost = sccMetric.FindMetric() };
                            OutputTableInfoParameters_SCC_dg.Items.Add(newrowInfoFunction);

                            CalculatInformationAboutThisMetric_SCC_TextBox.Text += sccMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_SCC);
                            sccMetric.FindFinalResult_forSomeMetric(sccMetric.CountOfFunction, countOfFunction_SCC, sccMetric.FindMetric());

                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Дані функції успішно внесені та проаналізовані!";
                            Information_ProgramDo.Text = "Готово";

                            for (int i = 0; i < InputTableInfoParameters_SCC_dg.Items.Count; i++)
                            {
                                sccMetric.ChangeValue_OfParameter(i, 0);
                            }
                            InputTableInfoParameters_SCC_dg.Items.Refresh();
                        }
                        ++countOfFunction_SCC;
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        InfoForStepSetOfFunctions_CPT_label.Content = "Введіть необхідні дані для " + (countOfFunction_CPT + 1) + "-ої функції";
                        if (countOfFunction_CPT == cptMetric.CountOfFunction - 1)
                        {
                            GO_Next_CPT_btn.Visibility = Visibility.Hidden;
                            READY_CPT_btn.Visibility = Visibility.Visible;
                        }
                        if (countOfFunction_CPT < cptMetric.CountOfFunction)
                        {
                            for (int i = 0; i < InputTableInfoParameters_CPT_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                cptMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            //заносимо дані до таблиці результатів: очікувана кількість рядків коду функції
                            var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 0, 1);
                            var content1 = cel1.Content as TextBlock;
                            var LOC1 = content1.Text;
                            //заносимо дані до таблиці результатів: очікувана кількість рядків коду в аналогічній функції
                            var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 1, 1);
                            var content2 = cel2.Content as TextBlock;
                            var LOC2 = content2.Text;
                            //заносимо дані до таблиці результатів: продуктивність процесу розроблення аналогічної функції
                            var cel3 = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 2, 1);
                            var content3 = cel3.Content as TextBlock;
                            var PRDCT3 = content3.Text;

                            TableForFunction_CPT newrowInfoFunction = new TableForFunction_CPT() { Number = countOfFunction_CPT, LOC_expected = double.Parse(LOC1, CultureInfo.InvariantCulture), LOC_similar = double.Parse(LOC2, CultureInfo.InvariantCulture), Productivity_similar = double.Parse(PRDCT3, CultureInfo.InvariantCulture), Productivity = cptMetric.FindMetric() };
                            OutputTableInfoParameters_CPT_dg.Items.Add(newrowInfoFunction);

                            CalculatInformationAboutThisMetric_CPT_TextBox.Text += cptMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_CPT);
                            cptMetric.FindFinalResult_forSomeMetric(cptMetric.CountOfFunction, countOfFunction_CPT, cptMetric.FindMetric());

                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Дані функції успішно внесені та проаналізовані!";
                            Information_ProgramDo.Text = "Готово";

                            for (int i = 0; i < InputTableInfoParameters_CPT_dg.Items.Count; i++)
                            {
                                cptMetric.ChangeValue_OfParameter(i, 0);
                            }
                            InputTableInfoParameters_CPT_dg.Items.Refresh();
                        }
                        ++countOfFunction_CPT;
                    } break;
                case 10:  //-----------------------------------------------------------------cCC
                    {
                        InfoForStepSetOfFunctions_CCC_label.Content = "Введіть необхідні дані для " + (countOfFunction_CCC + 1) + "-ої функції";
                        if (countOfFunction_CCC == cccMetric.CountOfFunction - 1)
                        {
                            GO_Next_CCC_btn.Visibility = Visibility.Hidden;
                            READY_CCC_btn.Visibility = Visibility.Visible;
                        }
                        if (countOfFunction_CCC < cccMetric.CountOfFunction)
                        {
                            for (int i = 0; i < InputTableInfoParameters_CCC_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                cccMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            //заносимо дані до таблиці результатів: кількість рядків 
                            var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, 0, 1);
                            var content1 = cel1.Content as TextBlock;
                            var LOC1 = content1.Text;
                            //заносимо дані до таблиці результатів: вартість рядка
                            var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, 1, 1);
                            var content2 = cel2.Content as TextBlock;
                            var PRDCT2 = content2.Text;

                            TableForFunction_CCC newrowInfoFunction = new TableForFunction_CCC() { Number = countOfFunction_CCC, LOC = double.Parse(LOC1, CultureInfo.InvariantCulture), Productivity = double.Parse(PRDCT2, CultureInfo.InvariantCulture), Costs = cccMetric.FindMetric() };
                            OutputTableInfoParameters_CCC_dg.Items.Add(newrowInfoFunction);

                            CalculatInformationAboutThisMetric_CCC_TextBox.Text += cccMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_CCC);
                            cccMetric.FindFinalResult_forSomeMetric(cccMetric.CountOfFunction, countOfFunction_CCC, cccMetric.FindMetric());

                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Дані функції успішно внесені та проаналізовані!";
                            Information_ProgramDo.Text = "Готово";

                            if (UseProductivity_CCC_chbx.IsChecked == true)
                            {
                                cccMetric.ChangeValue_OfParameter(0, 0);
                                cccMetric.ChangeValue_OfParameter(1, double.Parse(ResultAnalysisMetric_CPT_lb.Content.ToString()));
                                InputTableInfoParameters_CCC_dg.Items.Refresh();
                            }
                            else
                            {
                                for (int i = 0; i < InputTableInfoParameters_CCC_dg.Items.Count; i++)
                                {
                                    cccMetric.ChangeValue_OfParameter(i, 0);
                                }
                            }
                            InputTableInfoParameters_CCC_dg.Items.Refresh();
                        }
                        ++countOfFunction_CCC;
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        InfoForStepSetOfFunctions_FP_label.Content = "Введіть необхідні дані для " + (countOfFunction_FP + 1) + "-ої функції";
                        if (countOfFunction_FP == fpMetric.CountOfFunction - 1)
                        {
                            GO_Next_FP_btn.Visibility = Visibility.Hidden;
                            READY_FP_btn.Visibility = Visibility.Visible;
                        }
                        if (countOfFunction_FP < fpMetric.CountOfFunction)
                        {
                            for (int i = 0; i < InputTableInfoParameters_FP_dg.Items.Count; i++)
                            {
                                var cel = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, i, 1);
                                var content = cel.Content as TextBlock;
                                var text = content.Text;
                                fpMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                            }

                            //заносимо дані до таблиці результатів: EI
                            var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 0, 1);
                            var content1 = cel1.Content as TextBlock;
                            var EI1 = content1.Text;
                            //заносимо дані до таблиці результатів: EO
                            var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 1, 1);
                            var content2 = cel2.Content as TextBlock;
                            var EO2 = content2.Text;
                            //заносимо дані до таблиці результатів: EIN
                            var cel3 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 2, 1);
                            var content3 = cel3.Content as TextBlock;
                            var EIN3 = content3.Text;
                            //заносимо дані до таблиці результатів: ILF
                            var cel4 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 3, 1);
                            var content4 = cel4.Content as TextBlock;
                            var ILF4 = content4.Text;
                            //заносимо дані до таблиці результатів: ELF
                            var cel5 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 4, 1);
                            var content5 = cel5.Content as TextBlock;
                            var ELF5 = content5.Text;

                            TableForFunction_FP newrowInfoFunction = new TableForFunction_FP()
                            {
                                Number = "Функція " + countOfFunction_FP.ToString(),
                                EI = double.Parse(EI1, CultureInfo.InvariantCulture),
                                EO = double.Parse(EO2, CultureInfo.InvariantCulture),
                                EIN = double.Parse(EIN3, CultureInfo.InvariantCulture),
                                ILF = double.Parse(ILF4, CultureInfo.InvariantCulture),
                                ELF = double.Parse(ELF5, CultureInfo.InvariantCulture),
                                TotalEI = double.Parse(EI1, CultureInfo.InvariantCulture) * koefEI,
                                TotalEO = double.Parse(EO2, CultureInfo.InvariantCulture) * koefEO,
                                TotalEIN = double.Parse(EIN3, CultureInfo.InvariantCulture) * koefEIN,
                                TotalILF = double.Parse(ILF4, CultureInfo.InvariantCulture) * koefILF,
                                TotalELF = double.Parse(ELF5, CultureInfo.InvariantCulture) * koefELF,
                                Sum = fpMetric.FindApproximateFP(koefEI, koefEO, koefEIN, koefILF, koefELF)
                            };
                            OutputTableInfoParameters_FP_dg.Items.Add(newrowInfoFunction);

                            CalculatInformationAboutThisMetric_FP_TextBox.Text += fpMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_FP, koefEI, koefEO, koefEIN, koefILF, koefELF);

                            TimeForDoIt_ProgressBar(10);
                            Information_UserDo.Text = "Дані функції успішно внесені та проаналізовані!";
                            Information_ProgramDo.Text = "Готово";

                            for (int i = 0; i < InputTableInfoParameters_FP_dg.Items.Count; i++)
                            {
                                fpMetric.ChangeValue_OfParameter(i, 0);
                            }
                            InputTableInfoParameters_FP_dg.Items.Refresh();
                        }
                        ++countOfFunction_FP;
                    }
                    break;
            }
        }


        // < READY_forFinish_btn_Click > - функція-обробник для переходу на сторінку результатів знаходження метрики
        private void READY_forFinish_btn_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Успішне завершення налаштувань значень параметрів функції. Виконання аналізу та збір даних..";
            Information_ProgramDo.Text = "Збір даних...";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        for (int i = 0; i < InputTableInfoParameters_SCC_dg.Items.Count; i++)
                        {
                            var cel = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, i, 1);
                            var content = cel.Content as TextBlock;
                            var text = content.Text;
                            sccMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                        }

                        //заносимо дані до таблиці результатів: кількість рядків 
                        var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, 0, 1);
                        var content1 = cel1.Content as TextBlock;
                        var LOC1 = content1.Text;
                        //заносимо дані до таблиці результатів: вартість рядка
                        var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, 1, 1);
                        var content2 = cel2.Content as TextBlock;
                        var COST2 = content2.Text;

                        TableForFunction_SCC newrowInfoFunction = new TableForFunction_SCC() { Number = countOfFunction_SCC, LOC = double.Parse(LOC1, CultureInfo.InvariantCulture), Cost = double.Parse(COST2, CultureInfo.InvariantCulture), DevelopmentCost = sccMetric.FindMetric() };
                        OutputTableInfoParameters_SCC_dg.Items.Add(newrowInfoFunction);

                        CalculatInformationAboutThisMetric_SCC_TextBox.Text += sccMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_SCC);
                        var result = sccMetric.FindFinalResult_forSomeMetric(sccMetric.CountOfFunction, countOfFunction_SCC, sccMetric.FindMetric());

                        InfoFinish_SCC_label.Content = "Очікувані вартості процесу розроблення " + countOfFunction_SCC + " функцій";
                        GO_Next_SCC_btn.Visibility = Visibility.Hidden;
                        READY_SCC_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_SCC_btn.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutParameters1_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutParameters2_SCC_label.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_SCC_dg.Visibility = Visibility.Hidden;
                        NewParametrs_SCC_btn.Visibility = Visibility;
                        CleanParametrs_SCC_btn.Visibility = Visibility;
                        OutputTableInfoParameters_SCC_dg.Visibility = Visibility;
                        DisplayResult1_SCC_lb.Visibility = Visibility;
                        DisplayResult2_SCC_lb.Visibility = Visibility;
                        ImageResult_OfMetrica_SCC.Visibility = Visibility;
                        ResultAnalysisMetric_SCC_lb.Visibility = Visibility;
                        InfoFinish_SCC_label.Visibility = Visibility;
                        CalculationForMetric_SCC_btn.Visibility = Visibility;
                        ResultAnalysisMetric_SCC_lb.Content = result;
                        DisplayResult2_SCC_lb.Content = "        всього ПЗ становить: " + result;
                        Information_ProgramDo.Text = "Готово";
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика очікуваної вартості розроблення ПЗ                " + result + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        countOfFunction_SCC = 1;
                        TableResult[7].Value = result;
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        for (int i = 0; i < InputTableInfoParameters_CPT_dg.Items.Count; i++)
                        {
                            var cel = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, i, 1);
                            var content = cel.Content as TextBlock;
                            var text = content.Text;
                            cptMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                        }

                        // заносимо дані до таблиці результатів: очікувана кількість рядків коду функції
                        var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 0, 1);
                        var content1 = cel1.Content as TextBlock;
                        var LOC1 = content1.Text;
                        //заносимо дані до таблиці результатів: очікувана кількість рядків коду в аналогічній функції
                        var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 1, 1);
                        var content2 = cel2.Content as TextBlock;
                        var LOC2 = content2.Text;
                        //заносимо дані до таблиці результатів: продуктивність процесу розроблення аналогічної функції
                        var cel3 = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 2, 1);
                        var content3 = cel3.Content as TextBlock;
                        var PRDCT3 = content3.Text;

                        TableForFunction_CPT newrowInfoFunction = new TableForFunction_CPT() { Number = countOfFunction_CPT, LOC_expected = double.Parse(LOC1, CultureInfo.InvariantCulture), LOC_similar = double.Parse(LOC2, CultureInfo.InvariantCulture), Productivity_similar = double.Parse(PRDCT3, CultureInfo.InvariantCulture), Productivity = cptMetric.FindMetric() };
                        OutputTableInfoParameters_CPT_dg.Items.Add(newrowInfoFunction);

                        CalculatInformationAboutThisMetric_CPT_TextBox.Text += cptMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_CPT);
                        var result = cptMetric.FindFinalResult_forSomeMetric(cptMetric.CountOfFunction, countOfFunction_CPT, cptMetric.FindMetric());

                        InfoFinish_CPT_label.Content = "Очікувані вартості процесу розроблення " + countOfFunction_CPT + " функцій";
                        GO_Next_CPT_btn.Visibility = Visibility.Hidden;
                        READY_CPT_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_CPT_btn.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_CPT_label.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_CPT_dg.Visibility = Visibility.Hidden;
                        NewParametrs_CPT_btn.Visibility = Visibility;
                        CleanParametrs_CPT_btn.Visibility = Visibility;
                        OutputTableInfoParameters_CPT_dg.Visibility = Visibility;
                        DisplayResult1_CPT_lb.Visibility = Visibility;
                        DisplayResult2_CPT_lb.Visibility = Visibility;
                        ImageResult_OfMetrica_CPT.Visibility = Visibility;
                        ResultAnalysisMetric_CPT_lb.Visibility = Visibility;
                        InfoFinish_CPT_label.Visibility = Visibility;
                        CalculationForMetric_CPT_btn.Visibility = Visibility;
                        ResultAnalysisMetric_CPT_lb.Content = result;
                        DisplayResult2_CPT_lb.Content = "        всього ПЗ становить: " + result;
                        Information_ProgramDo.Text = "Готово";
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. продуктивності розроблення ПЗ             " + result + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        countOfFunction_CPT = 1;
                        TableResult[9].Value = result;
                    }
                    break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        for (int i = 0; i < InputTableInfoParameters_CCC_dg.Items.Count; i++)
                        {
                            var cel = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, i, 1);
                            var content = cel.Content as TextBlock;
                            var text = content.Text;
                            cccMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                        }

                        //заносимо дані до таблиці результатів: кількість рядків 
                        var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, 0, 1);
                        var content1 = cel1.Content as TextBlock;
                        var LOC1 = content1.Text;
                        //заносимо дані до таблиці результатів: вартість рядка
                        var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, 1, 1);
                        var content2 = cel2.Content as TextBlock;
                        var PRDCT2 = content2.Text;

                        TableForFunction_CCC newrowInfoFunction = new TableForFunction_CCC() { Number = countOfFunction_CCC, LOC = double.Parse(LOC1, CultureInfo.InvariantCulture), Productivity = double.Parse(PRDCT2, CultureInfo.InvariantCulture), Costs = cccMetric.FindMetric() };
                        OutputTableInfoParameters_CCC_dg.Items.Add(newrowInfoFunction);

                        CalculatInformationAboutThisMetric_CCC_TextBox.Text += cccMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_CCC);
                        var result = cccMetric.FindFinalResult_forSomeMetric(cccMetric.CountOfFunction, countOfFunction_CCC, cccMetric.FindMetric());

                        InfoFinish_CCC_label.Content = "Очікувані вартості процесу розроблення " + countOfFunction_CCC + " функцій";
                        GO_Next_CCC_btn.Visibility = Visibility.Hidden;
                        READY_CCC_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_CCC_btn.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_CCC_label.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_CCC_dg.Visibility = Visibility.Hidden;
                        NewParametrs_CCC_btn.Visibility = Visibility;
                        CleanParametrs_CCC_btn.Visibility = Visibility;
                        OutputTableInfoParameters_CCC_dg.Visibility = Visibility;
                        DisplayResult1_CCC_lb.Visibility = Visibility;
                        DisplayResult2_CCC_lb.Visibility = Visibility;
                        ImageResult_OfMetrica_CCC.Visibility = Visibility;
                        ResultAnalysisMetric_CCC_lb.Visibility = Visibility;
                        InfoFinish_CCC_label.Visibility = Visibility;
                        CalculationForMetric_CCC_btn.Visibility = Visibility;
                        ResultAnalysisMetric_CCC_lb.Content = result;
                        DisplayResult2_CCC_lb.Content = "        всього програмного коду становить: " + result;
                        Information_ProgramDo.Text = "Готово";
                        Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. витрат на реалізацію програмного коду            " + result + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                        countOfOperationCalculation_OfMetric++;
                        countOfFunction_CCC = 1;
                        TableResult[10].Value = result;
                    } break;
                case 11:  //-----------------------------------------------------------------FP
                    {
                        for (int i = 0; i < InputTableInfoParameters_FP_dg.Items.Count; i++)
                        {
                            var cel = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, i, 1);
                            var content = cel.Content as TextBlock;
                            var text = content.Text;
                            fpMetric.ChangeValue_OfParameter(i, double.Parse(text, CultureInfo.InvariantCulture));
                        }

                        //заносимо дані до таблиці результатів: EI
                        var cel1 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 0, 1);
                        var content1 = cel1.Content as TextBlock;
                        var EI1 = content1.Text;
                        //заносимо дані до таблиці результатів: EO
                        var cel2 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 1, 1);
                        var content2 = cel2.Content as TextBlock;
                        var EO2 = content2.Text;
                        //заносимо дані до таблиці результатів: EIN
                        var cel3 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 2, 1);
                        var content3 = cel3.Content as TextBlock;
                        var EIN3 = content3.Text;
                        //заносимо дані до таблиці результатів: ILF
                        var cel4 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 3, 1);
                        var content4 = cel4.Content as TextBlock;
                        var ILF4 = content4.Text;
                        //заносимо дані до таблиці результатів: ELF
                        var cel5 = DataGridHelper.GetCell(InputTableInfoParameters_FP_dg, 4, 1);
                        var content5 = cel5.Content as TextBlock;
                        var ELF5 = content5.Text;

                        TableForFunction_FP newrowInfoFunction = new TableForFunction_FP()
                        {
                            Number = "Функція " + countOfFunction_FP.ToString(),
                            EI = double.Parse(EI1, CultureInfo.InvariantCulture),
                            EO = double.Parse(EO2, CultureInfo.InvariantCulture),
                            EIN = double.Parse(EIN3, CultureInfo.InvariantCulture),
                            ILF = double.Parse(ILF4, CultureInfo.InvariantCulture),
                            ELF = double.Parse(ELF5, CultureInfo.InvariantCulture),
                            TotalEI = double.Parse(EI1, CultureInfo.InvariantCulture) * koefEI,
                            TotalEO = double.Parse(EO2, CultureInfo.InvariantCulture) * koefEO,
                            TotalEIN = double.Parse(EIN3, CultureInfo.InvariantCulture) * koefEIN,
                            TotalILF = double.Parse(ILF4, CultureInfo.InvariantCulture) * koefILF,
                            TotalELF = double.Parse(ELF5, CultureInfo.InvariantCulture) * koefELF,
                            Sum = fpMetric.FindApproximateFP(koefEI, koefEO, koefEIN, koefILF, koefELF)
                        };
                        OutputTableInfoParameters_FP_dg.Items.Add(newrowInfoFunction);

                        CalculatInformationAboutThisMetric_FP_TextBox.Text += fpMetric.ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(countOfFunction_FP, koefEI, koefEO, koefEIN, koefILF, koefELF);

                        GO_Next_FP_btn.Visibility = Visibility.Hidden;
                        READY_FP_btn.Visibility = Visibility.Hidden;
                        InfoForStepSetOfFunctions_FP_label.Visibility = Visibility.Hidden;
                        InputTableInfoParameters_FP_dg.Visibility = Visibility.Hidden;
                        if (_specialRequirement == false)
                        {
                            NewParametrs_FP_btn.Visibility = Visibility;
                            CleanParametrs_FP_btn.Visibility = Visibility;
                            OutputTableInfoParameters_FP_dg.Visibility = Visibility;
                            ImageResult_OfMetrica_FP.Visibility = Visibility;
                            ResultAnalysisMetric_FP_lb.Visibility = Visibility;
                            CalculationForMetric_FP_btn.Visibility = Visibility;
                            CalculatInformationAboutThisMetric_FP_TextBox.Text += "Sum(Fi)=0 - Сума загальних характеристик становить 0, оскільки немає (жодних) спеціальних вимог до проекту.\n";
                            CalculatInformationAboutThisMetric_FP_TextBox.Text += "FP(наближений)= ";
                            for (int i = 1; i <=fpMetric.CountOfFunction; i++)
                            {
                                CalculatInformationAboutThisMetric_FP_TextBox.Text += "FP(набл.)("+i+") ";
                                if (i != fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "+";
                                if (i == fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "=\n";
                            }
                            double totalSUM = 0;
                            for (int i = 1; i <=fpMetric.CountOfFunction; i++)
                            {
                                var cel = DataGridHelper.GetCell(OutputTableInfoParameters_FP_dg, i, 11);
                                var content = cel.Content as TextBlock;
                                var sum = content.Text;
                                totalSUM += double.Parse(sum, CultureInfo.InvariantCulture);
                                CalculatInformationAboutThisMetric_FP_TextBox.Text += sum;
                                if (i != fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "+";
                                if (i == fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "= ";
                            }
                            CalculatInformationAboutThisMetric_FP_TextBox.Text += totalSUM +"\n\n";
                            CalculatInformationAboutThisMetric_FP_TextBox.Text += "Оскільки немає жодних спеціальних вимог до проекту, то ми повинні FP(набл.) зменшити на 35%\n";
                            double resultFP = fpMetric.FindMetric_FP(_specialRequirement,totalSUM,0);
                            CalculatInformationAboutThisMetric_FP_TextBox.Text += "FP(уточн.) = FP(набл.) * [0,65 + 0,01*Sum(Fi)] = "+(totalSUM-(totalSUM*0.35))+"*"+"[0,65 + 0,01*0] = "+ resultFP + "\n";
                            ResultAnalysisMetric_FP_lb.Content = resultFP;
                            DisplayResult_FP_lb.Visibility = Visibility;
                            DisplayResult_FP_lb.Content = "FP(уточн.) = FP(набл.) * [0, 65 + 0, 01 * Sum(Fi)] = \n"+ (totalSUM - (totalSUM * 0.35)) + " * "+"[0, 65 + 0, 01 * 0] = "+ resultFP;
                            Information_ProgramDo.Text = "Готово";
                            Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. функційного розміру                     " + resultFP + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                            Information_ProgramDo.Text = "Готово";
                            countOfOperationCalculation_OfMetric++;
                            countOfFunction_FP = 1;
                            TableResult[11].Value = resultFP;
                        }
                        if (_specialRequirement == true)
                        {
                            SetInfo_Of14Characteristics_FP_label.Visibility = Visibility;
                            SetInfo_Of14Characteristics_FP_button.Visibility = Visibility;
                            InfoTableOf14Characteristic_FP_dg.Visibility = Visibility;
                            listOfCharacteristics.Add(new MyTableInfoCharacteristic_forMetricFP("Вага", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                            InfoTableOf14Characteristic_FP_dg.ItemsSource = listOfCharacteristics;
                        }
                    } break;
            }
        }


        // < FindFormula_OfLOC_btn_Click > - функція підготовки для знаходження LOCоч за формулою
        private void FindFormula_OfLOC_btn_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Знаходження очікуваної кількості рядків коду за формулою...";
            Information_ProgramDo.Text = "Очікування...";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        InfoAboutParameters1_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutParameters2_SCC_label.Visibility = Visibility.Hidden;
                        GO_Next_SCC_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_SCC_btn.Visibility = Visibility.Hidden;
                        FindLOC_SCC_btn.Visibility = Visibility;
                        InfoAboutFind1_OfLOC_SCC_label.Visibility = Visibility;
                        InfoAboutFind2_OfLOC_SCC_label.Visibility = Visibility;
                        InfoAboutFind_OfLOC_one_SCC_label.Visibility = Visibility;
                        One_SCC_label.Visibility = Visibility;
                        LOC_better_SCC_label.Visibility = Visibility;
                        LOC_better_SCC_label.Text = "0";
                        LOC_worse_SCC_label.Visibility = Visibility;
                        LOC_worse_SCC_label.Text = "0";
                        LOC_probable_SCC_label.Visibility = Visibility;
                        LOC_probable_SCC_label.Text = "0";
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        GO_Next_CPT_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_CPT_btn.Visibility = Visibility.Hidden;
                        FindLOC_CPT_btn.Visibility = Visibility;
                        InfoAboutFind1_OfLOC_CPT_label.Visibility = Visibility;
                        InfoAboutFind2_OfLOC_CPT_label.Visibility = Visibility;
                        InfoAboutFind_OfLOC_one_CPT_label.Visibility = Visibility;
                        One_CPT_label.Visibility = Visibility;
                        LOC_better_CPT_label.Visibility = Visibility;
                        LOC_better_CPT_label.Text = "0";
                        LOC_worse_CPT_label.Visibility = Visibility;
                        LOC_worse_CPT_label.Text = "0";
                        LOC_probable_CPT_label.Visibility = Visibility;
                        LOC_probable_CPT_label.Text = "0";
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        GO_Next_CCC_btn.Visibility = Visibility.Hidden;
                        FindFormula_OfLOC_CCC_btn.Visibility = Visibility.Hidden;
                        FindLOC_CCC_btn.Visibility = Visibility;
                        InfoAboutFind1_OfLOC_CCC_label.Visibility = Visibility;
                        InfoAboutFind2_OfLOC_CCC_label.Visibility = Visibility;
                        InfoAboutFind_OfLOC_one_CCC_label.Visibility = Visibility;
                        One_CCC_label.Visibility = Visibility;
                        LOC_better_CCC_label.Visibility = Visibility;
                        LOC_better_CCC_label.Text = "0";
                        LOC_worse_CCC_label.Visibility = Visibility;
                        LOC_worse_CCC_label.Text = "0";
                        LOC_probable_CCC_label.Visibility = Visibility;
                        LOC_probable_CCC_label.Text = "0";
                    }
                    break;
            }
        }


        // < FindLOC_btn_Click > - функція знаходження LOCоч за формулою
        private void FindLOC_btn_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Очікувана кількість рядків коду успішно знайдена за формулою";
            Information_ProgramDo.Text = "Готово";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 7:  //-----------------------------------------------------------------SCC
                    {
                        FindFormula_OfLOC_SCC_btn.Visibility = Visibility;
                        GO_Next_SCC_btn.Visibility = Visibility;
                        InfoAboutParameters1_SCC_label.Visibility = Visibility;
                        InfoAboutParameters2_SCC_label.Visibility = Visibility;
                        FindLOC_SCC_btn.Visibility = Visibility.Hidden;
                        InfoAboutFind1_OfLOC_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind2_OfLOC_SCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind_OfLOC_one_SCC_label.Visibility = Visibility.Hidden;
                        One_SCC_label.Visibility = Visibility.Hidden;
                        LOC_better_SCC_label.Visibility = Visibility.Hidden;
                        LOC_worse_SCC_label.Visibility = Visibility.Hidden;
                        LOC_probable_SCC_label.Visibility = Visibility.Hidden;
                        double LOC1 = Convert.ToDouble(LOC_better_SCC_label.Text);
                        double LOC2 = Convert.ToDouble(LOC_worse_SCC_label.Text);
                        double LOC3 = Convert.ToDouble(LOC_probable_SCC_label.Text);
                        double LOC = sccMetric.FindLOC_forSomeFunction(LOC1, LOC2, LOC3);
                        CalculatInformationAboutThisMetric_SCC_TextBox.Text += "LOCкраще=" + LOC1 + "; LOCгірше=" + LOC2 + "; LOCімовірне=" + LOC3 + "; LOCоч =(" + LOC1 + "+" + LOC2 + "+ 4*" + LOC3 + ")/6= " + LOC + "\n";
                        var cel = DataGridHelper.GetCell(InputTableInfoParameters_SCC_dg, 1, 1);
                        var content = cel.Content as TextBlock;
                        var text = content.Text;
                        sccMetric.ChangeValue_OfParameter(1, double.Parse(text, CultureInfo.InvariantCulture));
                        sccMetric.ChangeValue_OfParameter(0, Convert.ToDouble(LOC));
                        InputTableInfoParameters_SCC_dg.Items.Refresh();
                    } break;
                case 9:  //-----------------------------------------------------------------CPT
                    {
                        FindFormula_OfLOC_CPT_btn.Visibility = Visibility;
                        GO_Next_CPT_btn.Visibility = Visibility;
                        FindLOC_CPT_btn.Visibility = Visibility.Hidden;
                        InfoAboutFind1_OfLOC_CPT_label.Visibility = Visibility.Hidden;
                        InfoAboutFind2_OfLOC_CPT_label.Visibility = Visibility.Hidden;
                        InfoAboutFind_OfLOC_one_CPT_label.Visibility = Visibility.Hidden;
                        One_CPT_label.Visibility = Visibility.Hidden;
                        LOC_better_CPT_label.Visibility = Visibility.Hidden;
                        LOC_worse_CPT_label.Visibility = Visibility.Hidden;
                        LOC_probable_CPT_label.Visibility = Visibility.Hidden;
                        double LOC1 = Convert.ToDouble(LOC_better_CPT_label.Text);
                        double LOC2 = Convert.ToDouble(LOC_worse_CPT_label.Text);
                        double LOC3 = Convert.ToDouble(LOC_probable_CPT_label.Text);
                        double LOC = cptMetric.FindLOC_forSomeFunction(LOC1, LOC2, LOC3);
                        CalculatInformationAboutThisMetric_CPT_TextBox.Text += "LOCкраще=" + LOC1 + "; LOCгірше=" + LOC2 + "; LOCімовірне=" + LOC3 + "; LOCоч =(" + LOC1 + "+" + LOC2 + "+ 4*" + LOC3 + ")/6= " + LOC + "\n";
                        var cel = DataGridHelper.GetCell(InputTableInfoParameters_CPT_dg, 1, 1);
                        var content = cel.Content as TextBlock;
                        var text = content.Text;
                        cptMetric.ChangeValue_OfParameter(1, double.Parse(text, CultureInfo.InvariantCulture));
                        cptMetric.ChangeValue_OfParameter(0, Convert.ToDouble(LOC));
                        InputTableInfoParameters_CPT_dg.Items.Refresh();
                    } break;
                case 10:  //-----------------------------------------------------------------CCC
                    {
                        FindFormula_OfLOC_CCC_btn.Visibility = Visibility;
                        GO_Next_CCC_btn.Visibility = Visibility;
                        FindLOC_CCC_btn.Visibility = Visibility.Hidden;
                        InfoAboutFind1_OfLOC_CCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind2_OfLOC_CCC_label.Visibility = Visibility.Hidden;
                        InfoAboutFind_OfLOC_one_CCC_label.Visibility = Visibility.Hidden;
                        One_CCC_label.Visibility = Visibility.Hidden;
                        LOC_better_CCC_label.Visibility = Visibility.Hidden;
                        LOC_worse_CCC_label.Visibility = Visibility.Hidden;
                        LOC_probable_CCC_label.Visibility = Visibility.Hidden;
                        double LOC1 = Convert.ToDouble(LOC_better_CCC_label.Text);
                        double LOC2 = Convert.ToDouble(LOC_worse_CCC_label.Text);
                        double LOC3 = Convert.ToDouble(LOC_probable_CCC_label.Text);
                        double LOC = cccMetric.FindLOC_forSomeFunction(LOC1, LOC2, LOC3);
                        CalculatInformationAboutThisMetric_CCC_TextBox.Text += "LOCкраще=" + LOC1 + "; LOCгірше=" + LOC2 + "; LOCімовірне=" + LOC3 + "; LOCоч =(" + LOC1 + "+" + LOC2 + "+ 4*" + LOC3 + ")/6= " + LOC + "\n";
                        var cel = DataGridHelper.GetCell(InputTableInfoParameters_CCC_dg, 1, 1);
                        var content = cel.Content as TextBlock;
                        var text = content.Text;
                        cccMetric.ChangeValue_OfParameter(1, double.Parse(text, CultureInfo.InvariantCulture));
                        cccMetric.ChangeValue_OfParameter(0, Convert.ToDouble(LOC));
                        InputTableInfoParameters_CCC_dg.Items.Refresh();
                    } break;
            }
        }


        //------------------------------------------------------------------------------------------------
        // ФУНКЦІЇ ДЛЯ МЕТРИКИ - FP для встановлення наявності вимог та визначення коефіцієнтів факторів, і тд.
        //------------------------------------------------------------------------------------------------
        // < SetAvailabilityOfSpecialRequirements_btn_Click > - функція встановлення наявності спеціальних вимог до проекту
        private void SetAvailabilityOfSpecialRequirements_btn_Click(object sender, RoutedEventArgs e)
        {
            if (NoneSpecialReq_FP_chbx.IsChecked == false && ExistSpecialReq_FP_chbx.IsChecked == false)
            {
                MessageBox.Show("Не обрано наявності спеціальних вимог до проекту! \n ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (NoneSpecialReq_FP_chbx.IsChecked == true)
                {
                    _specialRequirement = false;
                }
                if (ExistSpecialReq_FP_chbx.IsChecked == true)
                {
                    _specialRequirement = true;
                }
                Availability_OfSpecialReq_FP_label.Visibility = Visibility.Hidden;
                NoneSpecialReq_FP_chbx.Visibility = Visibility.Hidden;
                NoneSpecialReq_FP_chbx.IsChecked = false;
                ExistSpecialReq_FP_chbx.Visibility = Visibility.Hidden;
                ExistSpecialReq_FP_chbx.IsChecked = false;
                SetAvailability_OfSpecialRequirements_FP_button.Visibility = Visibility.Hidden;
                ChoiceOfFactors1_FP_label1.Visibility = Visibility;
                ChoiceOfFactors2_FP_label1.Visibility = Visibility;
                gridForArea_ChoiceComplexityOfFactors_scrollView.Visibility = Visibility;
                SetKoef_OfFactors_FP_button.Visibility = Visibility;
            }
        }

        // < SetInfo_Of14Characteristics_FP_btn_Click > - функція встановлення ваг для 14 загальних характеристик проекту
        private void SetInfo_Of14Characteristics_FP_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isGoodValue = true;
                for (int i = 1; i <= 14; i++)
                {
                    var cel = DataGridHelper.GetCell(InfoTableOf14Characteristic_FP_dg, 0, i);
                    var content = cel.Content as TextBlock;
                    var value = content.Text;
                    if (Convert.ToInt32(value.ToString()) > 5) isGoodValue = false;
                }
                if (isGoodValue == false)
                {
                    MessageBox.Show("Допустимі значення ваг виходять за межі! Діапазон допустимої ваги для характеристики становить інтервал від 0 до 5, тобто значення: 0,1,2,3,4,5! Спробуйте ще раз ввести дані! \n ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SetInfo_Of14Characteristics_FP_label.Visibility = Visibility.Hidden;
                    SetInfo_Of14Characteristics_FP_button.Visibility = Visibility.Hidden;
                    InfoTableOf14Characteristic_FP_dg.Visibility = Visibility.Hidden;
                    NewParametrs_FP_btn.Visibility = Visibility;
                    CleanParametrs_FP_btn.Visibility = Visibility;
                    OutputTableInfoParameters_FP_dg.Visibility = Visibility;
                    ImageResult_OfMetrica_FP.Visibility = Visibility;
                    ResultAnalysisMetric_FP_lb.Visibility = Visibility;
                    CalculationForMetric_FP_btn.Visibility = Visibility;

                    CalculatInformationAboutThisMetric_FP_TextBox.Text += "Sum(Fi)=";
                    for (int i = 1; i <= 14; i++)
                    {
                        var cel = DataGridHelper.GetCell(InfoTableOf14Characteristic_FP_dg, 0, i);
                        var content = cel.Content as TextBlock;
                        var value = content.Text;
                        listOfValueCharacteristics[i] = Convert.ToInt32(value.ToString());
                        CalculatInformationAboutThisMetric_FP_TextBox.Text += listOfValueCharacteristics[i];
                        if (i != 14) CalculatInformationAboutThisMetric_FP_TextBox.Text += "+";
                        if (i == 14) CalculatInformationAboutThisMetric_FP_TextBox.Text += " ;\n";
                    }

                    double sumFi = fpMetric.FindSumOfGeneralCharacteristics(listOfValueCharacteristics);
                    CalculatInformationAboutThisMetric_FP_TextBox.Text += "Sum(Fi)=" + sumFi + " - Сума загальних характеристик становить " + sumFi + ", оскільки існують спеціальні вимоги до проекту.\n";

                    CalculatInformationAboutThisMetric_FP_TextBox.Text += "FP(наближений)= ";
                    for (int i = 1; i <= fpMetric.CountOfFunction; i++)
                    {
                        CalculatInformationAboutThisMetric_FP_TextBox.Text += "FP(набл.)(" + i + ") ";
                        if (i != fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "+";
                        if (i == fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "=\n";
                    }
                    double totalSUM = 0;
                    for (int i = 1; i <= fpMetric.CountOfFunction; i++)
                    {
                        var cel = DataGridHelper.GetCell(OutputTableInfoParameters_FP_dg, i, 11);
                        var content = cel.Content as TextBlock;
                        var sum = content.Text;
                        totalSUM += double.Parse(sum, CultureInfo.InvariantCulture);
                        CalculatInformationAboutThisMetric_FP_TextBox.Text += sum;
                        if (i != fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "+";
                        if (i == fpMetric.CountOfFunction) CalculatInformationAboutThisMetric_FP_TextBox.Text += "= ";
                    }
                    CalculatInformationAboutThisMetric_FP_TextBox.Text += totalSUM + "\n";
                    CalculatInformationAboutThisMetric_FP_TextBox.Text += "Оскільки існують спеціальні вимоги до проекту, то ми повинні FP(набл.) збільшити на 1%\n";
                    double resultFP = fpMetric.FindMetric_FP(_specialRequirement,totalSUM,sumFi);
                    CalculatInformationAboutThisMetric_FP_TextBox.Text += "FP(уточн.) = FP(набл.) * [0,65 + 0,01*Sum(Fi)] = " + (totalSUM+(totalSUM*0.1)) + "*" + "[0,65 + 0,01*" + sumFi + "] = " + resultFP + "\n";
                    ResultAnalysisMetric_FP_lb.Content = resultFP;
                    DisplayResult_FP_lb.Visibility = Visibility;
                    DisplayResult_FP_lb.Content = "FP(уточн.) = FP(набл.) * [0, 65 + 0, 01 * Sum(Fi)] = \n" + (totalSUM + (totalSUM * 0.1)) + " * " + "[0, 65 + 0, 01 * " + sumFi + "] = " + resultFP;
                    Information_ProgramDo.Text = "Готово";
                    Result_OfAllMetric_TextBox.Text += "\n" + countOfOperationCalculation_OfMetric + "   Метрика пргн. функційного розміру                     " + resultFP + "                       " + DateTime.Now.ToString("hh:mm:ss tt");
                    Information_ProgramDo.Text = "Готово";
                    countOfOperationCalculation_OfMetric++;
                    countOfFunction_FP = 1;
                    TableResult[11].Value = resultFP;
                }
            }
            catch(Exception expt)
            {
                MessageBox.Show("Допустимі значення ваг виходять за межі! Спробуйте ще раз ввести дані! \n " + expt, "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // < SetAllKoeficientsOfFactors_btn_Click > - функція встановлення всіх відповідних коефіцієнтів для кожного фактора
        private void SetAllKoeficientsOfFactors_btn_Click(object sender, RoutedEventArgs e)
        {
            int selectedEI = this.EI_SetKoefOfFactor_comboBox.SelectedIndex;
            switch (selectedEI)
            {
                case 0: koefEI = 3;break;
                case 1: koefEI = 4; break;
                case 2: koefEI = 6; break;
            }
            //***************************************************
            int selectedEO = this.EO_SetKoefOfFactor_comboBox.SelectedIndex;
            switch (selectedEO)
            {
                case 0: koefEO = 4; break;
                case 1: koefEO = 5; break;
                case 2: koefEO = 7; break;
            }
            //***************************************************
            int selectedEIN = this.EIN_SetKoefOfFactor_comboBox.SelectedIndex;
            switch (selectedEIN)
            {
                case 0: koefEIN = 3; break;
                case 1: koefEIN = 4; break;
                case 2: koefEIN = 6; break;
            }
            //***************************************************
            int selectedILF = this.ILF_SetKoefOfFactor_comboBox.SelectedIndex;
            switch (selectedILF)
            {
                case 0: koefILF = 7; break;
                case 1: koefILF = 10; break;
                case 2: koefILF = 15; break;
            }
            //***************************************************
            int selectedELF = this.ELF_SetKoefOfFactor_comboBox.SelectedIndex;
            switch (selectedELF)
            {
                case 0: koefELF = 5; break;
                case 1: koefELF = 7; break;
                case 2: koefELF = 10; break;
            }
            ChoiceOfFactors1_FP_label1.Visibility = Visibility.Hidden;
            ChoiceOfFactors2_FP_label1.Visibility = Visibility.Hidden;
            gridForArea_ChoiceComplexityOfFactors_scrollView.Visibility = Visibility.Hidden;
            SetKoef_OfFactors_FP_button.Visibility = Visibility.Hidden;
            InfoForStepSetOfFunctions_FP_label.Visibility = Visibility;
            InputTableInfoParameters_FP_dg.Visibility = Visibility;
            GO_Next_FP_btn.Visibility = Visibility.Visible;
            InfoForStepSetOfFunctions_FP_label.Content = "Введіть необхідні дані для 1-ої функції";
            if (countOfFunction_FP == fpMetric.CountOfFunction)
            {
                GO_Next_FP_btn.Visibility = Visibility.Hidden;
                READY_FP_btn.Visibility = Visibility.Visible;
            }

            TableForFunction_FP newrowInfoFunction = new TableForFunction_FP()
            {
                Number = "Коефіцієнти",
                EI = (double)koefEI,
                EO = (double)koefEO,
                EIN = (double)koefEIN,
                ILF = (double)koefILF,
                ELF = (double)koefELF,
                TotalEI = 0,
                TotalEO = 0,
                TotalEIN = 0,
                TotalILF = 0,
                TotalELF = 0,
                Sum = 0
            };
            OutputTableInfoParameters_FP_dg.Items.Add(newrowInfoFunction);
            OutputTableInfoParameters_FP_dg.Items.Refresh();
        }


        // < EI_ShowInfo_button_Click > - функція розгортання інформації 
        private void EI_ShowInfo_button_Click(object sender, RoutedEventArgs e)
        {
            if (EI_INFO_showORhide == false)
            {
                EI_info.Text = "Кількість \nзовнішніх \nвходів";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(55);
                EI_ShowInfo_button.Content = "⇑";
                EI_INFO_showORhide = true;
            }
            else
            {
                EI_info.Text = "";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(15);
                EI_ShowInfo_button.Content = "⇓";
                EI_INFO_showORhide = false;
            }
        }
        //***************************************************
        private void EO_ShowInfo_button_Click(object sender, RoutedEventArgs e)
        {
            if (EO_INFO_showORhide == false)
            {
                EO_info.Text = "Кількість \nзовнішніх \nвиходів";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(55);
                EO_ShowInfo_button.Content = "⇑";
                EO_INFO_showORhide = true;
            }
            else
            {
                EO_info.Text = "";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(15);
                EO_ShowInfo_button.Content = "⇓";
                EO_INFO_showORhide = false;
            }
        }
        //***************************************************
        private void EIN_ShowInfo_button_Click(object sender, RoutedEventArgs e)
        {
            if (EIN_INFO_showORhide == false)
            {
                EIN_info.Text = "Кількість \nзовнішніх \nзапитів";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(55);
                EIN_ShowInfo_button.Content = "⇑";
                EIN_INFO_showORhide = true;
            }
            else
            {
                EIN_info.Text = "";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(15);
                EIN_ShowInfo_button.Content = "⇓";
                EIN_INFO_showORhide = false;
            }
        }
        //***************************************************
        private void ILF_ShowInfo_button_Click(object sender, RoutedEventArgs e)
        {
            if (ILF_INFO_showORhide == false)
            {
                ILF_info.Text = "Кількість \nвнутрішніх \nлог. файлів";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(55);
                ILF_ShowInfo_button.Content = "⇑";
                ILF_INFO_showORhide = true;
            }
            else
            {
                ILF_info.Text = "";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(15);
                ILF_ShowInfo_button.Content = "⇓";
                ILF_INFO_showORhide = false;
            }
        }
        //***************************************************
        private void ELF_ShowInfo_button_Click(object sender, RoutedEventArgs e)
        {
            if (ELF_INFO_showORhide == false)
            {
                ELF_info.Text = "Кількість \nзовнішніх \nлог. файлів";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(55);
                ELF_ShowInfo_button.Content = "⇑";
                ELF_INFO_showORhide = true;
            }
            else
            {
                ELF_info.Text = "";
                gridForArea_ChoiceComplexityOfFactors.RowDefinitions[2].Height = new GridLength(15);
                ELF_ShowInfo_button.Content = "⇓";
                ELF_INFO_showORhide = false;
            }
        }




        //------------------------------------------------------------------------------------------------
        // ФУНКЦІЇ ДЛЯ МЕТРИК - LC, DP для встановлення коефіцієнтів СОСОМО та інші налаштування анімаційних ефектів
        //------------------------------------------------------------------------------------------------
        // < SetKoefCOCOMO_btn_Click > - функція встановлення коефіцієнтів СОСОМО та подальший перехід на наступний етап
        private void SetKoefCOCOMO_btn_Click(object sender, RoutedEventArgs e)
        {
            TimeForDoIt_ProgressBar(10);
            Information_UserDo.Text = "Встановлення коефіцієнтів СОСОМО...";
            Information_ProgramDo.Text = "Очікування...";
            int selectedMetric = this.AllMetrics_TabControl.SelectedIndex;
            switch (selectedMetric)
            {
                case 12:  //-----------------------------------------------------------------LC
                    {
                        if(ForIndependent_LC_chbx.IsChecked == true)
                        {
                            lcMetric.SetAllParametersWithSelectedFactor_OfMetric(1);
                        }
                        if (ForEmbedded_LC_chbx.IsChecked == true)
                        {
                            lcMetric.SetAllParametersWithSelectedFactor_OfMetric(2);
                        }
                        if (ForIntermediate_LC_chbx.IsChecked == true)
                        {
                            lcMetric.SetAllParametersWithSelectedFactor_OfMetric(3);
                        }
                        if (ForIndependent_LC_chbx.IsChecked == true || ForEmbedded_LC_chbx.IsChecked == true || ForIntermediate_LC_chbx.IsChecked == true)
                        {
                            TableInfoParametrs_LC_dg.Items.Refresh();
                            SetKoefCOCOMO_LC_button.Visibility = Visibility.Hidden;
                            KoefCOCOMO_LC_label.Visibility = Visibility.Hidden;
                            ListKoef1_cmbx.Visibility = Visibility.Hidden;
                            ListKoef2_cmbx.Visibility = Visibility.Hidden;
                            ListKoef3_cmbx.Visibility = Visibility.Hidden;
                            ForIndependent_LC_chbx.Visibility = Visibility.Hidden;
                            ForEmbedded_LC_chbx.Visibility = Visibility.Hidden;
                            ForIntermediate_LC_chbx.Visibility = Visibility.Hidden;
                            TableInfoParametrs_LC_dg.Visibility = Visibility;
                            ResultAnalysisMetric_LC_image.Visibility = Visibility;
                            ResultAnalysisMetric_LC_lb.Visibility = Visibility;
                            CleanParametrs_LC_btn.Visibility = Visibility;
                            RefreshParametrs_LC_btn.Visibility = Visibility;
                            NewParametrs_LC_btn.Visibility = Visibility;
                        }
                        else
                        {
                            MessageBox.Show("Не обрано типу ПП! Оберіть тип програмного проекту (ПП) за яким обиратимуться коефіцієнти СОСОМО для знаходження метрики! \n ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
                case 13:  //-----------------------------------------------------------------DP
                    {
                        if (ForIndependent_DP_chbx.IsChecked == true)
                        {
                            dpMetric.SetAllParametersWithSelectedFactor_OfMetric(1);
                        }
                        if (ForEmbedded_DP_chbx.IsChecked == true)
                        {
                            dpMetric.SetAllParametersWithSelectedFactor_OfMetric(2);
                        }
                        if (ForIntermediate_DP_chbx.IsChecked == true)
                        {
                            dpMetric.SetAllParametersWithSelectedFactor_OfMetric(3);
                        }
                        if (ForIndependent_DP_chbx.IsChecked == true || ForEmbedded_DP_chbx.IsChecked == true || ForIntermediate_DP_chbx.IsChecked == true)
                        {
                            TableInfoParametrs_DP_dg.Items.Refresh();
                            SetKoefCOCOMO_DP_button.Visibility = Visibility.Hidden;
                            KoefCOCOMO_DP_label.Visibility = Visibility.Hidden;
                            ListKoef1_DP_cmbx.Visibility = Visibility.Hidden;
                            ListKoef2_DP_cmbx.Visibility = Visibility.Hidden;
                            ListKoef3_DP_cmbx.Visibility = Visibility.Hidden;
                            ForIndependent_DP_chbx.Visibility = Visibility.Hidden;
                            ForEmbedded_DP_chbx.Visibility = Visibility.Hidden;
                            ForIntermediate_DP_chbx.Visibility = Visibility.Hidden;
                            TableInfoParametrs_DP_dg.Visibility = Visibility;
                            ResultAnalysisMetric_DP_image.Visibility = Visibility;
                            ResultAnalysisMetric_DP_lb.Visibility = Visibility;
                            CleanParametrs_DP_btn.Visibility = Visibility;
                            RefreshParametrs_DP_btn.Visibility = Visibility;
                            NewParametrs_DP_btn.Visibility = Visibility;
                        }
                        else
                        {
                            MessageBox.Show("Не обрано типу ПП! Оберіть тип програмного проекту (ПП) за яким обиратимуться коефіцієнти СОСОМО для знаходження метрики! \n ", "Попередження:", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } break;
            }
        }



        //------------------------------------------------------------------------------------------------
        // ФУНКЦІЇ ДЛЯ ВИЗНАЧЕННЯ ОБРАНОЇ МЕТРИКИ ТА ІНФОРМУВАННЯ КОРИСТУВАЧА 
        //------------------------------------------------------------------------------------------------
        // < TreeView_findItem > - функція знаходження Items у TreeView для розпізнання обраної метрики
        public void TreeView_findItem()
        {
            item_Metric_exact_value = (TreeViewItem)ListOfAllMetrics_tv.Items[0]; //метрики з точними значеннями
            item_Metric_CHP = (TreeViewItem)item_Metric_exact_value.Items[0];
            item_Metric_CPP = (TreeViewItem)item_Metric_exact_value.Items[1];
            item_Metric_RUP = (TreeViewItem)item_Metric_exact_value.Items[2];
            item_Metric_MMT = (TreeViewItem)item_Metric_exact_value.Items[3];
            item_Metric_MBQ = (TreeViewItem)item_Metric_exact_value.Items[4];

            item_Metric_predicted_value = (TreeViewItem)ListOfAllMetrics_tv.Items[2]; //метрики з прогнозованими значеннями
            item_Metric_SCT = (TreeViewItem)item_Metric_predicted_value.Items[0];
            item_Metric_SDT = (TreeViewItem)item_Metric_predicted_value.Items[1];
            item_Metric_SCC = (TreeViewItem)item_Metric_predicted_value.Items[2];
            item_Metric_SQC = (TreeViewItem)item_Metric_predicted_value.Items[3];
            item_Metric_CPT = (TreeViewItem)item_Metric_predicted_value.Items[4];
            item_Metric_CCC = (TreeViewItem)item_Metric_predicted_value.Items[5];
            item_Metric_FP = (TreeViewItem)item_Metric_predicted_value.Items[6];
            item_Metric_LC = (TreeViewItem)item_Metric_predicted_value.Items[7];
            item_Metric_DP = (TreeViewItem)item_Metric_predicted_value.Items[8];
        }


        // < ListOfAllMetrics_SelectedItemChange > - функція для TreeViewItem, з метою синхронізації з TabControl та інформуванням кроків в StatusBar
        private void ListOfAllMetrics_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (item_Metric_CHP.IsSelected)
            {
                if(_statusBar_start_info == true)
                {
                    _statusBar_start_info = false;
                }
                else
                {
                    TimeForDoIt_ProgressBar(3);
                    Information_UserDo.Text = "Обрана метрика з точним значенням: Метрика зв'язності";
                    AllMetrics_TabControl.SelectedItem = Chp_TabItem;
                }
            }
            if(item_Metric_CPP.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з точним значенням: Метрика зчеплення";
                AllMetrics_TabControl.SelectedItem = Cpp;
            }
            if (item_Metric_RUP.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з точним значенням: Метрика звернення до глобальної змінної";
                AllMetrics_TabControl.SelectedItem = Rup;
            }
            if (item_Metric_MMT.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з точним значенням: Метрика часу модифікації моделей";
                AllMetrics_TabControl.SelectedItem = Mmt;
            }
            if (item_Metric_MBQ.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з точним значенням: Метрика загальної кількості знайдених помилок при інспектуванні моделей та ...";
                AllMetrics_TabControl.SelectedItem = Mbq;
            }

            if (item_Metric_SCT.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованого загального часу розроблення ПЗ";
                AllMetrics_TabControl.SelectedItem = Sct;
            }
            if (item_Metric_SDT.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика часу виконання робіт етапу проектування ПЗ";
                AllMetrics_TabControl.SelectedItem = Sdt;
            }
            if (item_Metric_SCC.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика очікуваної вартості розроблення ПЗ";
                AllMetrics_TabControl.SelectedItem = Scc;
            }
            if (item_Metric_SQC.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованої вартості перевірки якості ПЗ";
                AllMetrics_TabControl.SelectedItem = Sqc;
            }
            if (item_Metric_CPT.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованої продуктивності розроблення ПЗ";
                AllMetrics_TabControl.SelectedItem = Cpt;
            }
            if (item_Metric_CCC.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованих витрат на реалізацію програмного коду";
                AllMetrics_TabControl.SelectedItem = Ccc;
            }
            if (item_Metric_FP.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованого функційного розміру";
                AllMetrics_TabControl.SelectedItem = Fp;
            }
            if (item_Metric_LC.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованої оцінки трудовитрат за моделю Боема";
                AllMetrics_TabControl.SelectedItem = Lc;
            }
            if (item_Metric_DP.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                Information_UserDo.Text = "Обрана метрика з прогнозованим значенням: Метрика прогнозованої оцінки тривалості проекту за моделю Боема";
                AllMetrics_TabControl.SelectedItem = Dp;
            }
        }

        // < AllMetrics_TabControl_SelectionChanged > - функція для TabControl, з метою синхронізації з TreeViewItem та інформуванням кроків в StatusBar
        private void AllMetrics_TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Chp_TabItem.IsSelected)
            {
                if (_statusBar_start_info == true)
                {
                    _statusBar_start_info = false;
                }
                else
                {
                    TimeForDoIt_ProgressBar(3);
                    item_Metric_CHP.Focus();
                }
            }
            if (Cpp.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_CPP.Focus();
            }
            if (Rup.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_RUP.Focus();
            }
            if (Mmt.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_MMT.Focus();
            }
            if (Mbq.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_MBQ.Focus();
            }
            if (Sct.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_SCT.Focus();
            }
            if (Sdt.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_SDT.Focus();
            }
            if (Scc.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_SCC.Focus();
            }
            if (Sqc.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_SQC.Focus();
            }
            if (Cpt.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_CPT.Focus();
            }
            if (Ccc.IsSelected)
            {
                TimeForDoIt_ProgressBar(3);
                item_Metric_CCC.Focus();
            }
            //    if (Lc.IsSelected)
            //    {
            //        TimeForDoIt_ProgressBar(3);
            //        item_Metric_LC.Focus();
            //    }
            //    if (Dp.IsSelected)
            //    {
            //        TimeForDoIt_ProgressBar(3);
            //        item_Metric_DP.Focus();
            //    }
            //    if (Fp.IsSelected)
            //    {
            //        TimeForDoIt_ProgressBar(3);
            //        item_Metric_FP.Focus();
            //    }
            //    if (Analysis_OfAllMetric_ti.IsSelected)
            //    {
            //        Analyze_OfAllMetric_TextBox.Text = AnalyzeOfAllReceiveValue_OfMetrics();
            //    }
        }

        //------------------------------------------------------------------------------------------------
        //                          ФУНКЦІЇ ПРОЦЕСУ РОЗРОБКИ
        //------------------------------------------------------------------------------------------------
        // < TimeForDoIt_ProgressBar > - функція для ProgressBar, для анімаційного ефекту виконання завдання
        //  *int timeForExecute - час виконання
        private void TimeForDoIt_ProgressBar(int timeForExecute)
        {
            ProgresBar_TimeDoIt.BeginAnimation(ProgressBar.ValueProperty, null);
            Duration duration = new Duration(TimeSpan.FromSeconds(timeForExecute));
            DoubleAnimation doubleanimation = new DoubleAnimation(200.0, duration);
            ProgresBar_TimeDoIt.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }



        //------------------------------------------------------------------------------------------------
        //                          ФУНКЦІЇ ОБРОБНИКИ ПОДІЙ
        //------------------------------------------------------------------------------------------------
        // < ScrollViewerForThisTab_PreviewMouseWheel > - функція для роботи Mouse wheel для вертикальної прокрутки
        private void ScrollViewerForThisTab_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;

            if (e.Delta > 0)
            {
                scrollviewer.LineUp();
            }
            else
            {
                scrollviewer.LineDown();
            }
            e.Handled = true;           

        }

            // < ControlAllHotKey_KeyDown > - функція обробник гарячих клавіш
        private void ControlAllHotKey_KeyDown(object sender, KeyEventArgs e)
        {
            Key enterKey = e.Key;
            if (enterKey == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Save_all_Click(sender, e);
            }
            if (enterKey == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                New_forStart_Click(sender, e);
            }
            if (enterKey == Key.Delete && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Clean_allParameters_Click(sender, e);
            }
            if (enterKey == Key.R && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                Refresh_allParameters_Click(sender, e);
            }
            if (enterKey == Key.F && enterKey == Key.P && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                Search_parameter_Click(sender, e);
            }
            if (enterKey == Key.F && enterKey == Key.M && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                Search_metric_Click(sender, e);
            }
            if (enterKey == Key.A && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                View_allParameters_Click(sender, e);
            }
            if (enterKey == Key.M && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                View_allMetrics_Click(sender, e);
            }
            if (enterKey == Key.M && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                View_allMetrics_Click(sender, e);
            }
            if (enterKey == Key.F1)
            {
                Help_Click(sender, e);
            }
            if (enterKey == Key.F4)
            {
                Exit_Click(sender, e);
            }
            if (enterKey == Key.F6)
            {
                Settings_Click(sender, e);
            }
            if (enterKey == Key.F8)
            {
                Settings_Click(sender, e);
            }
            else
            { }
        }

        // < TableInfoParametrs_PreviewTextInput > - функція блокування вводу літер та символів для коректного вводу значень для параметрів (лише цифри)
        private void TableInfoParametrs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        // < TableInfoCharacteristic_PreviewTextInput > - функція блокування вводу літер, чисел від 6 до 9, та символів для коректного вводу ваг для 14 загальних характеристик проекту
        private void TableInfoCharacteristic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-5]+").IsMatch(e.Text);
        }
    }  
}

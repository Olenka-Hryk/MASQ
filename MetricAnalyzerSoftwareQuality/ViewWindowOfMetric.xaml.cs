using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Exact_Value;
using MetricAnalyzerSoftwareQuality.Class.Predicted_Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MetricAnalyzerSoftwareQuality
{
    public partial class ViewWindowOfMetric : Window
    {
        List<MyTableInfo_OfAllMetrics> allMetrics = new List<MyTableInfo_OfAllMetrics>(13);
        //--метрики з точним значенням
        CHPmetric chpMetric = new CHPmetric((MainWindow)App.MainWin);
        CPPmetric cppMetric = new CPPmetric((MainWindow)App.MainWin);
        RUPmetric rupMetric = new RUPmetric((MainWindow)App.MainWin);
        MMTmetric mmtMetric = new MMTmetric((MainWindow)App.MainWin);
        MBQmetric mbqMetric = new MBQmetric((MainWindow)App.MainWin);

        //--метрики з прогнозованим значення
        SCTmetric sctMetric = new SCTmetric((MainWindow)App.MainWin);
        SDTmetric sdtMetric = new SDTmetric((MainWindow)App.MainWin);
        SCCmetric sccMetric = new SCCmetric((MainWindow)App.MainWin);
        SQCmetric sqcMetric = new SQCmetric((MainWindow)App.MainWin);
        CPTmetric cptMetric = new CPTmetric((MainWindow)App.MainWin);
        CCCmetric cccMetric = new CCCmetric((MainWindow)App.MainWin);
        FPmetric fpMetric = new FPmetric((MainWindow)App.MainWin);
        LCmetric lcMetric = new LCmetric((MainWindow)App.MainWin);
        DPmetric dpMetric = new DPmetric((MainWindow)App.MainWin);

        public ViewWindowOfMetric()
        {
            InitializeComponent();

            titleBar.MouseLeftButtonDown += (o, e) => DragMove();
            //CHP
            chpMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric1 = new MyTableInfo_OfAllMetrics() { Number = 1, Name = chpMetric.Name, MIN = chpMetric.PermissibleMINvalue, MAX = chpMetric.PermisibleMAXvalue, Count = chpMetric.ParametersCount };
            allMetrics.Add(Metric1);
            //CPP
            cppMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric2 = new MyTableInfo_OfAllMetrics() { Number = 2, Name = cppMetric.Name, MIN = cppMetric.PermissibleMINvalue, MAX = cppMetric.PermisibleMAXvalue, Count = cppMetric.ParametersCount };
            allMetrics.Add(Metric2);
            //RUP
            rupMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric3 = new MyTableInfo_OfAllMetrics() { Number = 3, Name = rupMetric.Name, MIN = rupMetric.PermissibleMINvalue, MAX = rupMetric.PermisibleMAXvalue, Count = rupMetric.ParametersCount };
            allMetrics.Add(Metric3);
            //MMT
            mmtMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric4 = new MyTableInfo_OfAllMetrics() { Number = 4, Name = mmtMetric.Name, MIN = mmtMetric.PermissibleMINvalue, MAX = mmtMetric.PermisibleMAXvalue, Count = mmtMetric.ParametersCount };
            allMetrics.Add(Metric4);
            //MBQ
            mbqMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric5 = new MyTableInfo_OfAllMetrics() { Number = 5, Name = mbqMetric.Name, MIN = mbqMetric.PermissibleMINvalue, MAX = mbqMetric.PermisibleMAXvalue, Count = mbqMetric.ParametersCount };
            allMetrics.Add(Metric5);
            //SCT
            sctMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric6 = new MyTableInfo_OfAllMetrics() { Number = 6, Name = sctMetric.Name, MIN = sctMetric.PermissibleMINvalue, MAX = sctMetric.PermisibleMAXvalue, Count = sctMetric.ParametersCount };
            allMetrics.Add(Metric6);
            //SDT
            sdtMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric7 = new MyTableInfo_OfAllMetrics() { Number = 7, Name = sdtMetric.Name, MIN = sdtMetric.PermissibleMINvalue, MAX = sdtMetric.PermisibleMAXvalue, Count = sdtMetric.ParametersCount };
            allMetrics.Add(Metric7);
            //SCC
            sccMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric8 = new MyTableInfo_OfAllMetrics() { Number = 8, Name = sccMetric.Name, MIN = sccMetric.PermissibleMINvalue, MAX = sccMetric.PermisibleMAXvalue, Count = sccMetric.ParametersCount };
            allMetrics.Add(Metric8);
            //SQC
            sqcMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric9 = new MyTableInfo_OfAllMetrics() { Number = 9, Name = sqcMetric.Name, MIN = sqcMetric.PermissibleMINvalue, MAX = sqcMetric.PermisibleMAXvalue, Count = sqcMetric.ParametersCount };
            allMetrics.Add(Metric9);
            //CPT
            cptMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric10 = new MyTableInfo_OfAllMetrics() { Number = 10, Name = cptMetric.Name, MIN = cptMetric.PermissibleMINvalue, MAX = cptMetric.PermisibleMAXvalue, Count = cptMetric.ParametersCount };
            allMetrics.Add(Metric10);
            //CCC
            cccMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric11 = new MyTableInfo_OfAllMetrics() { Number = 11, Name = cccMetric.Name, MIN = cccMetric.PermissibleMINvalue, MAX = cccMetric.PermisibleMAXvalue, Count = cccMetric.ParametersCount };
            allMetrics.Add(Metric11);
            //FP
            fpMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric12 = new MyTableInfo_OfAllMetrics() { Number = 12, Name = fpMetric.Name, MIN = fpMetric.PermissibleMINvalue, MAX = fpMetric.PermisibleMAXvalue, Count = fpMetric.ParametersCount };
            allMetrics.Add(Metric12);
            //LC
            lcMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric13 = new MyTableInfo_OfAllMetrics() { Number = 13, Name = lcMetric.Name, MIN = lcMetric.PermissibleMINvalue, MAX = lcMetric.PermisibleMAXvalue, Count = lcMetric.ParametersCount };
            allMetrics.Add(Metric13);
            //DP
            dpMetric.SetInformation_OfMetric();
            MyTableInfo_OfAllMetrics Metric14 = new MyTableInfo_OfAllMetrics() { Number = 14, Name = dpMetric.Name, MIN = dpMetric.PermissibleMINvalue, MAX = dpMetric.PermisibleMAXvalue, Count = dpMetric.ParametersCount };
            allMetrics.Add(Metric14);

            AllMetricsInfo_dataGrid.ItemsSource = allMetrics;
            AllMetricsInfo_dataGrid.Items.Refresh();

        }

        // < CloseViewMetrics_Button_Click > - закриття вікна, функція-обробник кнопки "ЗАКРИТИ"
        private void CloseViewMetrics_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

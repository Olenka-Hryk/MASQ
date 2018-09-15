using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Exact_Value;
using MetricAnalyzerSoftwareQuality.Class.Predicted_Value;
using MetricAnalyzerSoftwareQuality.Class.Table;
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
    public partial class ViewWindowOfParameter : Window
    {
        List<MyTableInfo_OfAllParameters> allParameters = new List<MyTableInfo_OfAllParameters>();

        //--метрики з точним значенням
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

        public ViewWindowOfParameter()
        {
            InitializeComponent();
            titleBar.MouseLeftButtonDown += (o, e) => DragMove();
            
            //RUP
            rupMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter1 = new MyTableInfo_OfAllParameters() { Number = 1, Name = rupMetric.GetNameOfParameter(0) };
            allParameters.Add(Parameter1);
            MyTableInfo_OfAllParameters Parameter2 = new MyTableInfo_OfAllParameters() { Number = 2, Name = rupMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter2);
            //MMT
            mmtMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter3 = new MyTableInfo_OfAllParameters() { Number = 3, Name = mmtMetric.GetNameOfParameter(0) };
            allParameters.Add(Parameter3);
            MyTableInfo_OfAllParameters Parameter4 = new MyTableInfo_OfAllParameters() { Number = 4, Name = mmtMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter4);
            MyTableInfo_OfAllParameters Parameter5 = new MyTableInfo_OfAllParameters() { Number = 5, Name = mmtMetric.GetNameOfParameter(2) };
            allParameters.Add(Parameter5);
            //MBQ
            mbqMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter6 = new MyTableInfo_OfAllParameters() { Number = 6, Name = mbqMetric.GetNameOfParameter(0) };
            allParameters.Add(Parameter6);
            MyTableInfo_OfAllParameters Parameter7 = new MyTableInfo_OfAllParameters() { Number = 7, Name = mbqMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter7);
            //SCC
            sccMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter8 = new MyTableInfo_OfAllParameters() { Number = 8, Name = sccMetric.GetNameOfParameter(0) };
            allParameters.Add(Parameter8);
            MyTableInfo_OfAllParameters Parameter9 = new MyTableInfo_OfAllParameters() { Number = 9, Name = sccMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter9);
            //SQC
            sqcMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter10 = new MyTableInfo_OfAllParameters() { Number = 10, Name = sqcMetric.GetNameOfParameter(0) };
            allParameters.Add(Parameter10);
            MyTableInfo_OfAllParameters Parameter11 = new MyTableInfo_OfAllParameters() { Number = 11, Name = sqcMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter11);
            //CPT
            cptMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter12 = new MyTableInfo_OfAllParameters() { Number = 12, Name = cptMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter12);
            MyTableInfo_OfAllParameters Parameter13 = new MyTableInfo_OfAllParameters() { Number = 13, Name = cptMetric.GetNameOfParameter(2) };
            allParameters.Add(Parameter13);
            //CCC
            cccMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter14 = new MyTableInfo_OfAllParameters() { Number = 14, Name = cccMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter14);
            //FP
            fpMetric.SetAllParametersWithDefaultValue_OfMetric();
            MyTableInfo_OfAllParameters Parameter15 = new MyTableInfo_OfAllParameters() { Number = 15, Name = fpMetric.GetNameOfParameter(0) };
            allParameters.Add(Parameter15);
            MyTableInfo_OfAllParameters Parameter16 = new MyTableInfo_OfAllParameters() { Number = 16, Name = fpMetric.GetNameOfParameter(1) };
            allParameters.Add(Parameter16);
            MyTableInfo_OfAllParameters Parameter17 = new MyTableInfo_OfAllParameters() { Number = 17, Name = fpMetric.GetNameOfParameter(2) };
            allParameters.Add(Parameter17);
            MyTableInfo_OfAllParameters Parameter18 = new MyTableInfo_OfAllParameters() { Number = 18, Name = fpMetric.GetNameOfParameter(3) };
            allParameters.Add(Parameter18);
            MyTableInfo_OfAllParameters Parameter19 = new MyTableInfo_OfAllParameters() { Number = 19, Name = fpMetric.GetNameOfParameter(4) };
            allParameters.Add(Parameter19);

            AllParametersInfo_dataGrid.ItemsSource = allParameters;
            AllParametersInfo_dataGrid.Items.Refresh();
        }

        // < CloseViewParameters_Button_Click > - закриття вікна, функція-обробник кнопки "ЗАКРИТИ"
        private void CloseViewParameters_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

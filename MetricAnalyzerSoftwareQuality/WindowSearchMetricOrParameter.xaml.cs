using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Exact_Value;
using MetricAnalyzerSoftwareQuality.Class.Predicted_Value;
using System;
using System.Collections.Generic;
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

namespace MetricAnalyzerSoftwareQuality
{
    public partial class WindowSearchMetricOrParameter : Window
    {
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
        MainWindow mainWindow = new MainWindow();

        public WindowSearchMetricOrParameter()
        {
            InitializeComponent();
            titleBar.MouseLeftButtonDown += (o, e) => DragMove();

            chpMetric.SetInformation_OfMetric();
            cppMetric.SetInformation_OfMetric();
            rupMetric.SetInformation_OfMetric();
            mmtMetric.SetInformation_OfMetric();
            mbqMetric.SetInformation_OfMetric();
            sctMetric.SetInformation_OfMetric();
            sdtMetric.SetInformation_OfMetric();
            sccMetric.SetInformation_OfMetric();
            sqcMetric.SetInformation_OfMetric();
            cptMetric.SetInformation_OfMetric();
            cccMetric.SetInformation_OfMetric();
            fpMetric.SetInformation_OfMetric();
            lcMetric.SetInformation_OfMetric();
            dpMetric.SetInformation_OfMetric();

            rupMetric.SetAllParametersWithDefaultValue_OfMetric();
            mmtMetric.SetAllParametersWithDefaultValue_OfMetric();
            mbqMetric.SetAllParametersWithDefaultValue_OfMetric();
            sctMetric.SetAllParametersWithDefaultValue_OfMetric();
            sdtMetric.SetAllParametersWithDefaultValue_OfMetric();
            sccMetric.SetAllParametersWithDefaultValue_OfMetric();
            sqcMetric.SetAllParametersWithDefaultValue_OfMetric();
            cptMetric.SetAllParametersWithDefaultValue_OfMetric();
            cccMetric.SetAllParametersWithDefaultValue_OfMetric();
            fpMetric.SetAllParametersWithDefaultValue_OfMetric();
            lcMetric.SetAllParametersWithDefaultValue_OfMetric();
            dpMetric.SetAllParametersWithDefaultValue_OfMetric();
        }


        // < SearchMetric_Button_Click > - пошук метрики
        private void SearchMetric_Button_Click(object sender, RoutedEventArgs e)
        {
            List<String> listMetrics = new List<String>() { "","","", "", "", "", "", "", "", "", "", "", "", ""};
            int i = -1;
            String searchOfMetric = InputSearchOfMetric_txtbx.Text;
            if (Regex.IsMatch(chpMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = chpMetric.Name;
            }
           if (Regex.IsMatch(cppMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = cppMetric.Name;
            }
           if (Regex.IsMatch(rupMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = rupMetric.Name;
            }
           if (Regex.IsMatch(mmtMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = mmtMetric.Name;
            }
           if (Regex.IsMatch(mbqMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = mbqMetric.Name;
            }
           if (Regex.IsMatch(sctMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = sctMetric.Name;
            }
           if (Regex.IsMatch(sdtMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = sdtMetric.Name;
            }
           if (Regex.IsMatch(sccMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = sccMetric.Name;
            }
           if (Regex.IsMatch(sqcMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = sqcMetric.Name;
            }
           if (Regex.IsMatch(cptMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = cptMetric.Name;
            }
           if (Regex.IsMatch(cccMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = cccMetric.Name;
            }
           if (Regex.IsMatch(fpMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = fpMetric.Name;
            }
           if (Regex.IsMatch(lcMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = lcMetric.Name;
            }
           if (Regex.IsMatch(dpMetric.Name, searchOfMetric) == true)
            {
                ++i;
                listMetrics[i] = dpMetric.Name;
            }

            if (i == -1)
            {
                MessageBox.Show("Не знайдено метрики!", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                String ListOfAllFindMetric = "";
                for(int j=0;j<listMetrics.Count;j++)
                {
                    ListOfAllFindMetric += "\n"+listMetrics[j];
                }
                MessageBox.Show("Знайдено метрики: "+ ListOfAllFindMetric, "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        // < SearchParameter_Button_Click > - пошук параметра
        private void SearchParameter_Button_Click(object sender, RoutedEventArgs e)
        {
            List<String> listMetrics = new List<String>();
            List<String> listParameters = new List<String>();
            int i = -1;
            String searchOfParameter = InputSearchOfParameter_txtbx.Text;
            if (Regex.IsMatch(rupMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(rupMetric.Name);
                listParameters.Add(rupMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(rupMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(rupMetric.Name);
                listParameters.Add(rupMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(mmtMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(mmtMetric.Name);
                listParameters.Add(mmtMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(mmtMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(mmtMetric.Name);
                listParameters.Add(mmtMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(mmtMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(mmtMetric.Name);
                listParameters.Add(mmtMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(mbqMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(mbqMetric.Name);
                listParameters.Add(mbqMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(mbqMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(mbqMetric.Name);
                listParameters.Add(mbqMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(sctMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sctMetric.Name);
                listParameters.Add(sctMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(sctMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sctMetric.Name);
                listParameters.Add(sctMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(sdtMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sdtMetric.Name);
                listParameters.Add(sdtMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(sdtMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sdtMetric.Name);
                listParameters.Add(sdtMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(sdtMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sdtMetric.Name);
                listParameters.Add(sdtMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(sccMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sccMetric.Name);
                listParameters.Add(sccMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(sccMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sccMetric.Name);
                listParameters.Add(sccMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(sqcMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sqcMetric.Name);
                listParameters.Add(sqcMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(sqcMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sqcMetric.Name);
                listParameters.Add(sqcMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(sqcMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sqcMetric.Name);
                listParameters.Add(sqcMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(sqcMetric.GetNameOfParameter(3), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(sqcMetric.Name);
                listParameters.Add(sqcMetric.GetNameOfParameter(3));
            }
            if (Regex.IsMatch(cptMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(cptMetric.Name);
                listParameters.Add(cptMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(cptMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(cptMetric.Name);
                listParameters.Add(cptMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(cptMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(cptMetric.Name);
                listParameters.Add(cptMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(cccMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(cccMetric.Name);
                listParameters.Add(cccMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(cccMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(cccMetric.Name);
                listParameters.Add(cccMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(lcMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(lcMetric.Name);
                listParameters.Add(lcMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(lcMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(lcMetric.Name);
                listParameters.Add(lcMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(lcMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(lcMetric.Name);
                listParameters.Add(lcMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(dpMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(dpMetric.Name);
                listParameters.Add(dpMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(dpMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(dpMetric.Name);
                listParameters.Add(dpMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(dpMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(dpMetric.Name);
                listParameters.Add(dpMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(dpMetric.GetNameOfParameter(3), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(dpMetric.Name);
                listParameters.Add(dpMetric.GetNameOfParameter(3));
            }
            if (Regex.IsMatch(dpMetric.GetNameOfParameter(4), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(dpMetric.Name);
                listParameters.Add(dpMetric.GetNameOfParameter(4));
            }
            if (Regex.IsMatch(fpMetric.GetNameOfParameter(0), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(fpMetric.Name);
                listParameters.Add(fpMetric.GetNameOfParameter(0));
            }
            if (Regex.IsMatch(fpMetric.GetNameOfParameter(1), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(fpMetric.Name);
                listParameters.Add(fpMetric.GetNameOfParameter(1));
            }
            if (Regex.IsMatch(fpMetric.GetNameOfParameter(2), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(fpMetric.Name);
                listParameters.Add(fpMetric.GetNameOfParameter(2));
            }
            if (Regex.IsMatch(fpMetric.GetNameOfParameter(3), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(fpMetric.Name);
                listParameters.Add(fpMetric.GetNameOfParameter(3));
            }
            if (Regex.IsMatch(fpMetric.GetNameOfParameter(4), searchOfParameter) == true)
            {
                ++i;
                listMetrics.Add(fpMetric.Name);
                listParameters.Add(fpMetric.GetNameOfParameter(4));
            }

            if (i == -1)
            {
                MessageBox.Show("Не знайдено параметра!", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                String ListOfAllFindMetric = "";
                for (int j = 0; j < listMetrics.Count; j++)
                {
                    ListOfAllFindMetric += "\n(" + listMetrics[j] + ")->" + listParameters[j];
                }
                MessageBox.Show("Знайдено параметри: " + ListOfAllFindMetric, "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        // < CancelSearchMetric_Button_Click > - закриття вікна пошуку метрики, функція-обробник кнопки "СКАСУВАТИ"
        private void CancelSearchMetric_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // < CancelSearchParameter_Button_Click > - закриття вікна пошуку параметра, функція-обробник кнопки "СКАСУВАТИ"
        private void CancelSearchParameter_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

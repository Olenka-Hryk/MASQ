using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetricAnalyzerSoftwareQuality.Class.Predicted_Value
{
    public class FPmetric : MetricsQualitySoftware
    {
        public int CountOfFunction;
        private MainWindow _mainWindow;

        public struct TableForFunction_FP
        {
            public string Number { get; set; }            // номер функції
            public double EI { get; set; }                // кількість зовнішніх входів
            public double EO { get; set; }                // кількість зовнішніх виходів    
            public double EIN { get; set; }               // кількість зовнішніх запитів
            public double ILF { get; set; }               // кількість вн. логічних файлів або унікальних логічних груп
            public double ELF { get; set; }               // кількість звн. логічних файлів або унікальних логічних груп    
            public double TotalEI { get; set; }           // к-сть*коеф.EI
            public double TotalEO { get; set; }           // к-сть*коеф.EO
            public double TotalEIN { get; set; }          // к-сть*коеф.EIN
            public double TotalILF { get; set; }          // к-сть*коеф.ILF
            public double TotalELF { get; set; }          // к-сть*коеф.ELF
            public double Sum { get; set; }               // загальна сума; к-сть*коеф.EI + к-сть*коеф.EO + к-сть*коеф.EIN + к-сть*коеф.ILF + к-сть*коеф.ELF;
        }

        public FPmetric() : base()
        {
            Console.WriteLine();
        }
        public FPmetric(String name, String description, int permissibleMIN, int permissibleMAXvalue, int parametersCount, int countOfFunction, MainWindow win) : base(name, description, permissibleMIN, permissibleMAXvalue, parametersCount)
        {
            this.CountOfFunction = countOfFunction;
            this._mainWindow = win;
        }
        public FPmetric(MainWindow win) : base()
        {
            this._mainWindow = win;
        }

        public override void SetInformation_OfMetric()
        {
            this.Name = "Метрика прогнозування функційного розміру";
            this.Description = "Вимірює суть можливостей майбутньої програми. Для обчислення функційного розміру ідентифікуються \nочікувані від програмного додатку функції за критеріями International Function Point Users Group.";
            this.PermissibleMINvalue = -1;
            this.PermisibleMAXvalue = 2945;
            this.ParametersCount = 5;
            this.CountOfFunction = 1;
        }

        public override void SetAllParametersWithDefaultValue_OfMetric()
        {
            Parameter firstParameter = new Parameter() { Number = 1, Name = "EI  -  Кількість зовнішніх входів функції, які по-різному впливають на виконувану функцію", Value = 567 };
            Metric.Add(firstParameter);
            Parameter secondParameter = new Parameter() { Number = 2, Name = "EO  - Кількість зовнішніх виходів функції, для істотно різних алгоритмів і нетривіальної функційності", Value = 78 };
            Metric.Add(secondParameter);
            Parameter thirdParameter = new Parameter() { Number = 3, Name = "EIN - Кількість зовнішніх запитів", Value = 340 };
            Metric.Add(thirdParameter);
            Parameter fourthParameter = new Parameter() { Number = 4, Name = "ILF  - Кількість внутрішніх логічних файлів або унікальних логічних груп користувацьких даних", Value = 5 };
            Metric.Add(fourthParameter);
            Parameter fifthParameter = new Parameter() { Number = 5, Name = "ELF - Кількість зовнішніх логічних файлів або унікальних логічних груп користувацьких даних", Value = 8 };
            Metric.Add(fifthParameter);

            this._mainWindow.InputTableInfoParameters_FP_dg.ItemsSource = Metric;
        }

        public override String ShowDescription_OfMetric()
        {
            return " " + Name + ": \n" + Description;
        }

        public override String ShowInformationOfParameters_and_SolutionOfMetric()
        {
            return "\n" + Metric[0].GetName() + "\n" 
                   + Metric[1].GetName() + "\n" 
                   + Metric[2].GetName() + "\n" 
                   + Metric[3].GetName() + "\n" 
                   + Metric[4].GetName() + "\n" +
                   "FP(уточн.) = FP(набл.) * [0,65 + 0,01*Sum(Fi)]  FPє[" + PermissibleMINvalue + ";0..." + PermisibleMAXvalue + "] \n";
        }

        public String ShowInformationOfParameters_and_SolutionOfMetric_ForFunction(int numberFunction, int koefEI, int koefEO, int koefEIN, int koefILF, int koefELF)    //вивід інформації по кожній функції 
        {
            return "--->>>> ФУНКЦІЯ " + numberFunction + " :\n" +
                   "EI = " + Metric[0].GetValue() + "; Koef.EI = " + koefEI + "; TotalEI=EI*Koef.EI= " + (Metric[0].GetValue() * koefEI) + ";\n" +
                   "EO = " + Metric[1].GetValue() + "; Koef.EO = "+ koefEO+"; TotalEO=EO*Koef.EO= "+(Metric[1].GetValue()*koefEO) +";\n" +
                   "EIN = " + Metric[2].GetValue() + "; Koef.EIN = " + koefEIN + "; TotalEIN=EIN*Koef.EIN= " + (Metric[2].GetValue() * koefEIN) + ";\n" +
                   "ILF = " + Metric[3].GetValue() + "; Koef.ILF = " + koefILF + "; TotalILF=ILF*Koef.ILF= " + (Metric[3].GetValue() * koefILF) + ";\n" +
                   "ELF = " + Metric[4].GetValue() + "; Koef.ELF = " + koefELF + "; TotalELF=ELF*Koef.ELF= " + (Metric[4].GetValue() * koefELF) + ";\n" +
                   "FP(наближений)(" + numberFunction + ") = TotalEI + TotalEO + TotalEIN + TotalILF + TotalELF = " + FindApproximateFP(koefEI, koefEO, koefEIN, koefILF, koefELF) + "\n\n";
        }

        public override double FindMetric()
        {
            return 0;
        }

        public override void ClearAllParameters_OfMetric()
        {
            MessageBox.Show("Значення всіх параметрів метрики успішно очищено! ", "Інформація:", MessageBoxButton.OK, MessageBoxImage.Information);
            this._mainWindow.OutputTableInfoParameters_FP_dg.Items.Refresh();
        }


        public double FindApproximateFP(int koefEI, int koefEO, int koefEIN, int koefILF, int koefELF)
        {
            return ((koefEI* Metric[0].GetValue()) + (koefEO* Metric[1].GetValue()) + (koefEIN* Metric[2].GetValue()) + (koefILF* Metric[3].GetValue()) + (koefELF* Metric[4].GetValue()));
        }

        public double FindSumOfGeneralCharacteristics(List<int> characteristic)
        {
            int result = 0;
            for (int i=1; i<=14; i++)
            {
                result += characteristic[i];
            }
            return result;
        }

        public double FindMetric_FP(bool isSpecialRequirement, double ApproximateFP_ofAllFunctions, double Sum_OfAllCharacteristics)
        {
            try
            {
                if (isSpecialRequirement == false)
                {
                    double percentage = ApproximateFP_ofAllFunctions * 0.35;
                    double new_ApproximateFP_ofAllFunctions = ApproximateFP_ofAllFunctions - percentage;
                    double result = new_ApproximateFP_ofAllFunctions * (0.65 + (0.01 * Sum_OfAllCharacteristics));
                    if ((result <= PermisibleMAXvalue) && (result >= PermissibleMINvalue))
                    {
                        int intRes = (int)(result * 1000);
                        result = (double)intRes / 1000.0;
                        return result;
                    }
                    else
                    {
                        MessageBox.Show("Некоректні вхідні дані, адже значення метрики (FP = " + result + " ) виходить за допустимі межі [-1,0..2945]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                        return 0;
                    }
                }
                else
                {
                    double percentage = ApproximateFP_ofAllFunctions * 0.1;
                    double new_ApproximateFP_ofAllFunctions = ApproximateFP_ofAllFunctions + percentage;
                    double result = new_ApproximateFP_ofAllFunctions * (0.65 + (0.01 * Sum_OfAllCharacteristics));
                    if ((result <= PermisibleMAXvalue) && (result >= PermissibleMINvalue))
                    {
                        int intRes = (int)(result * 1000);
                        result = (double)intRes / 1000.0;
                        return result;
                    }
                    else
                    {
                        MessageBox.Show("Некоректні вхідні дані, адже значення метрики (FP = " + result + " ) виходить за допустимі межі [-1,0..2945]! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n ", "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Невірні вхідні дані! Будь ласка, ретельно перевірте введені параметри та попробуйте ще раз! \n " + e, "Помилка:", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
    }
}

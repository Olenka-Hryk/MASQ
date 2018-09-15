using System;
using System.Collections.Generic;
using MetricAnalyzerSoftwareQuality;
using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Predicted_Value;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static MetricAnalyzerSoftwareQuality.MetricsQualitySoftware;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest_MetricFP
    {
        [TestMethod]
        public void FP_FindApproximateFP_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            FPmetric metric_FP = new FPmetric(mainWindow);
            metric_FP.SetInformation_OfMetric();
            metric_FP.SetAllParametersWithDefaultValue_OfMetric();
            metric_FP.ChangeValue_OfParameter(0, 56);
            metric_FP.ChangeValue_OfParameter(1, 32);
            metric_FP.ChangeValue_OfParameter(2,13);
            metric_FP.ChangeValue_OfParameter(3,1);
            metric_FP.ChangeValue_OfParameter(4, 0);

            // Act
            double actual = metric_FP.FindApproximateFP(3,5,2,1,8);

            // Assert
            Assert.AreEqual(355, actual);
        }

        [TestMethod]
        public void FP_FindSumOfGeneralCharacteristics_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            FPmetric metric_FP = new FPmetric(mainWindow);
            List<int> listEl = new List<int>();
            for (int i = 0; i <15; i++)
            {
                listEl.Add(1);
            }

            // Act
            double actual = metric_FP.FindSumOfGeneralCharacteristics(listEl);

            // Assert
            Assert.AreEqual(14, actual);
        }
    }
}
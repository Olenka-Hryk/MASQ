using System;
using System.Collections.Generic;
using MetricAnalyzerSoftwareQuality;
using MetricAnalyzerSoftwareQuality.Class;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static MetricAnalyzerSoftwareQuality.MetricsQualitySoftware;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest_MetricRUP
    {
        [TestMethod]
        public void RUP_SetInformation_OfMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            RUPmetric metric_RUP = new RUPmetric(mainWindow);
            metric_RUP.SetInformation_OfMetric();

            // Act
            string name = metric_RUP.Name;
            double max = metric_RUP.PermisibleMAXvalue;
            double min = metric_RUP.PermissibleMINvalue;
            int countParam = metric_RUP.ParametersCount;

            // Assert
            Assert.AreEqual("Метрика звертання до глобальних змінних", name);
            Assert.AreEqual(1, max);
            Assert.AreEqual(0, min);
            Assert.AreEqual(2, countParam);
        }

        [TestMethod]
        public void RUP_findMetric_TestMethod()
        {
            var mainWindow = new MainWindow();

            // Arrange
            RUPmetric metric_RUP = new RUPmetric(mainWindow);
            metric_RUP.SetInformation_OfMetric();
            metric_RUP.SetAllParametersWithDefaultValue_OfMetric();
            metric_RUP.ChangeValue_OfParameter(0, 3);
            metric_RUP.ChangeValue_OfParameter(1, 6);
            
            // Act
            double result = metric_RUP.FindMetric();

            // Assert
            Assert.AreEqual(0.5, result);
        }
    }
}

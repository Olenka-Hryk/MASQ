using System;
using System.Collections.Generic;
using MetricAnalyzerSoftwareQuality;
using MetricAnalyzerSoftwareQuality.Class;
using MetricAnalyzerSoftwareQuality.Class.Exact_Value;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static MetricAnalyzerSoftwareQuality.MetricsQualitySoftware;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest_MetricMBQ
    {
        [TestMethod]
        public void MBQ_SetInformation_OfMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            MBQmetric metric_MBQ = new MBQmetric(mainWindow);
            metric_MBQ.SetInformation_OfMetric();

            // Act
            string name = metric_MBQ.Name;
            double max = metric_MBQ.PermisibleMAXvalue;
            double min = metric_MBQ.PermissibleMINvalue;
            int countParam = metric_MBQ.ParametersCount;

            // Assert
            Assert.AreEqual("Метрика загальної кількості знайдених помилок при інспектуванні моделей та прототипів модулів", name);
            Assert.AreEqual(5000, max);
            Assert.AreEqual(-1, min);
            Assert.AreEqual(2, countParam);
        }

        [TestMethod]
        public void MBQ_findMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            MBQmetric metric_MBQ = new MBQmetric(mainWindow);
            metric_MBQ.SetInformation_OfMetric();
            metric_MBQ.SetAllParametersWithDefaultValue_OfMetric();
            metric_MBQ.ChangeValue_OfParameter(0, 143);
            metric_MBQ.ChangeValue_OfParameter(1, 65);

            // Act
            double result = metric_MBQ.FindMetric();

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}

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
    public class UnitTest_MetricMMT
    {
        [TestMethod]
        public void MMT_SetInformation_OfMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            MMTmetric metric_MMT = new MMTmetric(mainWindow);
            metric_MMT.SetInformation_OfMetric();

            // Act
            string name = metric_MMT.Name;
            double max = metric_MMT.PermisibleMAXvalue;
            double min = metric_MMT.PermissibleMINvalue;
            int countParam = metric_MMT.ParametersCount;

            // Assert
            Assert.AreEqual("Метрика часу модифікації моделей", name);
            Assert.AreEqual(46, max);
            Assert.AreEqual(-1, min);
            Assert.AreEqual(3, countParam);
        }

        [TestMethod]
        public void MMT_findMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            MMTmetric metric_MMT = new MMTmetric(mainWindow);
            metric_MMT.SetInformation_OfMetric();
            metric_MMT.SetAllParametersWithDefaultValue_OfMetric();
            metric_MMT.ChangeValue_OfParameter(0, 6743);
            metric_MMT.ChangeValue_OfParameter(1, 125);
            metric_MMT.ChangeValue_OfParameter(2, 4);

            // Act
            double result = metric_MMT.FindMetric();

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}

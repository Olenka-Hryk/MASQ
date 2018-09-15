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
    public class UnitTest_MetricSCC
    {
        [TestMethod]
        public void SCC_ShowDescription_OfMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            SCCmetric metric_SCC = new SCCmetric(mainWindow);
            metric_SCC.SetInformation_OfMetric();

            // Act
            string actual = metric_SCC.ShowDescription_OfMetric();
            string expected = " Метрика очікуваної вартості розроблення ПЗ: \nОчікувана вартість процесу розроблення і-ої функції: \nB(i) = LOC(оч i)*ВАРТІСТЬ(рядка і),   і=1,m";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SCC_findMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            SCCmetric metric_SCC = new SCCmetric(mainWindow);
            metric_SCC.SetInformation_OfMetric();
            metric_SCC.SetAllParametersWithDefaultValue_OfMetric();
            metric_SCC.ChangeValue_OfParameter(0, 1450);
            metric_SCC.ChangeValue_OfParameter(1, 6);

            // Act
            double actual = metric_SCC.FindMetric();

            // Assert
            Assert.AreEqual(8700, actual);
        }
    }
}

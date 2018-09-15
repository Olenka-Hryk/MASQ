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
    public class UnitTest_MetricDP
    {
        [TestMethod]
        public void DP_FindMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            DPmetric metric_DP = new DPmetric(mainWindow);
            metric_DP.SetInformation_OfMetric();
            metric_DP.SetAllParametersWithSelectedFactor_OfMetric(2);
            metric_DP.ChangeValue_OfParameter(0, 4);

            // Act
            double actual = metric_DP.FindMetric();
            double expected = 6.414;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

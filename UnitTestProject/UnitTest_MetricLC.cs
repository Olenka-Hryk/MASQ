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
    public class UnitTest_MetricLC
    {
        [TestMethod]
        public void LC_FindMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            LCmetric metric_LC = new LCmetric(mainWindow);
            metric_LC.SetInformation_OfMetric();
            metric_LC.SetAllParametersWithSelectedFactor_OfMetric(1);
            metric_LC.ChangeValue_OfParameter(0, 3);

            // Act
            double actual = metric_LC.FindMetric();
            double expected = 7.606;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

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
    public class UnitTest_Metrics
    {
        [TestMethod]
        public void AbstractClass_FindLOC_forSomeFunction_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            CPTmetric metric_CPT = new CPTmetric(mainWindow);

            // Act
            double actual = metric_CPT.FindLOC_forSomeFunction(8, 6, 1);

            // Assert
            Assert.AreEqual(3, actual);
        }

        [TestMethod]
        public void AbstractClass_GetNameOfParameter_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            DPmetric metric_DP = new DPmetric(mainWindow);
            metric_DP.SetInformation_OfMetric();
            metric_DP.SetAllParametersWithDefaultValue_OfMetric();

            // Act
            string actual = metric_DP.GetNameOfParameter(3);

            // Assert
            Assert.AreEqual("Коефіцієнт СОСОМО (c)", actual);
        }
    }
}
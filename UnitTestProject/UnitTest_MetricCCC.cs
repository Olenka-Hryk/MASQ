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
    public class UnitTest_MetricCCC
    {
        [TestMethod]
        public void CCC_SetInformation_OfMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            CCCmetric metric_CCC = new CCCmetric(mainWindow);
            metric_CCC.SetInformation_OfMetric();

            // Act
            string name = metric_CCC.Name;
            double max = metric_CCC.PermisibleMAXvalue;
            double min = metric_CCC.PermissibleMINvalue;
            int countParam = metric_CCC.ParametersCount;

            // Assert
            Assert.AreEqual("Метрика прогнозування витрат на реалізацію програмного коду", name);
            Assert.AreEqual(70000, max);
            Assert.AreEqual(-1, min);
            Assert.AreEqual(2, countParam);
        }

        [TestMethod]
        public void CCC_findMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            CCCmetric metric_CCC = new CCCmetric(mainWindow);
            metric_CCC.SetInformation_OfMetric();
            metric_CCC.SetAllParametersWithDefaultValue_OfMetric();
            metric_CCC.ChangeValue_OfParameter(0, 8743);
            metric_CCC.ChangeValue_OfParameter(1, 7653);

            // Act
            double result = metric_CCC.FindMetric();

            // Assert
            Assert.AreEqual(1.142, result);
        }
    }
}

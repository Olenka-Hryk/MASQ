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
    public class UnitTest_MetricCPT
    {
        [TestMethod]
        public void CPT_SetInformation_OfMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            CPTmetric metric_CPT = new CPTmetric(mainWindow);
            metric_CPT.SetInformation_OfMetric();

            // Act
            string name = metric_CPT.Name;
            double max = metric_CPT.PermisibleMAXvalue;
            double min = metric_CPT.PermissibleMINvalue;
            int countParam = metric_CPT.ParametersCount;

            // Assert
            Assert.AreEqual("Метрика прогнозування продуктивності розроблення ПЗ", name);
            Assert.AreEqual(5, max);
            Assert.AreEqual(-1, min);
            Assert.AreNotEqual(4, countParam);
        }

        [TestMethod]
        public void CPT_findMetric_TestMethod()
        {
            // Arrange
            var mainWindow = new MainWindow();
            CPTmetric metric_CPT = new CPTmetric(mainWindow);
            metric_CPT.SetInformation_OfMetric();
            metric_CPT.SetAllParametersWithDefaultValue_OfMetric();
            metric_CPT.CountOfFunction = 1;
            metric_CPT.ChangeValue_OfParameter(0, 34);
            metric_CPT.ChangeValue_OfParameter(1, 23);
            metric_CPT.ChangeValue_OfParameter(2, 1);

            // Act
            double result = metric_CPT.FindMetric();

            // Assert
            Assert.AreEqual(0.676, result);
        }
    }
}

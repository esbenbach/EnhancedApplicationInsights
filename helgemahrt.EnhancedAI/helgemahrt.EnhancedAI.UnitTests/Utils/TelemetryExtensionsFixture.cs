using helgemahrt.EnhancedAI.Utils;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace helgemahrt.EnhancedAI.UnitTests.Utils
{
    [TestClass]
    public class TelemetryExtensionsFixture
    {
        [TestMethod]
        public void TestGetNameOrMessage_EventTelemetry()
        {
            // arrange
            string expected = "TestName";
            EventTelemetry telemetry = new EventTelemetry(expected);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_TraceTelemetry()
        {
            // arrange
            string expected = "TestName";
            TraceTelemetry telemetry = new TraceTelemetry(expected);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_PageViewTelemetry()
        {
            // arrange
            string expected = "TestName";
            PageViewTelemetry telemetry = new PageViewTelemetry(expected);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_ExceptionTelemetry()
        {
            // arrange
            string expected = "TestName";
            Exception ex = new Exception(expected);
            ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_MetricTelemetry()
        {
            // arrange
            string expected = "TestName";
            MetricTelemetry telemetry = new MetricTelemetry(expected, 0);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_RequestTelemetry()
        {
            // arrange
            string expected = "TestName";
            RequestTelemetry telemetry = new RequestTelemetry(expected, DateTimeOffset.Now, TimeSpan.Zero, "", true);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_DependencyTelemetry()
        {
            // arrange
            string expected = "TestName";
            DependencyTelemetry telemetry = new DependencyTelemetry("", "", expected, "");

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNameOrMessage_AvailabilityTelemetry()
        {
            // arrange
            string expected = "TestName";
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry(expected, DateTimeOffset.Now, TimeSpan.Zero, "", true);

            // act
            string actual = telemetry.GetNameOrMessage();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_EventTelemetry()
        {
            // arrange
            string expected = "Event";
            EventTelemetry telemetry = new EventTelemetry(expected);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_PageViewTelemetry()
        {
            // arrange
            string expected = "PageView";
            PageViewTelemetry telemetry = new PageViewTelemetry(expected);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_TraceTelemetry()
        {
            // arrange
            string expected = "Trace";
            TraceTelemetry telemetry = new TraceTelemetry(expected);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_ExceptionTelemetry()
        {
            // arrange
            string expected = "Exception";
            Exception ex = new Exception(expected);
            ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_MetricTelemetry()
        {
            // arrange
            string expected = "Metric";
            MetricTelemetry telemetry = new MetricTelemetry(expected, 0);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_RequestTelemetry()
        {
            // arrange
            string expected = "Request";
            RequestTelemetry telemetry = new RequestTelemetry(expected, DateTimeOffset.Now, TimeSpan.Zero, "", true);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_DependencyTelemetry()
        {
            // arrange
            string expected = "Dependency";
            DependencyTelemetry telemetry = new DependencyTelemetry("", "", expected, "");

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTelemetryTypePrefix_AvailabilityTelemetry()
        {
            // arrange
            string expected = "Availability";
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry(expected, DateTimeOffset.Now, TimeSpan.Zero, "", true);

            // act
            string actual = telemetry.GetTelemetryTypePrefix();

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}

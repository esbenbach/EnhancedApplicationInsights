using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using helgemahrt.EnhancedAI.TelemetryProcessors;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.ApplicationInsights.Fakes;
using Microsoft.ApplicationInsights.DataContracts;

namespace helgemahrt.EnhancedAI.UnitTests.TelemetryProcessors
{
    [TestClass]
    public class TelemetryToMetricsFixture
    {
        [TestMethod]
        public void TestSetupBuffers_Works()
        {
            using (ShimsContext.Create())
            {
                // arrange
                int expectedEvent = 1;
                int expectedTrace = 10;
                int expectedMetric = 10;
                int actualEvent = 0;
                int actualTrace = 0;
                int actualMetric = 0;
                TelemetryToMetrics sut = new TelemetryToMetrics(new MockTransmissionProcessor((x) =>
                {
                    if (x is EventTelemetry)
                    {
                        ++actualEvent;
                    }
                    if (x is TraceTelemetry)
                    {
                        ++actualTrace;
                    }
                    if (x is MetricTelemetry)
                    {
                        ++actualMetric;
                    }
                }));
                sut.TelemetryTypesToTrack = "EventTelemetry,ExceptionTelemetry,MetricTelemetry";

                EventTelemetry item1 = new EventTelemetry("event1");
                item1.Properties["key1"] = "value1";
                item1.Properties["key2"] = "value2";
                item1.Properties["key3"] = "value3";
                item1.Properties["key4"] = "value4";

                TraceTelemetry item2 = new TraceTelemetry("event1");
                item2.Properties["key2"] = "value2";
                item2.Properties["key4"] = "value4";
                item2.Properties["key3"] = "value3";
                item2.Properties["key5"] = "value1";

                MetricTelemetry metric = new MetricTelemetry("metric1", 0);

                ShimTelemetryClient.AllInstances.TrackMetricStringDoubleIDictionaryOfStringString = (client, name, value, dict) =>
                {
                };

                // act
                for (int i = 0; i < 10; ++i)
                {
                    sut.Process(item1);
                    sut.Process(item2);
                    sut.Process(metric);
                }

                // assert
                Assert.AreEqual(expectedEvent, actualEvent);
                Assert.AreEqual(expectedTrace, actualTrace);
                Assert.AreEqual(expectedMetric, actualMetric);
            }
        }

        [TestMethod]
        public void TestSetupBuffers_Works_ExoticConfigValues()
        {
            using (ShimsContext.Create())
            {
                // arrange
                int expectedEvent = 1;
                int expectedTrace = 10;
                int actualEvent = 0;
                int actualTrace = 0;
                TelemetryToMetrics sut = new TelemetryToMetrics(new MockTransmissionProcessor((x) =>
                {
                    if (x is EventTelemetry)
                    {
                        ++actualEvent;
                    }
                    if (x is TraceTelemetry)
                    {
                        ++actualTrace;
                    }
                }));
                sut.TelemetryTypesToTrack = "EventTelemetry,ExceptionTelemetry,MtrixTelmtry,Microsoft.ApplicationInsights.DataContracts.EventTelemetry";
                sut.TelemetryTypesToTrack = null;

                EventTelemetry item1 = new EventTelemetry("event1");
                item1.Properties["key1"] = "value1";
                item1.Properties["key2"] = "value2";
                item1.Properties["key3"] = "value3";
                item1.Properties["key4"] = "value4";

                TraceTelemetry item2 = new TraceTelemetry("event1");
                item2.Properties["key2"] = "value2";
                item2.Properties["key4"] = "value4";
                item2.Properties["key3"] = "value3";
                item2.Properties["key5"] = "value1";

                ShimTelemetryClient.AllInstances.TrackMetricStringDoubleIDictionaryOfStringString = (client, name, value, dict) =>
                {
                };

                // act
                for (int i = 0; i < 10; ++i)
                {
                    sut.Process(item1);
                    sut.Process(item2);
                }

                // assert
                Assert.AreEqual(expectedEvent, actualEvent);
                Assert.AreEqual(expectedTrace, actualTrace);
            }
        }
    }
}

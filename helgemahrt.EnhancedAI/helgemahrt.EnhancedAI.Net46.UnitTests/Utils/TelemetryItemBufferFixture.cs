using helgemahrt.EnhancedAI.UnitTests.TelemetryProcessors;
using helgemahrt.EnhancedAI.Utils;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace helgemahrt.EnhancedAI.UnitTests.Utils
{
    [TestClass]
    public class TelemetryItemBufferFixture
    {
        [TestMethod]
        public void TestCountTelemetry_DetectsDuplicateEvents()
        {
            // arrange
            TelemetryItemBuffer sut = new TelemetryItemBuffer();

            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key3"] = "value3";
            item2.Properties["key1"] = "value1";

            bool expected = false;

            // act
            sut.CountTelemetry(item1);
            bool actual = sut.CountTelemetry(item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCountTelemetry_DetectsUniqueDifference()
        {
            // arrange
            TelemetryItemBuffer sut = new TelemetryItemBuffer();

            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key3"] = "value3";
            item2.Properties["key5"] = "value1";

            bool expected = true;

            // act
            sut.CountTelemetry(item1);
            bool actual = sut.CountTelemetry(item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCountTelemetry_CountsEventsRight()
        {
            // arrange
            TelemetryItemBuffer sut = new TelemetryItemBuffer();

            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key3"] = "value3";
            item2.Properties["key5"] = "value1";

            double expected = 100;
            double actual = 0;

            // act
            for (int i = 0; i < 50; ++i)
            {
                sut.CountTelemetry(item1);
                sut.CountTelemetry(item2);
            }
            sut.SendMetrics(new MockTransmissionProcessor((x) => 
            {
                MetricTelemetry metric = x as MetricTelemetry;
                if (metric != null)
                {
                    if (string.Equals("event1", metric.Name))
                    {
                        actual = metric.Value;
                    }
                }
            }));

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCountTelemetry_CountsEventsRightWithPrefix()
        {
            // arrange
            TelemetryItemBuffer sut = new TelemetryItemBuffer();
            sut.PrefixMetricsWithType = true;

            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key3"] = "value3";
            item2.Properties["key5"] = "value1";

            double expected = 100;
            double actual = 0;

            // act
            for (int i = 0; i < 50; ++i)
            {
                sut.CountTelemetry(item1);
                sut.CountTelemetry(item2);
            }
            sut.SendMetrics(new MockTransmissionProcessor((x) => 
            {
                MetricTelemetry metric = x as MetricTelemetry;
                if (metric != null)
                {
                    if (string.Equals("Event.event1", metric.Name))
                    {
                        actual = metric.Value;
                    }
                }
            }));

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}

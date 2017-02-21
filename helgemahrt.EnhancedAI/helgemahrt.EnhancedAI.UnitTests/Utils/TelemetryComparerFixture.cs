using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using helgemahrt.EnhancedAI.Utils;
using Microsoft.ApplicationInsights.DataContracts;

namespace helgemahrt.EnhancedAI.UnitTests.Utils
{
    [TestClass]
    public class TelemetryComparerFixture
    {
        TelemetryComparer sut = new TelemetryComparer();

        [TestMethod]
        public void TestEquals_SameItems()
        {
            // arrange
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

            bool expected = true;

            // act
            bool actual = sut.Equals(item1, item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEquals_DifferentTypes()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            TraceTelemetry item2 = new TraceTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key3"] = "value3";
            item2.Properties["key1"] = "value1";

            bool expected = false;

            // act
            bool actual = sut.Equals(item1, item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEquals_DifferentNames()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event2");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key3"] = "value3";
            item2.Properties["key1"] = "value1";

            bool expected = false;

            // act
            bool actual = sut.Equals(item1, item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEquals_DifferentPropertyValues()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value3";
            item2.Properties["key3"] = "value3";
            item2.Properties["key1"] = "value1";

            bool expected = false;

            // act
            bool actual = sut.Equals(item1, item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEquals_DifferentPropertyCount()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key1"] = "value1";

            bool expected = false;

            // act
            bool actual = sut.Equals(item1, item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEquals_DifferentProperties()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key1"] = "value1";
            item2.Properties["key5"] = "value3";

            bool expected = false;

            // act
            bool actual = sut.Equals(item1, item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetHash_SameHash()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key1"] = "value1";
            item2.Properties["key3"] = "value3";

            int expected = sut.GetHashCode(item1);

            // act
            int actual = sut.GetHashCode(item2);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetHash_DifferentHash_Type()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            TraceTelemetry item2 = new TraceTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key1"] = "value1";
            item2.Properties["key3"] = "value3";

            int notExpected = sut.GetHashCode(item1);

            // act
            int actual = sut.GetHashCode(item2);

            // assert
            Assert.AreNotEqual(notExpected, actual);
        }

        [TestMethod]
        public void TestGetHash_DifferentHash_Name()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event2");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key1"] = "value1";
            item2.Properties["key3"] = "value3";

            int notExpected = sut.GetHashCode(item1);

            // act
            int actual = sut.GetHashCode(item2);

            // assert
            Assert.AreNotEqual(notExpected, actual);
        }

        [TestMethod]
        public void TestGetHash_DifferentHash_PropertyCount()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value4";
            item2.Properties["key1"] = "value1";

            int notExpected = sut.GetHashCode(item1);

            // act
            int actual = sut.GetHashCode(item2);

            // assert
            Assert.AreNotEqual(notExpected, actual);
        }

        [TestMethod]
        public void TestGetHash_DifferentHash_PropertyValues()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key4"] = "value3";
            item2.Properties["key1"] = "value1";
            item2.Properties["key3"] = "value3";

            int notExpected = sut.GetHashCode(item1);

            // act
            int actual = sut.GetHashCode(item2);

            // assert
            Assert.AreNotEqual(notExpected, actual);
        }

        [TestMethod]
        public void TestGetHash_DifferentHash_PropertyKeys()
        {
            // arrange
            EventTelemetry item1 = new EventTelemetry("event1");
            item1.Properties["key1"] = "value1";
            item1.Properties["key2"] = "value2";
            item1.Properties["key3"] = "value3";
            item1.Properties["key4"] = "value4";

            EventTelemetry item2 = new EventTelemetry("event1");
            item2.Properties["key2"] = "value2";
            item2.Properties["key5"] = "value4";
            item2.Properties["key1"] = "value1";
            item2.Properties["key3"] = "value3";

            int notExpected = sut.GetHashCode(item1);

            // act
            int actual = sut.GetHashCode(item2);

            // assert
            Assert.AreNotEqual(notExpected, actual);
        }
    }
}

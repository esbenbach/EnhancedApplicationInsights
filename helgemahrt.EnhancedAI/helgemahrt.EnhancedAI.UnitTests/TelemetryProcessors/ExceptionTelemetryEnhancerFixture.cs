using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using helgemahrt.EnhancedAI.TelemetryProcessors;
using Microsoft.QualityTools.Testing.Fakes;

namespace helgemahrt.EnhancedAI.UnitTests.TelemetryProcessors
{
    class CustomException : Exception
    {
        public string CustomProperty { get; set; }

        public CustomException(string message, string customProperty) : base(message)
        {
            CustomProperty = customProperty;
        }
    }

    [TestClass]
    public class ExceptionTelemetryEnhancerFixture
    {
        [TestMethod]
        public void TestProcess()
        {
            using (ShimsContext.Create())
            {
                // arrange
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(null);
                CustomException ex = new CustomException("Exception Message", "Important Property");


                // act

                // assert
            }
        }
    }
}

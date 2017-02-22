using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace helgemahrt.EnhancedAI.UnitTests.TelemetryProcessors
{
    public class MockTransmissionProcessor : ITelemetryProcessor
    {
        Action<ITelemetry> _action;

        public MockTransmissionProcessor(Action<ITelemetry> X)
        {
            _action = X;
        }

        public void Process(ITelemetry item)
        {
            _action(item);
        }
    }
}

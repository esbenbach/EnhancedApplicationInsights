using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace helgemahrt.EnhancedAI.Utils
{
    public static class TelemetryExtensions
    {
        public static string GetNameOrMessage(this ITelemetry telemetry)
        {
            if (telemetry is EventTelemetry)
            {
                return (telemetry as EventTelemetry).Name;
            }
            if (telemetry is MetricTelemetry)
            {
                return (telemetry as MetricTelemetry).Name;
            }
            if (telemetry is RequestTelemetry)
            {
                return (telemetry as RequestTelemetry).Name;
            }
            if (telemetry is DependencyTelemetry)
            {
                return (telemetry as DependencyTelemetry).Name;
            }
            if (telemetry is AvailabilityTelemetry)
            {
                return (telemetry as AvailabilityTelemetry).Name;
            }
            if (telemetry is PageViewTelemetry)
            {
                return (telemetry as PageViewTelemetry).Name;
            }
            if (telemetry is TraceTelemetry)
            {
                return (telemetry as TraceTelemetry).Message;
            }
            if (telemetry is ExceptionTelemetry)
            {
                return (telemetry as ExceptionTelemetry).Message;
            }

            return string.Empty;
        }

        public static string GetTelemetryTypePrefix(this ITelemetry telemetry)
        {
            if (telemetry is EventTelemetry)
            {
                return "Event";
            }
            if (telemetry is MetricTelemetry)
            {
                return "Metric";
            }
            if (telemetry is RequestTelemetry)
            {
                return "Request";
            }
            if (telemetry is DependencyTelemetry)
            {
                return "Dependency";
            }
            if (telemetry is AvailabilityTelemetry)
            {
                return "Availability";
            }
            if (telemetry is PageViewTelemetry)
            {
                return "PageView";
            }
            if (telemetry is TraceTelemetry)
            {
                return "Trace";
            }
            if (telemetry is ExceptionTelemetry)
            {
                return "Exception";
            }

            return string.Empty;
        }
    }
}

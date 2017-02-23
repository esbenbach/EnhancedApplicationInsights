using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace helgemahrt.EnhancedAI.Utils
{
    public class TelemetryItemBuffer
    {
        /// <summary>
        /// Holds the list of unique telemetry items we've seen already. There is no thread-safe HashSet so we have to use a ConcurrentDictionary as a workaround.
        /// This dictionary is used to keep track of the different telemetry items we've seen already, so we know which to send off to AI. (Taking into account
        /// the name/message + the properties)
        /// </summary>
        protected ConcurrentDictionary<ITelemetry, byte> _uniqueTelemetryItems = new ConcurrentDictionary<ITelemetry, byte>(new TelemetryComparer());

        /// <summary>
        /// Holds the number of times a name / message has been reported since the last time we sent metric data off to Application Insights. 
        /// We need a seconds dictionary because for the metrics we don't care about the properties, we just care about the message which appears
        /// on top of the graph in the Application Insights dashboard.
        /// </summary>
        protected ConcurrentDictionary<string, int> _telemetryMetrics = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// Add a prefix to each reported metric to identify which type the message belonged to. E.g. Event.[name] Default is false.
        /// </summary>
        public bool PrefixMetricsWithType { get; set; } = false;

        // the telemetry client we use to send the metrics off to AI
        private TelemetryClient _client;

        public TelemetryItemBuffer(string instrumentationKey)
        {
            _client = new TelemetryClient()
            {
                InstrumentationKey = instrumentationKey
            };
        }

        /// <summary>
        /// Counts the telemetry item.
        /// </summary>
        /// <param name="telemetry"></param>
        /// <returns>Returns true if this is a new telemetry item which we haven't seen before, which should be sent to AI. False if it should not be sent.</returns>
        public bool CountTelemetry(ITelemetry telemetry)
        {
            bool result = true;

            // first check whether we know this item already
            if (_uniqueTelemetryItems.ContainsKey(telemetry))
            {
                // we do, no need to send it to AI again
                result = false;
            }
            else
            {
                // we don't; remember it for next time
                _uniqueTelemetryItems[telemetry] = 1;
            }

            // now count the message (which will appear on the graph)
            string prefix = string.Empty;

            if (PrefixMetricsWithType)
            {
                prefix = $"{telemetry.GetTelemetryTypePrefix()}.";
            }

            if (_telemetryMetrics.ContainsKey($"{prefix}{telemetry.GetNameOrMessage()}"))
            {
                ++_telemetryMetrics[$"{prefix}{telemetry.GetNameOrMessage()}"];
            }
            else
            {
                _telemetryMetrics[$"{prefix}{telemetry.GetNameOrMessage()}"] = 1;
            }

            return result;
        }

        /// <summary>
        /// Sends the metrics data collected to AI and resets the statistics.
        /// </summary>
        public void SendMetrics()
        {
            foreach (KeyValuePair<string, int> kv in _telemetryMetrics)
            {
                // send the data
                _client.TrackMetric(kv.Key, kv.Value);

                // reset the counter
                _telemetryMetrics[kv.Key] = 0;
            }
        }
    }
}

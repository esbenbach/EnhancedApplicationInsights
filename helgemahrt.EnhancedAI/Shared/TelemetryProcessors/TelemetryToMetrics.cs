using helgemahrt.EnhancedAI.Utils;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace helgemahrt.EnhancedAI.TelemetryProcessors
{
    public class TelemetryToMetrics : ITelemetryProcessor
    {
        // the timer used for sending updates to AI
        private Timer _sendTimer;

        // the next telemetry processor in the chain
        private ITelemetryProcessor _next;

        // buffers for telemetry data; we'll have a set of buffers per instrumentation key
        // which equals to one set of buffers per instance of Application Insights
        private ConcurrentDictionary<string, ConcurrentDictionary<Type, TelemetryItemBuffer>> _telemetryBuffers = new ConcurrentDictionary<string, ConcurrentDictionary<Type, TelemetryItemBuffer>>();

        // telemtry types we're tracking
        private List<Type> _typesToTrack = new List<Type>();

        /// <summary>
        /// The interval at which metrics are sent to Application Insights, in seconds.
        /// </summary>
        public int SendInterval
        {
            get { return _sendInterval; }
            set
            {
                _sendInterval = value;

                ResetTimer();
            }
        }
        private int _sendInterval = 60;

        /// <summary>
        /// Add a prefix to each reported metric to identify which type the message belonged to. E.g. Event.[name] Default is false.
        /// </summary>
        public bool PrefixMetricsWithType
        {
            get { return _prefixMetricsWithType; }
            set
            {
                _prefixMetricsWithType = value;

                foreach (var ai in _telemetryBuffers.Values)
                {
                    foreach (var buffer in ai.Values)
                    {
                        buffer.PrefixMetricsWithType = value;
                    }
                }
            }
        }
        private bool _prefixMetricsWithType = false;

        public string TelemetryTypesToTrack
        {
            get { return _telemetryTypesToTrack; }
            set
            {
                _telemetryTypesToTrack = value;

                ClearBuffers();
            }
        }
        private string _telemetryTypesToTrack = null;

        public TelemetryToMetrics(ITelemetryProcessor next)
        {
            // Link processors to each other in a chain.
            _next = next;

            ResetTimer();
        }

        /// <summary>
        /// Stop the send timer.
        /// </summary>
        private void CleanupTimer()
        {
            if (_sendTimer != null)
            {
                _sendTimer.Elapsed -= SendMetrics;
                _sendTimer.Stop();
                _sendTimer.Dispose();
                _sendTimer = null;
            }
        }

        /// <summary>
        /// Stop the send timer and start it with the new interval.
        /// </summary>
        private void ResetTimer()
        {
            CleanupTimer();

            _sendTimer = new Timer(1000 * SendInterval);
            _sendTimer.Elapsed += SendMetrics;
            _sendTimer.AutoReset = true;
            _sendTimer.Enabled = true;
        }

        /// <summary>
        /// Send the metrics to Application Insights.
        /// </summary>
        private void SendMetrics(object sender, ElapsedEventArgs e)
        {
            // for all AI instances
            foreach (var ai in _telemetryBuffers.Values)
            {
                // get all buffers
                foreach (var buffer in ai.Values)
                {
                    // and send all metrics
                    buffer.SendMetrics();
                }
            }
        }

        private void ClearBuffers()
        {
            if (string.IsNullOrEmpty(TelemetryTypesToTrack))
            {
                // do nothing
                return;
            }

            // tear down the old buffers, if there are any
            _telemetryBuffers.Clear();
            UpdateTypesToTrack();
        }

        private void UpdateTypesToTrack()
        {
            // types to track is expected to be a list of comma-separated values
            string[] types = TelemetryTypesToTrack.Split(',');
            foreach (string type in types)
            {
                try
                {
                    // make sure we only get the last part after the dot, if there are any
                    string name = type;
                    if (name.Contains("."))
                    {
                        name = name.Substring(type.LastIndexOf('.'));
                    }

                    // then add the namespace of the telemetry data contracts
                    string fullName = $"Microsoft.ApplicationInsights.DataContracts.{name}, Microsoft.ApplicationInsights";
                    Type telemetryType = Type.GetType(fullName);
                    if (telemetryType != null)
                    {
                        if (telemetryType == typeof(MetricTelemetry))
                        {
                            // ignore metrics telemetry to avoid an infinite loop
                            // TODO: maybe subclass MetricTelemetry to avoid this
                            continue;
                        }

                        _typesToTrack.Add(telemetryType);
                    }
                }
                catch (Exception ex)
                {
                    // something went wrong, no metrics for this one
                    Debug.WriteLine($"Failed to configure telemetry buffer for type {type}: {ex.Message}");
                }
            }
        }

        private void EnsureBuffers(string instrumentationKey)
        {
            if (!_telemetryBuffers.ContainsKey(instrumentationKey))
            {
                _telemetryBuffers[instrumentationKey] = new ConcurrentDictionary<Type, TelemetryItemBuffer>();

                foreach (Type telemetryType in _typesToTrack)
                {
                    if (!_telemetryBuffers[instrumentationKey].ContainsKey(telemetryType))
                    {
                        _telemetryBuffers[instrumentationKey][telemetryType] = new TelemetryItemBuffer(instrumentationKey)
                        {
                            PrefixMetricsWithType = this.PrefixMetricsWithType
                        };
                    }
                }
            }
        }

        public void Process(ITelemetry item)
        {
            if (item is MetricTelemetry)
            {
                // if we don't ignore metric telemetry items we'll run into
                // an infinite loop
                _next.Process(item);
                return;
            }

            // do we track this type?
            if (_typesToTrack.Contains(item.GetType()))
            {
                // get the AI instance this item is supposed to go to and make sure we've got buffers
                EnsureBuffers(item.Context.InstrumentationKey);

                // count the item
                if (_telemetryBuffers[item.Context.InstrumentationKey][item.GetType()].CountTelemetry(item))
                {
                    // we haven't seen this one before (for this AI instance), so send it off to AI
                    _next.Process(item);
                }

                return;
            }

            _next.Process(item);
        }
    }
}

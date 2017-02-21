﻿using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace helgemahrt.EnhancedAI.TelemetryProcessors
{
    /// <summary>
    /// A telemetry processor which serializes properties of Exceptions into JSON strings and adds them to the properties of the ExceptionTelemetry item sent to ApplicationInsight.
    /// This way they show up under Custom Data in the Application Insights Dashboard.
    /// Excluded are duplicate properties like the Stack Trace, the Message, the Target Site and Tasks, which cause issues when serialized.
    /// </summary>
    public class ExceptionTelemetryEnhancer : ITelemetryProcessor
    {
        // the next telemetry processor in the chain
        private ITelemetryProcessor _next;

        public ExceptionTelemetryEnhancer(ITelemetryProcessor next)
        {
            _next = next;
        }

        // just create the settings once; ignore cyclic references
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        public void Process(ITelemetry item)
        {
            ExceptionTelemetry telemetry = item as ExceptionTelemetry;
            if (telemetry != null)
            {
                // get the properties of this exception
                Type exceptionType = telemetry.Exception?.GetType();
                if (exceptionType != null)
                {
                    foreach (PropertyInfo property in exceptionType.GetProperties())
                    {
                        // ignore duplicate data (already covered by AI) and tasks
                        if (!string.Equals(property.Name, "StackTrace") &&
                            !string.Equals(property.Name, "Message") &&
                            !string.Equals(property.Name, "TargetSite") &&
                            property.PropertyType.BaseType != typeof(Task))
                        {
                            try
                            {
                                // now serialize!
                                telemetry.Properties[$"{exceptionType.Name}.{property.Name}"] = JsonConvert.SerializeObject(property.GetValue(telemetry.Exception), jsonSettings);
                            }
                            catch (Exception ex)
                            {
                                // just in case
                                telemetry.Properties[$"{exceptionType.Name}.{property.Name}"] = "Serialization failed";
                                Debug.WriteLine($"Failed to serialize property {exceptionType.Name}.{property.Name}'s value. Error: {ex.Message}");
                            }
                        }
                    }
                }
            }

            // send the item off to the next processor
            _next.Process(item);
        }
    }
}

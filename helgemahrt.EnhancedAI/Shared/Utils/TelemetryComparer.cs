using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace helgemahrt.EnhancedAI.Utils
{
    public class TelemetryComparer : IEqualityComparer<ITelemetry>
    {
        public bool Equals(ITelemetry x, ITelemetry y)
        {
            // first compare the types
            Type typeX = x.GetType();
            Type typeY = y.GetType();
            if (!Equals(typeX, typeY))
            {
                return false;
            }

            // then compare the names
            if (!string.Equals(x.GetNameOrMessage(), y.GetNameOrMessage()))
            {
                return false;
            }

            // if the telemetry items support properties, include them in the comparison
            if (typeX.GetInterfaces().Contains(typeof(ISupportProperties)) &&
                typeY.GetInterfaces().Contains(typeof(ISupportProperties)))
            {
                ISupportProperties xp = x as ISupportProperties;
                ISupportProperties yp = y as ISupportProperties;

                // the compare the properties
                if (xp.Properties.Count != yp.Properties.Count)
                {
                    return false;
                }

                foreach (KeyValuePair<string, string> kv in xp.Properties)
                {
                    if (!yp.Properties.ContainsKey(kv.Key))
                    {
                        return false;
                    }

                    if (!string.Equals(xp.Properties[kv.Key], yp.Properties[kv.Key]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(ITelemetry obj)
        {
            // now construct the basis for our hash
            StringBuilder sb = new StringBuilder();

            // first append the type
            sb.Append(obj.GetType().ToString());

            // then the name
            sb.Append(obj.GetNameOrMessage());

            if (obj.GetType().GetInterfaces().Contains(typeof(ISupportProperties)))
            {
                ISupportProperties objp = obj as ISupportProperties;

                // Properties may have been added out of order, which may result
                // in different hash values despite having the same properties.
                // Therefore we need to construct the string on which we base our
                // hash value in an ordered manner.
                SortedSet<string> sortedKeys = new SortedSet<string>();
                foreach (string key in objp.Properties.Keys)
                {
                    sortedKeys.Add(key);
                }

                // then each key/value pair in an ordered manner
                foreach (string key in sortedKeys)
                {
                    sb.Append(key).Append(objp.Properties[key]);
                }
            }

            // and finally calculate the hash
            return sb.ToString().GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPlanOut.Core.Namespaces
{
    public static class PlanoutExtensions
    {
        public static Dictionary<string, object> AsPlanoutDictionary(this object value) =>
            (Dictionary<string, object>)value;
    }

    public class NamespaceManagerService : INamespaceManagerService
    {
        public Dictionary<string, object> Inputs { get; set; }
        public IEventLogger EventLogger { get; set; }
        public Dictionary<string, Namespace> Namespaces { get; set; }

        public NamespaceManagerService(Dictionary<string, object> inputs, IEventLogger eventLogger)
        {
            Inputs = inputs;
            EventLogger = eventLogger;
            Namespaces = new Dictionary<string, Namespace>();
        }

        public void AddNamespace(Namespace namesp)
        {
            Namespaces.Add(namesp.Name, namesp);
        }

        public void AddExperiment(string namesp, Experiment exp, int segments)
        {
            Namespaces[namesp].AddExperiment(exp, segments);
        }

        public bool HasNamespace(string namesp)
        {
            return Namespaces.ContainsKey(namesp);
        }

        public Dictionary<string, object> GetParams()
        {
            IEnumerable<KeyValuePair<string, object>> namespaceValues = new Dictionary<string, object>();
            foreach (var key in Namespaces.Keys)
            {
                Namespaces[key]._inputs = Inputs;
                namespaceValues = namespaceValues.Concat(Namespaces[key].GetParams());
            }
            return namespaceValues.ToDictionary(e=>e.Key, e=>e.Value);
        }

        public void LogEvent(string namesp, string eventName, Dictionary<string, object> extras)
        {
            if (Inputs == null)
            {
                throw new NullReferenceException("Inputs cannot be null.");
            }
            Namespaces[namesp]._inputs = Inputs;
            Namespaces[namesp].LogEvent(eventName, extras);
        }
    }
}
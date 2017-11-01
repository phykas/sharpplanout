using SharpPlanOut.Core;
using SharpPlanOut.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace SharpPlanOut.Services.Namespace
{
    public class NamespaceManager : INamespaceManager
    {
        public Dictionary<string, object> Inputs { get; set; }
        public IEventLogger EventLogger { get; set; }
        public Dictionary<string, Core.Namespace> Namespaces { get; set; }

        public NamespaceManager(IEventLogger eventLogger)
        {
            EventLogger = eventLogger;
            Namespaces = new Dictionary<string, Core.Namespace>();
        }

        public void AddNamespace(Core.Namespace namesp)
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

        public object GetParams(Dictionary<string, object> inputs)
        {
            Inputs = inputs;
            Dictionary<string, object> namespaceValues = new Dictionary<string, object>();
            foreach (var key in Namespaces.Keys)
            {
                Namespaces[key]._inputs = Inputs;
                namespaceValues.Add(key, Namespaces[key].GetParams());
            }
            return namespaceValues;
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
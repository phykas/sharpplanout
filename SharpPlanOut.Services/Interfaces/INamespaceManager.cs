using SharpPlanOut.Core;
using System.Collections.Generic;

namespace SharpPlanOut.Services.Interfaces
{
    public interface INamespaceManager
    {
        Dictionary<string, object> Inputs { get; set; }
        IEventLogger EventLogger { get; set; }

        void AddExperiment(string namesp, Experiment exp, int segments);

        object GetParams(Dictionary<string, object> inputs);

        void AddNamespace(Core.Namespace namesp);

        bool HasNamespace(string namesp);

        void LogEvent(string namesp, string eventName, Dictionary<string, object> extras);
    }
}
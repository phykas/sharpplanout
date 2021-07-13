using System.Collections.Generic;

namespace SharpPlanOut.Core.Namespaces
{
    public interface INamespaceManagerService
    {
        Dictionary<string, object> Inputs { get; set; }
        IEventLogger EventLogger { get; set; }

        void AddExperiment(string namesp, Experiment exp, int segments);

        Dictionary<string, object> GetParams();

        void AddNamespace(Namespace namesp);

        bool HasNamespace(string namesp);

        void LogEvent(string namesp, string eventName, Dictionary<string, object> extras);
    }
}
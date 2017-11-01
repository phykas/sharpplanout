using System.Collections.Generic;

namespace SharpPlanOut.Core
{
    public interface IEventLogger
    {
        void Log(Dictionary<string, object> logData);
    }
}
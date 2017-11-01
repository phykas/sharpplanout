using System.Collections.Generic;
using System.Diagnostics;

namespace SharpPlanOut.Core.Loggers
{
    public class DebugEventLogger : IEventLogger
    {
        public void Log(Dictionary<string, object> logData)
        {
            foreach (var log in logData)
            {
                Debug.WriteLine("{0} - {1}", log.Key, log.Value);
            }
        }
    }
}
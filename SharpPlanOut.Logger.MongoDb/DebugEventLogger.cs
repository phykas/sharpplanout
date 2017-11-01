using Newtonsoft.Json;
using SharpPlanOut.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace SharpPlanOut.Logger.MongoDb
{
    public class DebugEventLogger : IEventLogger
    {
        public void Log(Dictionary<string, object> logData)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(logData));
        }
    }
}
namespace SharpPlanOut.Logger
{
    public class JsonLogger
    {
        public void Log(object value)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }
}
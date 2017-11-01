using SharpPlanOut.Core.Random;
using System.Collections.Generic;

namespace SharpPlanOut.Core
{
    public class Assignment : IMapper
    {
        public Dictionary<string, object> Overrides { get; set; }
        public Dictionary<string, object> Params { get; set; }
        public string ExperimentSalt { get; set; }
        public string SaltSeparator { get; set; }
        public PlanOutOpRandom PlanOutRandom { get; set; }

        public Assignment(Dictionary<string, object> overrides)
        {
            Overrides = overrides;
            Params = overrides;
            SaltSeparator = ".";
        }

        public Assignment(string experimentSalt)
        {
            ExperimentSalt = experimentSalt;
            Params = new Dictionary<string, object>();
            Overrides = new Dictionary<string, object>();
            SaltSeparator = ".";
        }

        public Assignment(Dictionary<string, object> overrides, string experimentSalt)
        {
            ExperimentSalt = experimentSalt;
            Overrides = overrides;
            Params = overrides;
            SaltSeparator = ".";
        }

        public void AddOverride(KeyValuePair<string, object> overr)
        {
            Overrides[overr.Key] = overr.Value;
            Params[overr.Key] = overr.Value;
        }

        public object Set(string name, PlanOutOpRandom value)
        {
            PlanOutRandom = value;
            if (!value._param.ContainsKey("salt"))
            {
                value._param.Add("salt", name);
            }
            Params.Add(name, value.Execute(this));
            return Params[name];
        }

        public object Set(string name, object value)
        {
            if (Params.ContainsKey(name))
            {
                return null;
            }
            Params.Add(name, value);
            return Params[name];
        }

        public object Evaluate(object obj)
        {
            return obj;
        }

        public object Get(string name, object defaultValue = null)
        {
            switch (name)
            {
                case "_data":
                    return Params;

                case "_overrides":
                    return Overrides;

                case "experimentSalt":
                    return ExperimentSalt;

                case "saltSeparator":
                    return SaltSeparator;

                default:
                    object value;
                    Params.TryGetValue(name, out value);
                    return value ?? defaultValue;
            }
        }
    }
}
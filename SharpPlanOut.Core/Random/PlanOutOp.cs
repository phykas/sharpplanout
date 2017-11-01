using System;
using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public abstract class PlanOutOp
    {
        public readonly Dictionary<string, object> _param;
        public readonly Dictionary<string, object> _units;

        protected PlanOutOp(Dictionary<string, object> param, Dictionary<string, object> units)
        {
            _param = param;
            _units = units;
        }

        public abstract object Execute(IMapper mapper);

        public Dictionary<string, object> GetArguments()
        {
            return _param;
        }

        public object GetByName(string name)
        {
            if (_param.ContainsKey(name))
            {
                return _param[name];
            }
            throw new ArgumentException("No argument for a given name " + name);
        }
    }
}
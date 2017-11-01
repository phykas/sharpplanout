using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPlanOut.Core.Random
{
    public class BernoulliFilterBuilder : PlanOutOpRandom, IRandomOps
    {
        public BernoulliFilterBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var p = (double)_param["p"];
            var values = ((string[])_param["choices"]).ToList();
            if (p < 0 || p > 1)
            {
                throw new Exception("Invalid probability");
            }
            return values.Count == 0 ? null : values.Where(cur => GetUniform(0.0, 1.0, cur) <= p).ToList();
        }
    }
}
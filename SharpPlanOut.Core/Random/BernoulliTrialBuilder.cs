using System;
using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public class BernoulliTrialBuilder : PlanOutOpRandom, IRandomOps
    {
        public BernoulliTrialBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var p = (double)_param["p"];
            if (p < 0 || p > 1)
            {
                throw new Exception("Invalid probability");
            }

            return GetUniform() <= p ? 1 : 0;
        }
    }
}
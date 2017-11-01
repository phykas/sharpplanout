using System;
using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public class UniformChoiceBuilder : PlanOutOpRandom, IRandomOps
    {
        public UniformChoiceBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public long RandomIndexCalculation(Array choices)
        {
            var hash = GetHash();
            return hash % (choices.Length);
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var choices = (Array)_param["choices"];
            if (choices.Length == 0)
            {
                return null;
            }
            var randIndex = RandomIndexCalculation(choices);
            return choices.GetValue(randIndex);
        }
    }
}
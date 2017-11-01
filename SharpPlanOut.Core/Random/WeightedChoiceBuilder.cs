using System;
using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public class WeightedChoiceBuilder : PlanOutOpRandom, IRandomOps
    {
        public WeightedChoiceBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var choices = (Array)_param["choices"];
            var weights = (double[])_param["weights"];
            if (choices.Length == 0)
            {
                return null;
            }
            var cumSum = 0d;
            var cumWeights = new List<double>();
            foreach (var weight in weights)
            {
                cumSum += weight;
                cumWeights.Add(cumSum);
            }
            var stopVal = GetUniform(0.0, cumSum);
            for (var i = 0; i < cumWeights.Count; ++i)
            {
                if (stopVal <= cumWeights[i])
                {
                    return choices.GetValue(i);
                }
            }
            return null;
        }
    }
}
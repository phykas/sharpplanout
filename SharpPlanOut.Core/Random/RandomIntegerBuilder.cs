using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public class RandomIntegerBuilder : PlanOutOpRandom, IRandomOps
    {
        public RandomIntegerBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public long RandomIntegerCalculation(int minVal, int maxVal)
        {
            return (GetHash() + minVal) % (maxVal - minVal + 1);
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var minVal = (int)_param["min"];
            var maxVal = (int)_param["max"];
            return RandomIntegerCalculation(minVal, maxVal);
        }
    }
}
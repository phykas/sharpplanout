using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public class RandomFloatBuilder : PlanOutOpRandom, IRandomOps
    {
        public RandomFloatBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var minVal = (double)_param["min"];
            var maxVal = (double)_param["max"];
            return GetUniform(minVal, maxVal);
        }
    }
}
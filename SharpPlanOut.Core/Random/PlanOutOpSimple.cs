using System.Collections.Generic;

namespace SharpPlanOut.Core.Random
{
    public abstract class PlanOutOpSimple : PlanOutOp
    {
        public IMapper Mapper { get; set; }

        protected PlanOutOpSimple(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }
    }
}
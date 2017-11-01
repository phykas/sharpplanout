using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SharpPlanOut.Core.Random
{
    public class SampleBuilder : PlanOutOpRandom, IRandomOps
    {
        public SampleBuilder(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public bool AllowSampleStoppingPoint { get; set; }

        public long SampleIndexCalculation(int i)
        {
            return GetHash(i.ToString()) % (i + 1);
        }

        public IList Sample(List<int> array, int numDraws)
        {
            var len = array.Count;
            var stoppingPoint = len - numDraws;
            for (int i = len - 1; i > 0; i--)
            {
                var j = (int)SampleIndexCalculation(i);
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                if (AllowSampleStoppingPoint && stoppingPoint == i)
                {
                    return array.Skip(i).ToList();
                }
            }
            return array.GetRange(0, numDraws);
        }

        public override object Execute(IMapper mapper)
        {
            base.Execute(mapper);
            var choices = (List<int>)_param["choices"];
            var tempChoices = choices.ToList();
            int numDraws;
            if (_param.ContainsKey("draws"))
            {
                numDraws = (int)_param["draws"];
            }
            else
            {
                numDraws = tempChoices.Count;
            }
            return Sample(tempChoices, numDraws);
        }
    }
}
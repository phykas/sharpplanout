using System;
using System.Collections.Generic;
using System.Linq;
using SharpPlanOut.Core.Random;

namespace SharpPlanOut.Core.Namespaces
{
    public class DefaultExperiment : Experiment
    {
        public DefaultExperiment(string name, Dictionary<string, object> inputs, Func<Assignment, Dictionary<string, object>, bool> assign)
            : base(name, inputs, assign)
        {
        }
    }

    public abstract class Namespace
    {
        public string Name { get; set; }

        public string PrimaryUnit { get; set; }

        public Dictionary<string, Experiment> ActiveExperiments { get; set; }

        public List<int> AvailableSeqments { get; set; }

        public Dictionary<int, string> AllocationMap { get; set; }

        public Experiment DefaultExperiment { get; set; }

        public Experiment Experiment { get; set; }

        public Dictionary<string, object> _inputs;

        protected Namespace(Dictionary<string, object> inputs)
        {
            _inputs = inputs;
        }

        public abstract bool AddExperiment(Experiment experiment, int segments);

        public abstract bool RemoveExperiment(string name);

        public abstract int GetSegment();

        public abstract object Get(string name, object defaultValue);

        public abstract object DefaultGet(string name, object defaultValue);

        public abstract void LogExposure(string name, Dictionary<string, object> extras);

        public abstract void LogEvent(string eventName, Dictionary<string, object> extras);

        public void RequireExperiment()
        {
            if (Experiment == null)
            {
                AssignExperiment();
            }
        }

        public void RequireDefaultExperiment()
        {
            if (DefaultExperiment == null)
            {
                AssignDefaultExperiment();
            }
        }

        public int NumSegments { get; set; }

        public bool InExperiment { get; set; }

        public abstract void AssignExperiment();

        public abstract void AssignDefaultExperiment();

        public abstract Dictionary<string, object> GetParams();
    }

    public class SimpleNamespace : Namespace
    {
        public SimpleNamespace(string namespaceName, Dictionary<string, object> inputs,
            string primaryUnit, int numSegments)
            : base(inputs)
        {
            PrimaryUnit = primaryUnit;
            Name = namespaceName;
            NumSegments = numSegments;
            AvailableSeqments = Enumerable.Range(0, NumSegments).ToList();
            AllocationMap = new Dictionary<int, string>();
            ActiveExperiments = new Dictionary<string, Experiment>();
        }

        public override bool AddExperiment(Experiment experiment, int segments)
        {
            var numberAvailable = AvailableSeqments.Count;
            if (numberAvailable < segments || ActiveExperiments.ContainsKey(experiment.Name))
            {
                return false;
            }
            var assigment = new Assignment(Name);
            assigment.Set("sampled_segments",
                new SampleBuilder(new Dictionary<string, object>()
                {
                    {"choices", AvailableSeqments},
                    {"draws", segments}
                }, new Dictionary<string, object>()
                {
                    {"experiment_name", experiment.Name}
                }));
            var sample = (List<int>)assigment.Get("sampled_segments");
            foreach (int segment in sample)
            {
                AllocationMap[segment] = experiment.Name;
                var index = AvailableSeqments.IndexOf(segment);
                AvailableSeqments.RemoveAt(index);
                numberAvailable -= 1;
            }
            ActiveExperiments[experiment.Name] = experiment;
            return true;
        }

        public override bool RemoveExperiment(string name)
        {
            if (!ActiveExperiments.ContainsKey(name))
            {
                return false;
            }
            var keysToRemove =
                (from segmentAllocation in AllocationMap
                 where segmentAllocation.Value == name
                 select segmentAllocation.Key).ToList();

            foreach (var key in keysToRemove)
            {
                AllocationMap.Remove(key);
                AvailableSeqments.Add(key);
            }
            return true;
        }

        public override int GetSegment()
        {
            var assigment = new Assignment(Name);
            var segment = new RandomIntegerBuilder(new Dictionary<string, object>()
            {
                {"min", 0},
                {"max", NumSegments - 1}
            }, new Dictionary<string, object>()
            {
                {"primary_unit", _inputs[PrimaryUnit]}
            });
            assigment.Set("segment", segment);
            return Convert.ToInt32(assigment.Get("segment"));
        }

        public override object Get(string name, object defaultValue = null)
        {
            RequireExperiment();
            if (Experiment == null)
            {
                return DefaultGet(name, defaultValue);
            }

            return Experiment.Get(name, DefaultGet(name, defaultValue));
        }

        public override object DefaultGet(string name, object defaultValue = null)
        {
            RequireDefaultExperiment();
            return DefaultExperiment.Get(name, defaultValue);
        }

        public override void LogExposure(string name, Dictionary<string, object> extras)
        {
            RequireExperiment();
            if (Experiment == null)
            {
                return;
            }
            Experiment.LogExposure(name, extras);
        }

        public override void LogEvent(string eventName, Dictionary<string, object> extras)
        {
            RequireExperiment();
            if (Experiment == null)
            {
                return;
            }
            Experiment._inputs = _inputs;
            Experiment.LogEvent(eventName, extras);
        }

        public override void AssignExperiment()
        {
            var segment = GetSegment();
            if (!AllocationMap.ContainsKey(segment))
            {
                return;
            }

            var experimentName = AllocationMap[segment];
            _assignExperimentObject(experimentName);
        }

        private void _assignExperimentObject(string experimentName)
        {
            var experiment = ActiveExperiments[experimentName];
            var name = Name + "." + experimentName;
            experiment.Name = name;
            experiment.Salt = name;
            Experiment = experiment;
            InExperiment = experiment.InExperiment;
            if (!InExperiment)
            {
                AssignDefaultExperiment();
            }
        }

        public override Dictionary<string, object> GetParams()
        {
            RequireExperiment();
            if (Experiment != null)
            {
                Experiment._inputs = _inputs;
                return Experiment.GetParams();
            }
            RequireDefaultExperiment();
            DefaultExperiment._inputs = _inputs;
            return DefaultExperiment.GetParams();
        }

        public override void AssignDefaultExperiment()
        {
            Experiment = DefaultExperiment;
        }

        public Experiment GetExperiment(string name)
        {
            return ActiveExperiments[name];
        }
    }
}
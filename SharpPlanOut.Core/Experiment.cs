using System;
using System.Collections.Generic;

namespace SharpPlanOut.Core
{
    public class Experiment
    {
        public Dictionary<string, object> _inputs;
        public Assignment _assignment;
        private string _salt;

        public bool AutoExposureLog { get; set; }

        public bool InExperiment { get; set; }

        public string Salt
        {
            get { return _salt ?? Name; }
            set
            {
                _salt = value;
                _assignment.ExperimentSalt = value;
            }
        }

        public bool Assigned { get; set; }
        public bool ExposureLogged { get; set; }
        public Func<Assignment, Dictionary<string, object>, bool> Assign { get; set; }
        public Action ConfigureLogger { get; set; }
        public Action<Dictionary<string, object>> Log { get; set; }

        public Experiment(string name, Dictionary<string, object> inputs,
            Func<Assignment, Dictionary<string, object>, bool> assign)
        {
            Name = name;
            _inputs = inputs;
            _assignment = new Assignment(Salt);
            ExposureLogged = false;
            InExperiment = true;
            AutoExposureLog = true;
            Assigned = false;
            Assign = assign;
            ConfigureLogger = () => { };
            Log = objects => { };
        }

        public string Name { get; set; }

        public void _assign()
        {
            ConfigureLogger();
            var assignResponse = Assign(_assignment, _inputs);
            InExperiment = assignResponse;
            Assigned = true;
        }

        public void RequireAssignment()
        {
            if (!Assigned)
            {
                _assign();
            }
        }

        public void AddOverride(string key, object value)
        {
            _assignment.AddOverride(new KeyValuePair<string, object>(key, value));
        }

        public Dictionary<string, object> GetParams()
        {
            RequireAssignment();
            RequireExposureLogging("all");
            return _assignment.Params;
        }

        public object Get(string name, object defaultValue = null)
        {
            RequireAssignment();
            RequireExposureLogging(name);
            return _assignment.Get(name, defaultValue);
        }

        public void LogExposure(string name, object extras = null)
        {
            if (!InExperiment)
            {
                return;
            }
            ExposureLogged = true;
            LogEvent("exposure." + name, extras);
        }

        public void LogEvent(string eventType, object extras)
        {
            if (!InExperiment)
            {
                return;
            }
            var log = new Dictionary<string, object>()
            {
                {"Name", Name},
                {"Inputs", _inputs},
                {"Time", DateTime.UtcNow},
                {"Event", eventType},
                {"Extras", extras}
            };
            if (_assignment.Params.Count > 0)
            {
                log.Add("Assignment", _assignment);
            }
            Log(log);
        }

        private void RequireExposureLogging(string name)
        {
            if (ShouldLogExposure(name) || name == "all")
            {
                LogExposure(name);
            }
        }

        private bool ShouldLogExposure(string name)
        {
            return AutoExposureLog && _assignment.Params.ContainsKey(name);
        }
    }
}
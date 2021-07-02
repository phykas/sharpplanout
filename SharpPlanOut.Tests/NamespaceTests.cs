using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SharpPlanOut.Core;
using SharpPlanOut.Core.Random;
using System.Collections.Generic;

namespace SharpPlanOut.Tests
{
    [TestClass]
    public class NamespaceTests
    {
        private Experiment _experiment1;
        private Experiment _experiment2;
        private SimpleNamespace _simpleNamespace;
        private string _experiment1ParamName = "test";
        private string _experiment2ParamName = "test";
        private string _defaultParamName = "test";
        private Experiment _experimentDefault;
        private List<Dictionary<string, object>> _logs = new List<Dictionary<string, object>>();

        [TestInitialize]
        public void Initialize()
        {
            _logs = new List<Dictionary<string, object>>();
            _experiment1 = new Experiment("first_experiment", new Dictionary<string, object>(), (assignment, objects) =>
            {
                assignment.Set(_experiment1ParamName, 1);
                return true;
            })
            {
                Log = objects => { _logs.Add(objects); }
            };

            _experiment2 = new Experiment("second_experiment", new Dictionary<string, object>(),
                (assignment, objects) =>
                {
                    assignment.Set(_experiment2ParamName, 2);
                    return true;
                })
            {
                Log = objects => { _logs.Add(objects); }
            };

            _experimentDefault = new Experiment("default_experiment", new Dictionary<string, object>(),
                (assignment, objects) =>
                {
                    assignment.Set(_defaultParamName, 3);
                    return false;
                });

            _simpleNamespace = new SimpleNamespace("first_namespace", new Dictionary<string, object>()
            {
                {"user_id", "blah"}
            }, "user_id", 100);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldReturnAParamFromDefaultExperiment()
        {
            //arrange

            //act
            _simpleNamespace.DefaultExperiment = _experiment1;
            _simpleNamespace.AddExperiment(_experiment2, 10);

            //assert
            Assert.AreEqual(_simpleNamespace.Get(_experiment1ParamName), 1);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldReturnAParamFromAddedExperimentNotDefault()
        {
            //arrange

            //act
            _simpleNamespace.DefaultExperiment = _experiment1;
            _simpleNamespace.AddExperiment(_experiment2, 10);

            //assert
            Assert.AreEqual(_simpleNamespace.Get(_experiment2ParamName), 1);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldDecreaseAvailableSegments()
        {
            //arrange

            //act
            _simpleNamespace.DefaultExperiment = _experiment1;
            _simpleNamespace.AddExperiment(_experiment2, 10);

            //assert
            Assert.AreEqual(90, _simpleNamespace.AvailableSeqments.Count);
            Assert.AreEqual(100, _simpleNamespace.NumSegments);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_AddsTwoSegmentsCorrectly()
        {
            //arrange
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment2, 10);
            _simpleNamespace.AddExperiment(_experiment1, 10);

            //act
            var param1 = _simpleNamespace.Get(_experiment1ParamName);
            var param2 = _simpleNamespace.Get(_experiment2ParamName);

            //assert
            Assert.IsNotNull(param1);
            Assert.AreEqual(3, param1);
            Assert.IsNotNull(param2);
            Assert.AreEqual(3, param2);
            Assert.AreEqual(80, _simpleNamespace.AvailableSeqments.Count);
            Assert.AreEqual(100, _simpleNamespace.NumSegments);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_CanRemoveCorrectly()
        {
            //arrange
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment1, 10);
            _simpleNamespace.RemoveExperiment(_experiment1.Name);

            //act
            var param1 = _simpleNamespace.Get(_experiment1ParamName);
            var param2 = _simpleNamespace.Get(_experiment2ParamName);

            //assert
            Assert.IsNotNull(param1);
            Assert.AreEqual(3, param1);
            Assert.IsNotNull(param2);
            Assert.AreEqual(3, param2);
            Assert.AreEqual(100, _simpleNamespace.AvailableSeqments.Count);
            Assert.AreEqual(100, _simpleNamespace.NumSegments);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldNotLogUserNotInExperiment()
        {
            //arrange
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment1, 10);
            _simpleNamespace.RemoveExperiment(_experiment1.Name);

            //act
            var param1 = _simpleNamespace.Get(_experiment1ParamName);
            var param2 = _simpleNamespace.Get(_experiment2ParamName);

            //assert
            Assert.AreEqual(0, _logs.Count);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldAddOneLogToLogs()
        {
            //arrange
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment1, 100);

            //act
            var param1 = _simpleNamespace.Get(_experiment1ParamName);
            var param2 = _simpleNamespace.Get("test22");

            //assert
            Assert.AreEqual(1, _logs.Count);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldRespectAutoExposureLog()
        {
            //arrange
            _experiment1.AutoExposureLog = false;
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment1, 100);

            //act
            var param1 = _simpleNamespace.Get(_experiment1ParamName);
            var param2 = _simpleNamespace.Get("test22");

            //assert
            Assert.AreEqual(0, _logs.Count);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldLogCorrectly()
        {
            //arrange
            _experiment1 = new Experiment("test1", new Dictionary<string, object>()
            {
                {"userid", "123"}
            }, (assignment, objects) =>
            {
                assignment.Set(_experiment1ParamName, new UniformChoiceBuilder(new Dictionary<string, object>()
                {
                    {"choices", new[] {"test", "test1"}}
                }, _experiment1._inputs));
                return true;
            });
            _experiment1.Log = objects => { _logs.Add(objects); };
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment1, 100);

            //act
            _simpleNamespace.Get(_experiment1ParamName);
            _simpleNamespace.Get("test22");

            //assert
            Assert.AreEqual(1, _logs.Count);
        }

        [TestMethod]
        public void NamespaceTest_Tryout_ShouldLogEvent()
        {
            //arrange
            _experiment1 = new Experiment("test1", new Dictionary<string, object>()
            {
                {"userid", "123"}
            }, (assignment, objects) =>
            {
                assignment.Set(_experiment1ParamName, new UniformChoiceBuilder(new Dictionary<string, object>()
                {
                    {"choices", new[] {"test", "test1"}}
                }, _experiment1._inputs));
                return true;
            });
            _experiment1.Log = objects => { _logs.Add(objects); };
            _simpleNamespace.DefaultExperiment = _experimentDefault;
            _simpleNamespace.AddExperiment(_experiment1, 100);

            //act
            _simpleNamespace.LogEvent("purchase", new Dictionary<string, object>()
            {
                {"price", 1}
            });

            var json = JsonConvert.SerializeObject(_logs, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            //assert
            Assert.AreEqual(1, _logs.Count);
        }
    }
}
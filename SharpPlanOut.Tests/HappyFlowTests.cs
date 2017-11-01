using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SharpPlanOut.Core;
using SharpPlanOut.Core.Random;
using System.Collections.Generic;

namespace SharpPlanOut.Tests
{
    [TestClass]
    public class HappyFlowTests
    {
        [TestMethod]
        public void Assignment_WeightedChoiceBuilder_ShouldGetRandomByWeight()
        {
            //arrange
            Assignment ass = new Assignment(new Dictionary<string, object>(), "Exper1");
            Dictionary<object, int> valueCounter = new Dictionary<object, int>();

            //act
            for (int i = 0; i < 100000; i++)
            {
                var value =
                    ass.Set("has_test_Istest" + i, new WeightedChoiceBuilder(new Dictionary<string, object>()
                    {
                        {"choices", new object[] {1, 10, 30, "test", 51}},
                        {"weights", new[] {2d, 1d, 0d, 1d, 6d}},
                    }, new Dictionary<string, object>()
                    {
                        {"user_id", "testuser" + i}
                    }
                        ));
                if (!valueCounter.ContainsKey(value))
                {
                    valueCounter.Add(value, 1);
                }
                else
                {
                    valueCounter[value]++;
                }
            }

            //assert
            var ratio51 = valueCounter[51] / 100000d;
            Assert.AreEqual(0.6, actual: ratio51, delta: 0.05);

            var ratiotest = valueCounter["test"] / 100000d;
            Assert.AreEqual(0.1, actual: ratiotest, delta: 0.05);

            Assert.AreEqual(false, valueCounter.ContainsKey(30));
        }

        [TestMethod]
        public void ExperimentTest_GettingParams_ShouldReturnExpectedParam()
        {
            //arrange
            var experiment = new Experiment("first_experiment", new Dictionary<string, object>(), (assignment, objects) =>
            {
                assignment.Set("should_open_popup", new UniformChoiceBuilder(new Dictionary<string, object>()
                    {
                        {"choices", new[] {"test1", "test2", "test3"}},
                    },
                    new Dictionary<string, object>()
                        {
                            {"user_id", "filip123"}
                        }));
                return false;
            });
            //act
            var result = experiment.Get("should_open_popup");

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test3", result);
        }

        [TestMethod]
        public void ExperimentTest_Logging_ShouldNotLogData()
        {
            //arrange
            var iter = 0;
            var experiment = new Experiment("first_experiment", new Dictionary<string, object>(),
                (assignment, objects) =>
                {
                    assignment.Set("should_open_popup", new UniformChoiceBuilder(new Dictionary<string, object>()
                    {
                        {"choices", new[] {"test1", "test2", "test3"}},
                    },
                        new Dictionary<string, object>()
                        {
                            {"user_id", "filip123"}
                        }));
                    return false;
                })
            {
                Log = data => { iter++; }
            };

            //act
            experiment.Get("should_open_popup");

            //assert
            Assert.AreEqual(0, iter);
        }

        [TestMethod]
        public void ExperimentTest_Logging_ShouldLogData()
        {
            //arrange
            var iter = 0;
            var experiment = new Experiment("first_experiment", new Dictionary<string, object>(), (assignment, objects) =>
            {
                assignment.Set("should_open_popup", new UniformChoiceBuilder(
                    new Dictionary<string, object>()
                        {
                            {"choices", new[] {"test1", "test2", "test3"}},
                        },
                    new Dictionary<string, object>()
                        {
                            {"user_id", "filip123"}
                        }));
                return true;
            })
            {
                Log = data =>
                {
                    iter++;
                }
            };

            //act
            experiment.Get("should_open_popup");

            //assert
            Assert.AreEqual(1, iter);
        }

        [TestMethod]
        public void ExperimentTest_Uniform_ShouldDistributeUniformly()
        {
            //arrange
            Dictionary<object, int> counter = new Dictionary<object, int>();

            //act
            for (int i = 0; i < 10000; i++)
            {
                var i1 = i;
                var experiment = new Experiment("first_experiment", new Dictionary<string, object>(),
                    (assignment, objects) =>
                    {
                        assignment.Set("should_open_popup", new UniformChoiceBuilder(new Dictionary<string, object>()
                        {
                            {"choices", new[] {"test1", "test2", "test3"}},
                        },
                            new Dictionary<string, object>()
                            {
                                {"user_id", "filip123" + i1}
                            }));
                        return true;
                    });

                var result = experiment.Get("should_open_popup");
                if (counter.ContainsKey(result))
                {
                    counter[result]++;
                }
                else
                {
                    counter.Add(result, 1);
                }
            }

            //assert
            Assert.AreEqual(0.33, counter["test1"] / 10000d, 0.05);
        }

        [TestMethod]
        public void ExperimentTest_Assignment_ExperimentSaltShouldBeSet()
        {
            //arrange
            var experiment = new Experiment("first_experiment", new Dictionary<string, object>(), (assignment, objects) => false);

            //act

            //assert
            Assert.AreEqual("first_experiment", experiment._assignment.ExperimentSalt);
        }

        [TestMethod]
        public void ExperimentTest_ChangeExperimentName_ExperimentSaltShouldBeSet()
        {
            //arrange
            var experiment = new Experiment("first_experiment", new Dictionary<string, object>(),
                (assignment, objects) => false) { Salt = "new name" };

            //act

            //assert
            Assert.AreEqual("new name", experiment._assignment.ExperimentSalt);
        }
    }
}
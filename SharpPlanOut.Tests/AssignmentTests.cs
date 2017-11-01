using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpPlanOut.Core;
using SharpPlanOut.Core.Random;
using System.Collections.Generic;

namespace SharpPlanOut.Tests
{
    [TestClass]
    public class AssignmentTests
    {
        private string _experimentSalt = "test_salt";
        private string _experimentUnit = "4";

        [TestMethod]
        public void Set_ShouldSetConstantsCorrectly()
        {
            //arrange
            var a = new Assignment(_experimentSalt);

            //act
            a.Set("foo", 12);

            //arrange
            Assert.AreEqual(12, a.Get("foo"));
        }

        [TestMethod]
        public void Set_ShouldWorkWithUniformChoice()
        {
            //arrange
            var a = new Assignment(_experimentSalt);
            var choices = new[] { "a", "b" };

            //act
            a.Set("foo", new UniformChoiceBuilder(new Dictionary<string, object>()
            {
                {"choices", choices}
            }, new Dictionary<string, object>()
            {
                {"unit", _experimentUnit}
            }));
            a.Set("bar", new UniformChoiceBuilder(new Dictionary<string, object>()
            {
                {"choices", choices}
            }, new Dictionary<string, object>()
            {
                {"unit", _experimentUnit}
            }));
            a.Set("baz", new UniformChoiceBuilder(new Dictionary<string, object>()
            {
                {"choices", choices}
            }, new Dictionary<string, object>()
            {
                {"unit", _experimentUnit}
            }));

            //arrange
            Assert.AreEqual("b", a.Get("foo"));
            Assert.AreEqual("a", a.Get("bar"));
            Assert.AreEqual("a", a.Get("baz"));
        }

        [TestMethod]
        public void Set_ShouldReturnDefaultValue()
        {
            //arrange
            var a = new Assignment(_experimentSalt);

            //act
            a.Set("x", 5);
            a.Set("y", 6);

            //arrange
            Assert.AreEqual("boom", a.Get("z", "boom"));
        }

        [TestMethod]
        public void Set_ShouldWorkWithOverrides()
        {
            //arrange
            var a = new Assignment(new Dictionary<string, object>()
            {
                {"x", 42},
                {"y", 43}
            }, _experimentSalt);

            //act
            a.Set("x", 5);
            a.Set("y", 6);

            //arrange
            Assert.AreEqual(42, a.Get("x"));
            Assert.AreEqual(43, a.Get("y"));
        }

        [TestMethod]
        public void Set_ShouldWorkWithFalsyOverrides()
        {
            //arrange
            var a = new Assignment(new Dictionary<string, object>()
            {
                {"x", 42},
                {"y", ""},
                {"z", false}
            }, _experimentSalt);

            //act
            a.Set("x", 5);
            a.Set("y", 6);
            a.Set("z", 7);

            //arrange
            Assert.AreEqual(42, a.Get("x"));
            Assert.AreEqual("", a.Get("y"));
            Assert.AreEqual(false, a.Get("z"));
        }

        [TestMethod]
        public void Set_ShouldWorkWithCustomSalts()
        {
            //arrange
            var a = new Assignment(_experimentSalt);

            //act
            a.Set("foo", new UniformChoiceBuilder(new Dictionary<string, object>()
            {
                {"choices", new []{ 0, 1, 2, 3, 4, 5, 6, 7}}
            }, new Dictionary<string, object>()
            {
                {"unit", _experimentUnit}
            }));

            //arrange
            Assert.AreEqual(7, a.Get("foo"));
        }
    }
}
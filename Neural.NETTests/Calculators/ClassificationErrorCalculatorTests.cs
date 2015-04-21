using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neural.Calculators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Neural.Calculators.Tests
{
    [TestClass()]
    public class ClassificationErrorCalculatorTests
    {
        [TestMethod()]
        public void CalculateErrorTest()
        {
            ErrorCalculator calculator = new ClassificationErrorCalculator();

            IDictionary<string, double> expected = new Dictionary<string, double>();
            IDictionary<string, double> actual = new Dictionary<string, double>();

            actual["alpha"] = 0.0;
            actual["beta"] = 0.0;

            expected["alpha"] = 0.0;
            expected["beta"] = 0.0;

            Assert.AreEqual(calculator.CalculateError(expected, actual), 0.0, 0.00001);

            actual["alpha"] = 0.0;
            actual["beta"] = 1.0;

            expected["alpha"] = 0.0;
            expected["beta"] = 0.0;

            Assert.AreEqual(calculator.CalculateError(expected, actual), 1.0, 0.00001);

            actual["alpha"] = 1.0;
            actual["beta"] = 1.0;

            expected["alpha"] = 0.0;
            expected["beta"] = 0.0;

            Assert.AreEqual(calculator.CalculateError(expected, actual), 1.0, 0.00001);
        }
    }
}

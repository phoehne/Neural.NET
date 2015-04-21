using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neural.Activation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Neural.Activation.Tests
{
    [TestClass()]
    public class SigmoidActivationFunctionTests
    {
        [TestMethod()]
        public void ActivationTest()
        {
            SigmoidActivationFunction saf = new SigmoidActivationFunction();
            Assert.AreEqual(0.5, saf.Activation(0.0), 0.00001);
        }

        [TestMethod()]
        public void DerivativeTest()
        {
            SigmoidActivationFunction saf = new SigmoidActivationFunction();
            Assert.AreEqual(0.25, saf.Derivative(0.0), 0.00001);
        }
    }
}

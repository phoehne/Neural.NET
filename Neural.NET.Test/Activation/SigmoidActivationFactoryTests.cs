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
    public class SigmoidActivationFactoryTests
    {
        [TestMethod()]
        public void SigmoidActivationFactoryTest()
        {
            SigmoidActivationFactory saf = new SigmoidActivationFactory();
            ActivationFunction av = saf.MakeFunction();

            Assert.IsNotNull(av);
            Assert.IsTrue(av.GetType().Equals((new SigmoidActivationFunction()).GetType()));
        }
    }
}

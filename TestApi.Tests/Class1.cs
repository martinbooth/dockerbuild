using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApi.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void PassingTest()
        {
            Assert.AreEqual(4, 2 + 2);
        }
    }
}

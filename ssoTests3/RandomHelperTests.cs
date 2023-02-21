using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests
{
    [TestClass()]
    public class RandomHelperTests
    {
        [TestMethod()]
        public void GetStringTest()
        {
            var v4 = RandomHelper.GetString(0);
            var v5 = RandomHelper.GetString(1);
            var v6 = RandomHelper.GetString(2);
            var v7 = RandomHelper.GetString(100);

            var v1 = RandomHelper.GetString(6);
            var v2 = RandomHelper.GetString(7);
            var v3 = RandomHelper.GetString(8);
            Assert.Fail();
        }
    }
}
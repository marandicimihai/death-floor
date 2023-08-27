using System.Collections;
using System.Collections.Generic;
using DeathFloor.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DeathFloor.Tests
{
    public class OptionalVarTests
    {
        [Test]
        public void Value_ReturnsDefault_WhenDisabled()
        {
            var intOpt = new Optional<int>(5, false);
            var boolOpt = new Optional<bool>(true, true);

            Assert.AreEqual(default(int), intOpt.Value);
            Assert.AreEqual(true, boolOpt.Value);
        }
    }
}

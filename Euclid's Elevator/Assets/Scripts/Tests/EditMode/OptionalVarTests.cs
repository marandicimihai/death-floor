using DeathFloor.Utilities;
using NUnit.Framework;

namespace DeathFloor.Tests
{
    public class OptionalVarTests
    {
        [Test]
        public void Value_ReturnsDefault_WhenDisabled()
        {
            var intOpt = new Optional<int>(5, false);

            Assert.AreEqual(default(int), intOpt.Value);
        }

        [Test]
        public void Value_ReturnsValue_WhenEnabled()
        {
            var boolOpt = new Optional<bool>(true, true);

            Assert.AreEqual(true, boolOpt.Value);
        }
    }
}

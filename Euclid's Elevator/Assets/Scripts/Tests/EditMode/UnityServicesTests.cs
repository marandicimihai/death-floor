using DeathFloor.UnityServices;
using NUnit.Framework;

namespace DeathFloor.Tests
{
    public class UnityServicesTests
    {
        [Test]
        public void GetDeltaTime_Returns_ANonZeroValue()
        {
            IUnityService service = new UnityService();

            float value = service.GetDeltaTime();

            Assert.AreNotEqual(value, 0);
        }
    }
}

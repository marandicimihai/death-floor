using System.Collections;
using DeathFloor.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DeathFloor.Tests
{
    public class OptionalAssignerTests
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator AssignUsingGetComponent_ReturnsNull_WhenClassIsNotPresent()
        {
            var gameObject = new GameObject();
            var optionalVar = new Optional<MonoBehaviour>(gameObject.AddComponent<MockBehaviour>());
            var optionalAssigner = new OptionalAssigner(optionalVar.Value);

            var rb = optionalAssigner.AssignUsingGetComponent<Rigidbody>(optionalVar);

            yield return null;

            Assert.IsNull(rb);
        }

        [UnityTest]
        public IEnumerator AssignUsingGetComponent_ReturnsGetComponent_WhenDisabled()
        {
            var firstObject = new GameObject();
            var firstOptionalVar = new Optional<MonoBehaviour>(null, false);
            var firstOptionalAssigner = new OptionalAssigner(firstObject.AddComponent<MockBehaviour>());

            var secondObject = new GameObject();
            secondObject.AddComponent<Rigidbody>();
            var secondOptionalVar = new Optional<MonoBehaviour>(null, false);
            var secondOptionalAssigner = new OptionalAssigner(secondObject.AddComponent<MockBehaviour>());

            var firstrb = firstOptionalAssigner.AssignUsingGetComponent<Rigidbody>(firstOptionalVar);

            var secondrb = secondOptionalAssigner.AssignUsingGetComponent<Rigidbody>(secondOptionalVar);

            yield return null;

            Assert.IsNull(firstrb);
            Assert.IsNotNull(secondrb);
        }
    }
}

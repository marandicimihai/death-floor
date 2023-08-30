using DeathFloor.Camera.Rotation;
using DeathFloor.Movement;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace DeathFloor.Tests
{
    public class ControllerProviderTests
    {
        private GameObject _dummyObject;

        [SetUp]
        public void Setup()
        {
            _dummyObject = new GameObject();
        }

        [UnityTest]
        public IEnumerator MovementProvider_ReturnsNonZeroVector_OnAnyInput()
        {
            var provider = _dummyObject.AddComponent<DefaultMovementProvider>();

            yield return null;

            Vector3 vec = Vector3.zero;
            provider.CalculateMovement(Vector2.up, ref vec);

            Assert.AreNotEqual(vec, Vector3.zero);
        }

        [UnityTest]
        public IEnumerator GravityProvider_ReturnsNonZeroVector_WhenNoRaycastProviderOrNotGrounded()
        {
            var provider = _dummyObject.AddComponent<DefaultGravityProvider>();

            yield return null;

            Vector3 gravity = provider.ComputeGravity();

            Assert.AreNotEqual(gravity, Vector3.zero);
        }

        [UnityTest]
        public IEnumerator RotationProvider_ReturnsNonZeroVector_OnAnyInput()
        {
            var provider = _dummyObject.AddComponent<DefaultCameraRotationProvider>();

            yield return null;

            Vector2 init = Vector2.zero;
            Vector2 input = Vector2.left;
            Vector2 rotation = provider.CalculateRotation(input, init);

            Assert.AreNotEqual(init, rotation);
        }
    }
}
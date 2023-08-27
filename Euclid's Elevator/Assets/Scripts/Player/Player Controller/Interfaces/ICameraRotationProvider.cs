using UnityEngine;

namespace DeathFloor.Camera.Rotation
{
    internal interface ICameraRotationProvider
    {
        public Vector2 CalculateRotation(Vector2 input, Vector2 rotation);

        public Vector2 ResetAngle();
    }
}
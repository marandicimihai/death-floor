using UnityEngine;

namespace DeathFloor.Camera.Rotation
{
    public interface ICameraRotationProvider
    {
        public Vector2 CalculateRotation(Vector2 input, Vector2 rotation);

        public Vector2 ResetAngle();
    }
}
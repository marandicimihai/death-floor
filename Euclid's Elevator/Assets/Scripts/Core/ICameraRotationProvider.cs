using UnityEngine;

namespace DeathFloor.Camera.Rotation
{
    public interface ICameraRotationProvider
    {
        public Vector2 CalculateRotation(Vector2 input);

        public Vector2 ResetAngle();
    }
}
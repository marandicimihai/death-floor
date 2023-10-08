using DeathFloor.Utilities;

namespace DeathFloor.Camera
{
    public interface IVFX : IToggleable
    {
        public void BlackScreen(bool show, float fadeTime = 0f);
        public void InsanityEffect(bool show, float fadeTime = 0f);
        public void CameraShake(float time, float amplitude);
        public void ResetEffects();
    }
}
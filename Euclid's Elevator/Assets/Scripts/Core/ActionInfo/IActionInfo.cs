using DeathFloor.Utilities;

namespace DeathFloor.HUD
{
    public interface IActionInfo : IToggleable
    {
        public void DisplayText(string text);
        public void ClearText();
        public void StartSlider(object caller, SliderType sliderType);
        public void SetSliderValue(object caller, float value);
        public void StopSlider(object caller);
    }
}
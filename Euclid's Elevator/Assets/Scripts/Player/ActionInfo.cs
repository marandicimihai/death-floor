using UnityEngine;
using UnityEngine.UI;

namespace DeathFloor.HUD
{
    internal class ActionInfo : MonoBehaviour, IActionInfo
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _circleSliderObject;
        [SerializeField] private Image _circleSlider;
        [SerializeField] private GameObject _lockedSliderObject;
        [SerializeField] private Image _lockedSlider;
        [SerializeField] private GameObject _unlockedSliderObject;
        [SerializeField] private Image _unlockedSlider;

        private Utilities.Logger.ILogger _logger;

        private bool _canDisplay;
        private bool _active;
        private object _currentUser;
        private Image _currentSlider;
        private GameObject _currentSliderObject;

        private void Start()
        {
            Debug.LogError("Fix logger here.");

            Enable();
        }

        public void DisplayText(string text)
        {
            _text.text = text;
        }

        public void ClearText()
        {
            _text.text = string.Empty;
        }

        public void Disable()
        {
            if (_active)
            {
                _currentSliderObject.SetActive(false);
                _active = false;
            }
            _canDisplay = false;
            ClearText();
        }

        public void Enable()
        {
            if (_currentSliderObject != null && _currentSlider != null && _currentUser != null)
            {
                _currentSliderObject.SetActive(true);
                _active = true;
            }
            _canDisplay = true;
        }

        public void StartSlider(object caller, SliderType sliderType)
        {
            if (_active)
            {
                _logger.Debug("A slider is already active!");
                return;
            }

            if (!_canDisplay) return;

            if (sliderType == SliderType.Circle)
            {
                _currentSliderObject = _circleSliderObject;
                _currentSlider = _circleSlider;
            }
            else if (sliderType == SliderType.LockLocked)
            {
                _currentSliderObject = _lockedSliderObject;
                _currentSlider = _lockedSlider;
            }
            else if (sliderType == SliderType.LockUnlocked)
            {
                _currentSliderObject = _unlockedSliderObject;
                _currentSlider = _unlockedSlider;
            }

            _currentSliderObject.SetActive(true);
            _currentSlider.fillAmount = 0;
            _currentUser = caller;

            _active = true;
        }

        public void SetSliderValue(object caller, float value)
        {
            if (caller != _currentUser)
            {
                _logger.Debug($"Someone else {_logger.Italic("is already using a slider")}.");
                return;
            }

            _currentSlider.fillAmount = value;
        }

        public void StopSlider(object caller)
        {
            if (caller != _currentUser)
            {
                _logger.Debug($"Someone else attempted to stop the slider!");
                return;
            }

            _currentSliderObject.SetActive(false);
            _active = false;
        }
    }
}
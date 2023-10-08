using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace DeathFloor.Camera
{
    internal class VFX : MonoBehaviour, IVFX
    {
        [SerializeField] private float _defaultBlackScreenFadeTime = 1;
        [SerializeField] private float _defaultInsanityEffectFadeTime = 1;

        [SerializeField] private Volume _blackScreenVolume;
        [SerializeField] private Volume _insanityVolume;

        [SerializeField] private Transform _camera;

        private bool _enabled;

        private bool _showInsanity = false;
        private float _insanityFadeTime = 1;

        private bool _showBlackScreen = false;
        private float _blackScreenFadeTime = 1;

        private void Start()
        {
            StartCoroutine(InsanityEffectRoutine());
            StartCoroutine(BlackScreenRoutine());
            Enable();
        }

        public void BlackScreen(bool show, float fadeTime = 0)
        {
            if (fadeTime < 0)
                throw new ArgumentException("Fade time negative.", nameof(fadeTime));

            if (fadeTime == 0)
                fadeTime = _defaultBlackScreenFadeTime;

            if (!_enabled || _blackScreenVolume == null) return;

            _showBlackScreen = show;
            _blackScreenFadeTime = fadeTime;
        }

        private IEnumerator BlackScreenRoutine()
        {
            float t = _blackScreenVolume.weight;

            while (true)
            {
                if (_showBlackScreen)
                {
                    t += Time.deltaTime / _blackScreenFadeTime;
                }
                else
                {
                    t -= Time.deltaTime / _blackScreenFadeTime;
                }

                t = Mathf.Clamp01(t);

                _blackScreenVolume.weight = t;

                yield return null;
            }
        }

        public void CameraShake(float time, float amplitude)
        {
            if (amplitude < 0)
                throw new ArgumentException("Amplitude negative", nameof(amplitude));

            if (amplitude == 0 || !_enabled) return;

            StopCoroutine(nameof(ExecuteCameraShake));
            StartCoroutine(ExecuteCameraShake(time, amplitude));
        }

        private IEnumerator ExecuteCameraShake(float time, float amplitude)
        {
            float t = 0;

            while (t <= time)
            {
                t += Time.deltaTime;

                _camera.transform.localPosition = Vector3.zero + UnityEngine.Random.insideUnitSphere * amplitude;

                yield return null;
            }

            _camera.transform.localPosition = Vector3.zero;
        }

        public void InsanityEffect(bool show, float fadeTime = 0)
        {
            if (fadeTime < 0)
                throw new ArgumentException("Fade time negative.", nameof(fadeTime));

            if (fadeTime == 0)
                fadeTime = _defaultInsanityEffectFadeTime;

            if (!_enabled || _insanityVolume == null) return;

            _showInsanity = show;
            _insanityFadeTime = fadeTime;
        }

        private IEnumerator InsanityEffectRoutine()
        {
            float t = _insanityVolume.weight;

            while (true)
            {
                if (_showInsanity)
                {
                    t += Time.deltaTime / _insanityFadeTime;
                }
                else
                {
                    t -= Time.deltaTime / _insanityFadeTime;
                }

                t = Mathf.Clamp01(t);

                _insanityVolume.weight = t;

                yield return null;
            }
        }

        public void ResetEffects()
        {
            BlackScreen(false);
            InsanityEffect(false);
        }

        public void Disable()
        {
            _enabled = false;

            _showBlackScreen = false;
            _showInsanity = false;
            _blackScreenVolume.weight = 0;
            _insanityVolume.weight = 0;
        }

        public void Enable()
        {
            _enabled = true;
        }
    }
}
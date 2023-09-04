using System.Collections.Generic;
using DeathFloor.Popups;
using UnityEngine;

namespace DeathFloor.HUD
{
    internal struct PopupQueueObject
    {
        public RectTransform Rect;
        public PopupProperties Properties;
        public Vector2 InitialPosition;
    }

    internal class PopupDisplayer : MonoBehaviour, IPopupDisplayer
    {
        [SerializeField] private float _displayTime;
        [SerializeField] private float _slideTime;

        private bool _canDisplay;
        private Queue<PopupQueueObject> _displayQueue;

        private PopupQueueObject _current;

        private bool _slidingIn;
        private float _slideInAmount;

        private bool _displaying;
        private float _displayTimeElapsed;

        private bool _slidingOut;
        private float _slideOutAmount;

        private void Start()
        {
            _displayQueue = new();
            Enable();
        }

        private void Update()
        {
            if (_canDisplay)
            {
                if (!_slidingIn && !_slidingOut && !_displaying && _displayQueue.Count > 0)
                {
                    _slidingIn = true;
                    _current = _displayQueue.Dequeue();
                    _current.Rect = Instantiate(_current.Properties.Prefab, transform).GetComponent<RectTransform>();
                    _current.InitialPosition = new Vector2(0, GetComponent<RectTransform>().rect.height / 2 + _current.Rect.rect.height / 2);
                }
                if (_slidingIn)
                {
                    _slideInAmount += Time.deltaTime / _slideTime;
                    _slideInAmount = Mathf.Clamp01(_slideInAmount);
                    _current.Rect.anchoredPosition = Vector2.Lerp(_current.InitialPosition, Vector2.zero, _slideInAmount);
                    if (_slideInAmount >= 1f)
                    {
                        _slideInAmount = 0;
                        _slidingIn = false;
                        _displaying = true;
                    }
                }
                else if (_displaying)
                {
                    _displayTimeElapsed += Time.deltaTime;
                    if (_displayTimeElapsed >= _displayTime)
                    {
                        _displayTimeElapsed = 0;
                        _displaying = false;
                        _slidingOut = true;
                    }
                }
                else if (_slidingOut)
                {
                    _slideOutAmount += Time.deltaTime / _slideTime;
                    _slideOutAmount = Mathf.Clamp01(_slideOutAmount);
                    _current.Rect.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(GetComponent<RectTransform>().rect.width / 2 + _current.Rect.rect.width / 2, 0), _slideOutAmount);
                    if (_slideOutAmount >= 1f)
                    {
                        _slideOutAmount = 0;
                        Destroy(_current.Rect.gameObject);
                        _slidingOut = false;
                    }
                }
            }
        }

        public void DisplayPopup(PopupProperties popupProperties)
        {
            if (!_canDisplay) return;

            _displayQueue.Enqueue(new PopupQueueObject()
            {
                Properties = popupProperties,
            });
        }

        public void Disable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            _canDisplay = false;
        }

        public void Enable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            _canDisplay = true;
        }
    }
}
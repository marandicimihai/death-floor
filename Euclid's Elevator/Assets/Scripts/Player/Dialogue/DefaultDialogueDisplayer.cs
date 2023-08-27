using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DeathFloor.Dialogue
{
    internal class DefaultDialogueDisplayer : MonoBehaviour, IDialogueDisplayer, IToggleable
    {
        [SerializeField] private Text _textContainer;
        [SerializeField] private float _timeBetweenLetters;
        [SerializeField] private float _timeLasting;

        private IUnityService _unityService;

        private bool _canDisplay;

        private bool _typing;
        private bool _lasting;
        private LineProperties _currentLine;
        private Action<LineProperties> _callback;
        private int _currentLetter;
        private float _letterTimeElapsed;
        private float _lastingTimeElapsed;

        private void Start()
        {
            _unityService = new UnityService();

            Enable();
        }

        private void Update()
        {
            if (_canDisplay)
            {
                if (_typing)
                {
                    _letterTimeElapsed += _unityService.GetDeltaTime();
                    if (_letterTimeElapsed >= _timeBetweenLetters)
                    {
                        try
                        {
                            _textContainer.text += _currentLine.Text[_currentLetter];
                        }
                        catch
                        {
                            _typing = false;
                            _lasting = true;
                            _callback?.Invoke(_currentLine);
                        }
                        _currentLetter++;
                        _letterTimeElapsed = 0;
                    }
                }
                else if (_lasting)
                {
                    _lastingTimeElapsed += _unityService.GetDeltaTime();
                    if (_lastingTimeElapsed >= _timeLasting)
                    {
                        Clear();
                    }
                }
            }
        }

        public void DisplayDialogue(LineProperties lineProperties, Action<LineProperties> cb)
        {
            Clear();

            _typing = true;
            _currentLine = lineProperties;
            _callback = cb;
        }

        private void Clear()
        {
            _textContainer.text = string.Empty;
            _typing = false;
            _lasting = false;
            _currentLine = null;
            _callback = null;
            _currentLetter = 0;
            _letterTimeElapsed = 0;
            _lastingTimeElapsed = 0;
        }

        public void Enable()
        {
            _canDisplay = true;
        }

        public void Disable()
        {
            Clear();
            _canDisplay = false;
        }
    }
}
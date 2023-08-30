using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using System;
using System.Collections.Generic;
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
        private Queue<LineProperties> _linesToDisplay;
        private Queue<Action<LineProperties>> _callbacks;
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
                            _textContainer.text += _linesToDisplay?.Peek().Text[_currentLetter];
                            _currentLetter++;
                        }
                        catch
                        {
                            _callbacks?.Dequeue()?.Invoke(_linesToDisplay?.Dequeue());
                            if (_callbacks.Count == 0)
                            {
                                _linesToDisplay.Dequeue();
                                _typing = false;
                                _lasting = true;
                            }
                            _currentLetter = 0;
                        }
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
            _linesToDisplay.Enqueue(lineProperties);
            _callbacks.Enqueue(cb);
        }

        public void DisplayAdditive(LineProperties lineProperties, Action<LineProperties> cb)
        {
            _typing = true;
            _lasting = false;

            _linesToDisplay.Enqueue(lineProperties);
            _callbacks.Enqueue(cb);
        }

        private void Clear()
        {
            _textContainer.text = string.Empty;
            _typing = false;
            _lasting = false;
            _linesToDisplay?.Clear();
            _callbacks?.Clear();
            _currentLetter = 0;
            _letterTimeElapsed = 0;
            _lastingTimeElapsed = 0;
        }

        public void Enable()
        {
            _linesToDisplay = new();
            _callbacks = new();
            _canDisplay = true;
        }

        public void Disable()
        {
            Clear();
            _canDisplay = false;
        }
    }
}
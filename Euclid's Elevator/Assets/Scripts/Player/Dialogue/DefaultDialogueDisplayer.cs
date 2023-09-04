using DeathFloor.Dialogue;
using DeathFloor.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeathFloor.HUD
{
    internal struct LineQueueObject
    {
        public LineProperties Line;
        public Action<LineProperties> Callback;
    }

    internal class DefaultDialogueDisplayer : MonoBehaviour, IDialogueDisplayer
    {
        [SerializeField] private Text _textContainer;
        [SerializeField] private float _timeBetweenLetters;
        [SerializeField] private float _timeLasting;

        private bool _canDisplay;

        private bool _typing;
        private bool _lasting;
        private Queue<LineQueueObject> _lineQueue;
        private int _currentLetter;
        private float _letterTimeElapsed;
        private float _lastingTimeElapsed;

        private void Start()
        {
            Enable();
        }

        private void Update()
        {
            if (_canDisplay)
            {
                if (_typing)
                {
                    _letterTimeElapsed += Time.deltaTime;
                    if (_letterTimeElapsed >= _timeBetweenLetters)
                    {
                        try
                        {
                            _textContainer.text += _lineQueue?.Peek().Line.Text[_currentLetter];
                            _currentLetter++;
                        }
                        catch
                        {
                            if (_lineQueue.Count > 0)
                            {
                                LineQueueObject queueObject = _lineQueue.Dequeue();
                                queueObject.Callback?.Invoke(queueObject.Line);
                            }

                            if (_lineQueue.Count == 0)
                            {
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
                    _lastingTimeElapsed += Time.deltaTime;
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
            _lineQueue.Enqueue(new LineQueueObject() 
            {
                Line = lineProperties, 
                Callback = cb 
            });
        }

        public void DisplayAdditive(LineProperties lineProperties, Action<LineProperties> cb)
        {
            _typing = true;
            _lasting = false;

            _lineQueue.Enqueue(new LineQueueObject()
            {
                Line = lineProperties,
                Callback = cb
            });
        }

        private void Clear()
        {
            _textContainer.text = string.Empty;
            _typing = false;
            _lasting = false;
            _lineQueue?.Clear();
            _currentLetter = 0;
            _letterTimeElapsed = 0;
            _lastingTimeElapsed = 0;
        }

        public void Enable()
        {
            _lineQueue = new();
            _canDisplay = true;
        }

        public void Disable()
        {
            Clear();
            _canDisplay = false;
        }
    }
}
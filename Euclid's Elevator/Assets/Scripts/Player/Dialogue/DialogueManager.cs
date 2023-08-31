using DeathFloor.Utilities;
using DeathFloor.Utilities.Logger;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DeathFloor.Dialogue
{
    internal class DialogueManager : MonoBehaviour, IToggleable, IDialogueManager
    {
        [SerializeField] private Optional<MonoBehaviour> _dialogueDisplayerBehaviour;
        [SerializeField] private Optional<MonoBehaviour> _loggerFactoryBehaviour;
        
        private bool _canSay;

        private IOptionalAssigner _optionalAssigner;
        private ILoggerFactory _loggerFactory;
        private Utilities.Logger.ILogger _logger;
        private IDialogueDisplayer _dialogueDisplayer;

        private List<LineProperties> _usedLines;
        private Queue<LineProperties> _displaying;

        private void Start()
        {
            _optionalAssigner ??= new OptionalAssigner(this);

            _loggerFactory ??= _optionalAssigner.AssignUsingGetComponent<ILoggerFactory>(_loggerFactoryBehaviour);
            _logger ??= _loggerFactory.CreateLogger();

            _usedLines ??= new List<LineProperties>();

            _dialogueDisplayer ??= _optionalAssigner.AssignUsingGetComponent<IDialogueDisplayer>(_dialogueDisplayerBehaviour);

            Enable();
        }

        public void Say(LineProperties lineProperties)
        {
            if (!_canSay) return;

            if (lineProperties.OneTime)
            {
                if (_usedLines.Contains(lineProperties))
                {
                    _logger.Log($"The line {lineProperties.Name} has already been used.");
                    return;
                }
            }

            _displaying.Enqueue(lineProperties);
            _dialogueDisplayer.DisplayDialogue(lineProperties, FinishedDisplaying);
        }

        public void SayAdditive(LineProperties lineProperties)
        {
            if (!_canSay) return;

            if (lineProperties.OneTime)
            {
                if (_displaying.Contains(lineProperties))
                {
                    _logger.Log($"The line {lineProperties.Name} is in display queue.");
                    return;
                }

                if (_usedLines.Contains(lineProperties))
                {
                    _logger.Log($"The line {lineProperties.Name} has already been used.");
                    return;
                }
            }

            _displaying.Enqueue(lineProperties);
            _dialogueDisplayer.DisplayAdditive(lineProperties, FinishedDisplaying);
        }

        private void FinishedDisplaying(LineProperties properties)
        {
            if (properties.OneTime)
            {
                AddToUsed(properties);
            }
            RemoveFromQueue(properties);
        }

        private void AddToUsed(LineProperties lineProperties)
        {
            if (_usedLines.Contains(lineProperties))
            {
                _logger.LogWarning($"Line {lineProperties.Name} is already used.");
                return;
            }

            _usedLines.Add(lineProperties);
        }

        private void RemoveFromQueue(LineProperties lineProperties)
        {
            if (_displaying.Peek() == lineProperties)
            {
                _displaying.Dequeue();
            }
            else
            {
                _logger.Log("The line that has finished being displayed is not the first in display queue.");
            }
        }

        public void Enable()
        {
            _displaying = new();
            _canSay = true;
        }

        public void Disable()
        {
            _displaying?.Clear();
            _canSay = false;
        }
    }
}
using DeathFloor.Utilities;
using DeathFloor.Utilities.Logger;
using System.Collections.Generic;
using UnityEngine;

namespace DeathFloor.Dialogue
{
    internal class DialogueManager : MonoBehaviour, IToggleable, IDialogueManager
    {
        [SerializeField, RequireInterface(typeof(IDialogueDisplayer))] private Object _dialogueDisplayer;
        
        private bool _canSay;

        private Utilities.Logger.ILogger _logger;
        private IDialogueDisplayer _dialogueDisplayerInterface;

        private List<LineProperties> _usedLines;
        private Queue<LineProperties> _displaying;

        private void Start()
        {
            _logger = new DefaultLogger();

            _usedLines ??= new List<LineProperties>();

            _dialogueDisplayerInterface = _dialogueDisplayer as IDialogueDisplayer;

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
            _dialogueDisplayerInterface.DisplayDialogue(lineProperties, FinishedDisplaying);
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
            _dialogueDisplayerInterface.DisplayAdditive(lineProperties, FinishedDisplaying);
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
using DeathFloor.HUD;
using DeathFloor.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DeathFloor.Dialogue
{
    internal class DialogueManager : MonoBehaviour, IDialogueManager
    {
        [SerializeField, RequireInterface(typeof(IDialogueDisplayer))] private Object _dialogueDisplayer;
        
        private bool _canSay;

        private IDialogueDisplayer _dialogueDisplayerInterface;

        private List<LineProperties> _usedLines;
        private Queue<LineProperties> _displaying;

        private void Start()
        {
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
                    return;
                }
            }

            _displaying.Enqueue(lineProperties);
            _dialogueDisplayerInterface?.DisplayDialogue(lineProperties, FinishedDisplaying);
        }

        public void SayAdditive(LineProperties lineProperties)
        {
            if (!_canSay) return;

            if (lineProperties.OneTime)
            {
                if (_displaying.Contains(lineProperties))
                {
                    return;
                }

                if (_usedLines.Contains(lineProperties))
                {
                    return;
                }
            }

            _displaying.Enqueue(lineProperties);
            _dialogueDisplayerInterface?.DisplayAdditive(lineProperties, FinishedDisplaying);
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
        }

        public void Enable()
        {
            _dialogueDisplayerInterface?.Enable();
            _displaying = new();
            _canSay = true;
        }

        public void Disable()
        {
            _dialogueDisplayerInterface?.Disable();
            _displaying?.Clear();
            _canSay = false;
        }
    }
}
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
        [SerializeField] LineProperties aline;
        private bool _canSay;

        private IOptionalAssigner _optionalAssigner;
        private ILoggerFactory _loggerFactory;
        private Utilities.Logger.ILogger _logger;
        private List<LineProperties> _usedLines;
        private IDialogueDisplayer _dialogueDisplayer;

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

            if (_usedLines.Contains(lineProperties))
            {
                _logger.Log($"The line {lineProperties.Name} has already been used.");
                return;
            }

            _dialogueDisplayer.DisplayDialogue(lineProperties, lineProperties.OneTime ? AddToUsed : null);
        }

        public void SayAdditive(LineProperties lineProperties)
        {
            if (!_canSay) return;

            if (_usedLines.Contains(lineProperties))
            {
                _logger.Log($"The line {lineProperties.Name} has already been used.");
                return;
            }

            _dialogueDisplayer.DisplayAdditive(lineProperties, lineProperties.OneTime ? AddToUsed : null);
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

        public void Enable()
        {
            _canSay = true;
        }

        public void Disable()
        {
            _canSay = false;
        }
    }
}
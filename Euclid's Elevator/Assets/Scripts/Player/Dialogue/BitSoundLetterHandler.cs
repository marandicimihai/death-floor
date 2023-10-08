using DeathFloor.Audio;
using DeathFloor.Utilities;
using System.Linq;
using UnityEngine;

namespace DeathFloor.Dialogue
{
    internal class BitSoundLetterHandler : MonoBehaviour, IDialogueLetterHandler
    {
        [SerializeField] private int _charsPerSound;
        [SerializeField] private char[] _toIgnore;
        [SerializeField] private SoundProperties[] _bitSounds;
        [SerializeField, RequireInterface(typeof(IAudioPlayer))] private Object _audioPlayer;

        private int _current;
        private IAudioPlayer _player;

        private void Start()
        {
            _player = _audioPlayer as IAudioPlayer;
        }

        public void OnLetter(char letter, LineProperties current = null)
        {
            if (!_toIgnore.Contains(letter))
            {
                if (_current >= _charsPerSound)
                {
                    if (current == null || current.Sound)
                        _player?.PlayRandomAtAudioPlayer(_bitSounds);

                    _current = 0;
                }
                else
                {
                    _current++;
                }
            }
        }
    }
}
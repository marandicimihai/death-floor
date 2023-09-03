namespace DeathFloor.Audio
{
    public interface IAudioThread
    {
        public IAudioThread AddToQueue(SoundProperties properties);
        public void SetVolume(float volume);
        /// <summary>
        /// Gets destroyed.
        /// </summary>
        public void Clear();
        public void DestroyObjectWhenFinished();
    }
}
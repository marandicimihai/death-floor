namespace DeathFloor.Audio
{
    public interface IAudioThread
    {
        public void DestroyObjectWhenFinished();
        /// <summary>
        /// Gets destroyed.
        /// </summary>
        public void Clear();
        public IAudioThread AddToQueue(SoundProperties properties);
    }
}
namespace DeathFloor.SaveSystem
{
    public class InsanityData : SaveData
    {
        public float Insanity 
        {
            get => insanity;
            set => insanity = value;
        }

        public InsanityData(float insanity)
        {
            Insanity = insanity;
        }
    }
}
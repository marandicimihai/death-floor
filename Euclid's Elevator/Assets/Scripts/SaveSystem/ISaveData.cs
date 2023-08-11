namespace DeathFloor.SaveSystem
{
    public interface ISaveData<T> where T : SaveData
    {
        public bool CanSave { get; }

        public void OnFirstTimeLoaded();

        public T OnSaveData();

        public void LoadData(T data);

    }
}
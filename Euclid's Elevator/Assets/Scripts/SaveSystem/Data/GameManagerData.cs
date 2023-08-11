using UnityEngine;

namespace DeathFloor.SaveSystem
{
    public class GameManagerData : SaveData
    {
        public int Stage 
        { 
            get => stage;
            set => stage = value;
        }

        public int GameStage
        {
            get => gameStage;
            set => gameStage = value;
        }

        public GameManagerData(int stage, int gameStage)
        {
            Stage = stage;
            GameStage = gameStage;
        }
    }
}
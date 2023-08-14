using System.Collections.Generic;

namespace DeathFloor.SaveSystem
{
    public class PopUpData : SaveData
    {
        public List<PopUpProperties> UsedPopUps
        {
            get => GetPopUpProperties(usedPopUps);
            set => SetPopUpNames(value);
        }

        List<PopUpProperties> GetPopUpProperties(string[] names)
        {
            List<PopUpProperties> arr = new();

            for (int i = 0; i < names.Length; i++)
            {
                arr.Add(ScriptableObjectCatalogue.Instance.GetPopUpByName(names[i]));
            }

            return arr;
        }

        void SetPopUpNames(List<PopUpProperties> popUps)
        {
            usedPopUps = new string[popUps.Count];

            for (int i = 0; i < popUps.Count; i++)
            {
                usedPopUps[i] = ScriptableObjectCatalogue.Instance.GetName(popUps[i]);
            }
        }

        public PopUpData(List<PopUpProperties> popUps)
        {
            UsedPopUps = popUps;
        }

        public PopUpData()
        {

        }
    }
}
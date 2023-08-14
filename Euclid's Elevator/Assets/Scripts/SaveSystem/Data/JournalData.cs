using System.Collections.Generic;

namespace DeathFloor.SaveSystem
{
    public class JournalData : SaveData
    {
        public List<JournalPage> Pages
        {
            get => GetPages(pages);
            set => SetPages(value);
        }

        List<JournalPage> GetPages(string[] names)
        {
            List<JournalPage> pages = new();

            for (int i = 0; i < names.Length; i++)
            {
                pages.Add(ScriptableObjectCatalogue.Instance.GetJournalPageByName(names[i]));
            }

            return pages;
        }

        void SetPages(List<JournalPage> pages)
        {
            this.pages = new string[pages.Count];

            for (int i = 0; i < pages.Count; i++)
            {
                this.pages[i] = ScriptableObjectCatalogue.Instance.GetName(pages[i]);
            }
        }

        public JournalData(List<JournalPage> pages)
        {
            Pages = pages;
        }

        public JournalData()
        {

        }
    }
}
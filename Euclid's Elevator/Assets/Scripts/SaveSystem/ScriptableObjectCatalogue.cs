using System.Linq;
using UnityEngine;

namespace DeathFloor.SaveSystem
{
    public class ScriptableObjectCatalogue : Singleton<ScriptableObjectCatalogue>
    {
        //<======================>
        //WHEN ADDING NEW ARRAY UPDATE FUNCTIONS!
        //<======================>

        [SerializeField] ItemProperties[] items;
        [SerializeField] Line[] lines;
        [SerializeField] PopUpProperties[] popups;
        [SerializeField] JournalPage[] pages;

        public ItemProperties GetItemByName(string name)
        {
            return items.First(item => item.name == name);
        }

        public Line GetLineByName(string name)
        {
            return lines.First(line => line.name == name);
        }

        public PopUpProperties GetPopUpByName(string name)
        {
            return popups.First(popup => popup.name == name);
        }

        public JournalPage GetJournalPageByName(string name)
        {
            return pages.First(page => page.name == name);
        }

        public string GetName<T>(T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject is ItemProperties)
            {
                ItemProperties p = scriptableObject as ItemProperties;
                return p.name;
            }
            if (scriptableObject is Line)
            {
                Line l = scriptableObject as Line;
                return l.name;
            }
            if (scriptableObject is JournalPage)
            {
                JournalPage j = scriptableObject as JournalPage;
                return j.name;
            }
            if (scriptableObject is PopUpProperties)
            {
                PopUpProperties p = scriptableObject as PopUpProperties;
                return p.name;
            }
            return "";
        }
    }
}
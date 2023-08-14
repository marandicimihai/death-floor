using System.Collections.Generic;

namespace DeathFloor.SaveSystem
{
    public class DialogueData : SaveData
    {
        public List<Line> UsedLines
        {
            get => GetLines(usedLines);
            set => SetLineNames(value);
        }
        
        List<Line> GetLines(string[] names)
        {
            List<Line> lines = new();

            for (int i = 0; i < names.Length; i++)
            {
                lines.Add(ScriptableObjectCatalogue.Instance.GetLineByName(names[i]));
            }

            return lines;
        }

        void SetLineNames(List<Line> lines)
        {
            usedLines = new string[lines.Count];

            for (int i = 0; i < lines.Count; i++)
            {
                usedLines[i] = ScriptableObjectCatalogue.Instance.GetName(lines[i]);
            }
        }

        public DialogueData(List<Line> lines)
        {
            UsedLines = lines;
        }

        public DialogueData()
        {

        }
    }
}
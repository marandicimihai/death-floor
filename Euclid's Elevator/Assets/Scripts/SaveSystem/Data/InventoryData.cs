namespace DeathFloor.SaveSystem
{
    public class InventoryData : SaveData
    {
        public ItemProperties[] ItemProperties
        {
            get => GetProperties(holdingitems);
            set => SetProperties(value);
        }

        ItemProperties[] GetProperties(string[] names)
        {
            ItemProperties[] arr = new ItemProperties[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] != string.Empty)
                {
                    arr[i] = ScriptableObjectCatalogue.Instance.GetItemByName(names[i]);
                }
                else
                {
                    arr[i] = null;
                }
            }

            return arr;
        }

        void SetProperties(ItemProperties[] props)
        {
            holdingitems = new string[props.Length];

            for (int i = 0; i < props.Length; i++)
            {
                if (props[i] != null)
                {
                    holdingitems[i] = ScriptableObjectCatalogue.Instance.GetName(props[i]);
                }
                else
                {
                    holdingitems[i] = string.Empty;
                }
            }
        }

        public string[][] ItemVariables
        {
            get => GetVariables(holdingLengths, holdingVariables);
            set => SetVariables(value);
        }

        string[][] GetVariables(int[] lengths, string[] vars)
        {
            int count = 0;
            int index = 0;
            string[][] strings = new string[lengths.Length][];
            strings[index] = new string[lengths[index]];

            for (int i = 0; i < vars.Length; i++)
            {
                if (count >= lengths[index])
                {
                    count = 0;
                    index++;
                    strings[index] = new string[lengths[index]];
                }

                strings[index][count] = vars[i];

                count++;
            }

            return strings;
        }

        void SetVariables(string[][] values)
        {
            int count = 0;

            holdingLengths = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                holdingLengths[i] = values[i].Length;
                for (int j = 0; j < values[i].Length; j++)
                {
                    count++;
                }
            }

            holdingVariables = new string[count];

            int index = 0;

            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    holdingVariables[index] = values[i][j];
                    index++;
                }
            }
        }

        public InventoryData(ItemProperties[] itemProperties, string[][] itemVariables)
        {
            ItemProperties = itemProperties;
            ItemVariables = itemVariables;
        }
    }
}
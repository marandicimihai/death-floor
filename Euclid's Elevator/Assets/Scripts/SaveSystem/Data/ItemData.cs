using DeathFloor.Conversion;
using UnityEngine;

namespace DeathFloor.SaveSystem
{
    public class ItemData : SaveData
    {
        public ItemProperties[] ItemsProperties
        {
            get => StringsToItemProperties(spawneditems);
            set => ItemPropertiesToStrings(value);
        }

        ItemProperties[] StringsToItemProperties(string[] strings)
        {
            ItemProperties[] props = new ItemProperties[strings.Length];

            for (int i = 0; i < strings.Length; i++)
            {
                props[i] = ScriptableObjectCatalogue.Instance.GetItemByName(strings[i]);
            }

            return props;
        }

        void ItemPropertiesToStrings(ItemProperties[] arr)
        {
            spawneditems = new string[arr.Length];
            
            for (int i = 0; i < arr.Length; i++)
            {
                spawneditems[i] = ScriptableObjectCatalogue.Instance.GetName(arr[i]);
            }
        }

        public Vector3[] ItemPositions
        {
            get => Converter.FloatArrayToVector3Array(spawneditemPositions);
            set => spawneditemPositions = Converter.Vector3ArrayToFloatArray(value);
        }

        public string[][] ItemVariables
        {
            get => GetVariables(spawnedlengths, spawnedvariables);
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

            spawnedlengths = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                spawnedlengths[i] = values[i].Length;
                for (int j = 0; j < values[i].Length; j++)
                {
                    count++;
                }
            }

            spawnedvariables = new string[count];

            int index = 0;

            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    spawnedvariables[index] = values[i][j];
                    index++;
                }
            }
        }

        public ItemData(ItemProperties[] itemsProperties, Vector3[] itemPositions, string[][] itemVariables)
        {
            ItemsProperties = itemsProperties;
            ItemPositions = itemPositions;
            ItemVariables = itemVariables;
        }
    }
}
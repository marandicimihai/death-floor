using UnityEngine;

namespace DeathFloor.Conversion
{
    public static class Converter
    {
        public static float[] Vector2ToFloatArray(Vector2 vector)
        {
            return new float[] { vector.x, vector.y };
        }

        public static Vector2 FloatArrayToVector2(float[] arr)
        {
            return new Vector2(arr[0], arr[1]);
        }

        public static float[] Vector3ToFloatArray(Vector3 vector)
        {
            return new float[] { vector.x, vector.y, vector.z };
        }

        public static Vector3 FloatArrayToVector3(float[] floats)
        {
            return new Vector3(floats[0], floats[1], floats[2]);
        }

        public static float[] QuaternionToFloatArray(Quaternion quaternion)
        {
            return new float[] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
        }

        public static Quaternion FloatArrayToQuaternion(float[] floats)
        {
            return new Quaternion(floats[0], floats[1], floats[2], floats[3]);
        }

        public static float[] Vector3ArrayToFloatArray(Vector3[] arr)
        {
            float[] newArr = new float[arr.Length * 3];

            int newId = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[newId++] = arr[i].x;
                newArr[newId++] = arr[i].y;
                newArr[newId++] = arr[i].z;
            }

            return newArr;
        }

        public static Vector3[] FloatArrayToVector3Array(float[] arr)
        {
            if (arr.Length % 3 != 0)
            {
                throw new System.Exception("Conversion failed. Array length can't be divided by 3.");
            }
            Vector3[] newArr = new Vector3[arr.Length / 3];

            for (int i = 0; i < newArr.Length; i++)
            {
                newArr[i] = new Vector3(arr[i] * 3, arr[i] * 3 + 1, arr[i] + 2);
            }

            return newArr;
        }
    }
}
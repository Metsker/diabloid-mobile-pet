using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector3) =>
            new (vector3.x, vector3.y, vector3.z);

        public static Vector3 AddY(this Vector3 vector3, float y) =>
            new (vector3.x, vector3.y + y, vector3.z);
        
        public static Vector3 ChangeY(this Vector3 vector3, float y) =>
            new (vector3.x, y, vector3.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new (vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);
        
        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);
    }
}
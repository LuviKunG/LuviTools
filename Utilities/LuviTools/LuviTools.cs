using UnityEngine;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace LuviKunG
{
    public static class LuviTools
    {
        public static T InstantiateAsChild<T>(T gameObject, Transform parent) where T : MonoBehaviour
        {
            GameObject obj = Object.Instantiate(gameObject.gameObject) as GameObject;
            obj.transform.name = gameObject.name;
            obj.transform.parent = parent;
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            return obj.GetComponent<T>();
        }

        public static T InstantiateAsChildLocal<T>(T gameObject, Transform parent) where T : MonoBehaviour
        {
            GameObject obj = Object.Instantiate(gameObject.gameObject) as GameObject;
            obj.transform.name = gameObject.name;
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            return obj.GetComponent<T>();
        }

        public static T InstantiateAsChild<T>(T gameObject, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale) where T : MonoBehaviour
        {
            GameObject obj = Object.Instantiate(gameObject.gameObject) as GameObject;
            obj.transform.name = gameObject.name;
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.localScale = scale;
            return obj.GetComponent<T>();
        }

        public static T InstantiateAsChildLocal<T>(T gameObject, Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 scale) where T : MonoBehaviour
        {
            GameObject obj = Object.Instantiate(gameObject.gameObject) as GameObject;
            obj.transform.name = gameObject.name;
            obj.transform.parent = parent;
            obj.transform.localPosition = localPosition;
            obj.transform.localRotation = localRotation;
            obj.transform.localScale = scale;
            return obj.GetComponent<T>();
        }

        public static void DestroyAllInChild(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static T RandomObject<T>(params T[] array)
        {
            int rand = UnityRandom.Range(0, array.Length);
            return array[rand];
        }

        public static T RandomObject<T>(List<T> list)
        {
            int rand = UnityRandom.Range(0, list.Count);
            return list[rand];
        }

        public static Vector3 RandomInBounds(ref Bounds bound)
        {
            return bound.center + new Vector3(((UnityRandom.value - 0.5f) * bound.extents.x), ((UnityRandom.value - 0.5f) * bound.extents.y), ((UnityRandom.value - 0.5f) * bound.extents.z));
        }

        public enum StringRandomType
        {
            Default,
            LowerCase,
            UpperCase,
            UpperAndLowerCase,
            LowerCaseWithNumber,
            UpperCaseWithNumber
        }

        public static string RandomString(int length, StringRandomType type = StringRandomType.Default)
        {
            string valid;
            switch (type)
            {
                case StringRandomType.LowerCase:
                    valid = "abcdefghijklmnopqrstuvwxyz";
                    break;
                case StringRandomType.UpperCase:
                    valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case StringRandomType.UpperAndLowerCase:
                    valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case StringRandomType.LowerCaseWithNumber:
                    valid = "abcdefghijklmnopqrstuvwxyz0123456789";
                    break;
                case StringRandomType.UpperCaseWithNumber:
                    valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    break;
                default:
                    valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    break;
            }
            string res = "";
            while (0 < length--)
                res += valid[UnityRandom.Range(0, valid.Length)];
            return res;
        }

        public static void RemoveNull<T>(ref List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i--);
                    continue;
                }
            }
        }
        public static void RemoveNull<T>(ref T[] array)
        {
            List<T> list = new List<T>(array);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i--);
                    continue;
                }
            }
            array = list.ToArray();
        }
    }
}
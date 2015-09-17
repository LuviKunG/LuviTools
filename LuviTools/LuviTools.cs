using UnityEngine;

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

        public static void DestroyChild(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static T RandomObject<T>(params T[] array)
        {
            int rand = Random.Range(0, array.Length);
            return array[rand];
        }

        public static T RandomObject<T>(System.Collections.Generic.List<T> list)
        {
            int rand = Random.Range(0, list.Count);
            return list[rand];
        }

        public static void Do(int time, System.Action action)
        {
            for (int i = 0; i < time; i++)
            {
                action();
            }
        }

        public static int Repeat(int t, int length)
        {
            return t % length;
        }

        public static System.Collections.IEnumerator DelayAction(float time, System.Action action)
        {
            float cd = 0;
            while (cd < time)
            {
                cd += Time.unscaledDeltaTime;
                yield return null;
            }
            action();
        }

        public enum StringRandomType
        {
            LowerCase,
            UpperCase,
            UpperAndLowerCase,
            LowerCaseWithNumber,
            UpperCaseWithNumber,
            UpperAndLowerCaseWithNumber
        }

        public static string RandomString(int length, StringRandomType type = StringRandomType.UpperAndLowerCaseWithNumber)
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
                res += valid[UnityEngine.Random.Range(0, valid.Length)];
            return res;
        }
    }
}
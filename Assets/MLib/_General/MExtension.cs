
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLib
{
    public static class MExtension
    {
        #region MonoBehaviour
        public static void Active(this MonoBehaviour m)
        {
            m.gameObject.SetActive(true);
        }
        public static void Deactive(this MonoBehaviour m)
        {
            m.gameObject.SetActive(false);
        }

        public static Coroutine DelayCall(this MonoBehaviour m, float delay, Action callback)
        {
            return m.StartCoroutine(IE_DelayCall());

            IEnumerator IE_DelayCall()
            {
                yield return new WaitForSeconds(delay);

                callback?.Invoke();
            }
        }

        public static Coroutine DelayRealtimeCall(this MonoBehaviour m, float delay, Action callback)
        {
            return m.StartCoroutine(IE_DelayCall());

            IEnumerator IE_DelayCall()
            {
                yield return new WaitForSecondsRealtime(delay);

                callback?.Invoke();
            }
        }
        public static List<T> ClonePrefabsInside<T>(this MonoBehaviour m, T prefab, int amount, bool hide = false) 
            where T : MonoBehaviour
        {
            List<T> result = new List<T>();
            Transform parent = m.transform;
            for (int i = 0; i < amount; i++)
            {
                T clone = GameObject.Instantiate(prefab, parent);
                clone.gameObject.SetActive(!hide);
                result.Add(clone);
            }
            return result;
        }
        #endregion

        #region List
        public static T GetRandom<T>(this List<T> list)
        {
            if(list.Count <= 0) return default;
            else
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

namespace Source.MobGenerator
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance;
        // ReSharper disable once UnassignedReadonlyField
        public Queue<GameObject>[] Mobs;
        public GameObject[] mobPrefabs;
        public int[] mobPoolCount;
        [HideInInspector] public bool[] toggled;
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            int mobTypeCount = Enum.GetValues(typeof(MobType)).Length;
            Mobs = new Queue<GameObject>[mobTypeCount];
            for (int i = 0; i < mobTypeCount; i++)
            {
                Mobs[i] = new Queue<GameObject>();
            }

        }

        public GameObject GetMob(MobType type, Vector3 pos = default, Quaternion rot = default)
        {
            //Debug.Log(Mobs[(int)MobType.Bullet].Count);
            if (Mobs[(int)type].Count < 1)
            {
                var newMob = Instantiate(mobPrefabs[(int)type], pos, rot,null);
                return newMob;
            }
            else
            {
                var newMob = Mobs[(int)type].Dequeue();
                if (!newMob)
                    newMob = Instantiate(mobPrefabs[(int)type], pos, rot, null);
                newMob.transform.SetParent(null);
                newMob.transform.position = pos;
                newMob.transform.rotation = rot;
                newMob.SetActive(true);
                return newMob;
            }
        }

        public void ReturnMob(MobType type, GameObject obj)
        {
            if (!obj)
                return;
            if (Mobs[(int)type].Count < mobPoolCount[(int) type])
            {
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                Mobs[(int)type].Enqueue(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
        
        public enum MobType
        {
            Bullet,
            Unit,
            Enemy,
            Boss,
            Gate,
            Item,
        }
    }
}

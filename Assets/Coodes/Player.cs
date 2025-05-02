using System;
using Source.MobGenerator;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public IngamePlayerMouse playerMouse;
    public float shotSpeed;
    public int unitDamage;
    [SerializeField] private Material playerMaterial;
    public float bulletLifeTime;

    public void AddUnit(int count = 1)
    {
        if (count < 0)
        {
            DeleteUnit(-count);
        }
        else
            playerMouse.AddUnits(count);
    }
    

    public void DeleteUnit(int cnt)
    {
        int firstIdx = -1;
        
        int idx;
        for (int i = 0; i < cnt&&playerMouse.units.Count>0; i++)
        {
            float zMax = -2147483647;
            idx = 0;
            foreach (var u in playerMouse.units)
            {
                if (u.transform.position.z > zMax)
                {
                    zMax = u.transform.position.z;
                    firstIdx = idx;
                }

                idx++;
            }

            if (firstIdx == -1)
            {
                playerMouse.RemoveUnit(0);
                continue;
            }
            playerMouse.RemoveUnit(firstIdx);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            AddUnit();
    }
}

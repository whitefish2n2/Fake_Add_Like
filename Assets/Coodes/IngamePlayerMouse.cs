using System;
using System.Collections.Generic;
using Source.MobGenerator;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class IngamePlayerMouse : MonoBehaviour
{
    
    [SerializeField] public GameObject playerGround;
    [SerializeField] public List<GameObject> units = new List<GameObject>();
    [SerializeField] private List<Rigidbody> unitRigidbodies = new List<Rigidbody>();
    [SerializeField] public float unitRbP;
    private float _mouseX;
    private Camera _cam;
    private void Awake()
    {
        _cam = Camera.main;
        foreach (GameObject go in units)
        {
            unitRigidbodies.Add(go.GetComponent<Rigidbody>());
        }
    }

    private void AddUnit(GameObject unit)
    {
        units.Add(unit);
        unitRigidbodies.Add(unit.GetComponent<Rigidbody>());
    }

    public void RemoveUnit(int i)
    {
        ObjectPool.instance.ReturnMob(ObjectPool.MobType.Unit, units[i]);
        units.RemoveAt(i);
        unitRigidbodies.RemoveAt(i);
    }

    public void AddUnits(int mobPlus)
    {
        if (units.Count + mobPlus < 100)
        {
            for(int i = 0; i < mobPlus; i++)
                AddUnit(ObjectPool.instance.GetMob(ObjectPool.MobType.Unit, transform.position + new Vector3(Random.Range(-1.5f,1.5f),0, Random.Range(-1.5f, 1.5f)), Quaternion.identity));
            return;
        }
        int upgradeTime = 0;
        
        int targetCount = units.Count + mobPlus;
        while (targetCount>100)
        {
            upgradeTime++;
            targetCount /= 2;
        }
        var removeMobCount = units.Count - targetCount;
        GameStatic.instance.player.unitDamage *= (int)math.pow(2, upgradeTime);
        if (removeMobCount > 0)
        {
            for(var i = 0; i < removeMobCount; i++)
                RemoveUnit(0);
        }
        else
        {
            for (var i = 0; i < -removeMobCount; i++)
            {
                var o =ObjectPool.instance.GetMob(ObjectPool.MobType.Unit, transform.position + new Vector3(Random.Range(-1.5f,1.5f),0, Random.Range(-1.5f, 1.5f)), Quaternion.identity);
                AddUnit(o);
            }
        }
    }
    
            

    private void Update()
    {
        _mouseX = Input.mousePosition.x;
        var srp = _cam.ScreenToWorldPoint(new Vector3(_mouseX, 0, 10));
        playerGround.transform.position = new Vector3(Mathf.Clamp(srp.x, -5f, 5f), 0, -2);
    }
    
}

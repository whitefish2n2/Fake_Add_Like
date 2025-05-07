using System;
using System.Collections;
using System.Collections.Generic;
using Source.MobGenerator;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public int count=0;
    [SerializeField] private Renderer[] mat;
    Coroutine coroutine;
    [SerializeField] private TextMeshPro valueText;
    

    public void Init(int c)
    {
        count = c;
        valueText.text = count.ToString();
        if (count < 0)
        {
            foreach (var m in mat)
            {
                m.material.color = new Color(1f,0f,0f,m.material.color.a);
            }
        }
        else
        {
            foreach (var m in mat)
            {
                m.material.color = new Color(0f,0f,1f,m.material.color.a);
            }
        }

        coroutine = StartCoroutine(Logic());
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger on gate " + other.name );
        if (other.transform.CompareTag("bullet"))
        {
            Hit(1);
            ObjectPool.instance.ReturnMob(ObjectPool.MobType.Bullet, other.gameObject);
        }
        else if (other.transform.CompareTag("Player"))
        {
            GameStatic.instance.player.AddUnit(count);
            //transform.parent.
            gameObject.SetActive(false);
        }
    }
    
    IEnumerator Logic()
    {
        while (gameObject.activeInHierarchy)
        {
            gameObject.transform.Translate(transform.forward * (GameStatic.instance.gateSpeed * Time.deltaTime));
            yield return null;
        }
    }

    public void Hit(int damage)
    {
        if (count < 0 && count + damage >= 0)
        {
            Debug.Log("change color");
            foreach (var m in mat)
            {
                foreach(var n in m.materials)
                    n.color = new Color(0f,0f,1f,m.material.color.a);
            }
        }
        count+=damage;
        valueText.text = count.ToString();
    }
}

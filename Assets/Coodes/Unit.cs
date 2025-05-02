using System;
using System.Collections;
using System.Collections.Generic;
using Source.MobGenerator;
using Unity.Mathematics;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private static readonly int Property = Shader.PropertyToID("BaseMap");
    private bool alive;
    private Coroutine _currentRoutine;
    private bool flag = false;
    private List<GameObject> collisingObjects =  new List<GameObject>(10);
    private Rigidbody rb;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
        flag = true;
        _currentRoutine = StartCoroutine(Logic());
    }

    IEnumerator Logic()
    {
         while (true)
         {
             yield return new WaitForSeconds(1/GameStatic.instance.player.shotSpeed);
             var a =ObjectPool.instance.GetMob(ObjectPool.MobType.Bullet, gameObject.transform.position, Quaternion.identity);
         }
    }

    public void OnEnable()
    {
        if (!flag) return;
        _currentRoutine = StartCoroutine(Logic());
        
    }

    public void OnDisable()
    {
        StopCoroutine(_currentRoutine);
    }

    private int cnt;
    private void FixedUpdate()
    {
        cnt = 0;
        var r = Physics.RaycastAll(transform.position,
            (GameStatic.instance.playerMouse.transform.position - gameObject.transform.position).normalized,
            0.7f, LayerMask.GetMask("Unit"));
        foreach (var t in r)
        {
            if (t.collider.gameObject.CompareTag("unit"))
                cnt++;
        }

        if (cnt > 1 && Vector3.Distance(transform.position, GameStatic.instance.playerMouse.transform.position) < 1.5f )
        {
            rb.linearVelocity = Vector3.zero;
            rb.mass = 10f;
            return;
        }
        else if (cnt > 2)
        {
            rb.linearVelocity = Vector3.zero;
            rb.mass = 10f;
            return;
        }
        else if (Vector3.Distance(transform.position, GameStatic.instance.playerMouse.transform.position) < 1f)
            rb.linearVelocity =
                (GameStatic.instance.playerMouse.transform.position - gameObject.transform.position).normalized *
                ((float)Math.Pow(
                     Vector3.Distance(transform.position, GameStatic.instance.playerMouse.transform.position), 2) *
                 GameStatic.instance.playerMouse.unitRbP);
        else
            rb.linearVelocity =
                ((GameStatic.instance.playerMouse.transform.position - gameObject.transform.position).normalized *
                 GameStatic.instance.playerMouse.unitRbP);
        rb.mass = 1f;
        mat.color = Color.white;
        /*rb.mass = 10/math.abs(math.distance(GameStatic.instance.playerMouse.transform.position,gameObject.transform.position)) + 1;*/
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("unit") && !collisingObjects.Contains(collision.gameObject))
        {
            collisingObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collisingObjects.Contains(collision.gameObject))
        {
            collisingObjects.Remove(collision.gameObject);
        }
    }
}

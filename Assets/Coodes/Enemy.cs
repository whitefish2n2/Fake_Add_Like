using System;
using System.Collections;
using System.Collections.Generic;
using Source.MobGenerator;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class Enemy:MonoBehaviour
{
    [HideInInspector] public static readonly int IsAliveAnimHash = Animator.StringToHash("isAlive");
    [HideInInspector] public static readonly int DieAnimHash = Animator.StringToHash("Die");
    private bool alive = true;
    [SerializeField] private float dieAnimTime = 0f;
    [SerializeField] private int hp;
    [SerializeField] private int atk;
    [SerializeField] private Animator animator;
    Coroutine currentCoroutine;
    [SerializeField] private ObjectPool.MobType mobType;
    private int currentHp;
    private bool isOnPool = false;

    public void Start()
    {
        isOnPool = true;
        Init();
    }

    public void OnEnable()
    {
        if (isOnPool)
        {
            Init();
        }
    }

    public virtual void Init()
    {
        if(currentCoroutine is not null)
            StopCoroutine(currentCoroutine);
        currentHp = hp;
        alive = true;
        animator.SetBool(IsAliveAnimHash,true);
        currentCoroutine = StartCoroutine(Logic());
    }

    public void Hit(int Damage)
    {
        currentHp -= Damage;
        OnHit();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(!alive) return;
        if (other.gameObject.CompareTag("endLine"))
        {
            Attack();
            alive = false;
            Die();
        }
        else if (other.gameObject.CompareTag("bullet"))
        {
            Hit(GameStatic.instance.player.unitDamage);
            ObjectPool.instance.ReturnMob(ObjectPool.MobType.Bullet, other.gameObject);
        }
    }
    IEnumerator Logic()
    {
        while (alive)
        {
            gameObject.transform.parent.Translate(transform.forward * (GameStatic.instance.enemySpeed * Time.deltaTime));
            yield return null;
        }
    }

    public virtual void OnHit()
    {
        if (currentHp < 1)
        {
            alive = false;
            Die();
        }
    }

    public virtual void Die()
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(DieIE());
    }

    public virtual IEnumerator DieIE()
    {
        animator.SetBool(IsAliveAnimHash,false);
        animator.SetTrigger(DieAnimHash);
        yield return new WaitForSeconds(dieAnimTime);
        ObjectPool.instance.ReturnMob(mobType, gameObject.transform.parent.gameObject);
    }
    public virtual void Attack()
    {
        GameStatic.instance.player.DeleteUnit(atk);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Source.MobGenerator;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float _currentLifeTime;
    private Coroutine currentCoroutine;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        trailRenderer.enabled = true;
        _currentLifeTime = 0f;
        currentCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (_currentLifeTime<GameStatic.instance.player.bulletLifeTime)
        {
            gameObject.transform.Translate(-transform.forward * (GameStatic.instance.bulletSpeed * Time.deltaTime));
            _currentLifeTime += Time.deltaTime;
            yield return null;
        }
        if(gameObject.activeInHierarchy)
            ObjectPool.instance.ReturnMob(ObjectPool.MobType.Bullet, gameObject);
    }
    private void OnDisable()
    {
        trailRenderer.Clear();
        trailRenderer.enabled = false;
        StopCoroutine(currentCoroutine);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Source.MobGenerator;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float _currentLifeTime;
    private Coroutine _currentCoroutine;
    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        _trailRenderer.enabled = true;
        _currentLifeTime = 0f;
        _currentCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (_currentLifeTime<GameStatic.instance.player.bulletLifeTime)
        {
            gameObject.transform.Translate(-transform.forward * (GameStatic.instance.bulletSpeed * Time.deltaTime));
            _currentLifeTime += Time.deltaTime;
            yield return null;
        }
        ObjectPool.instance.ReturnMob(ObjectPool.MobType.Bullet, gameObject);
    }
    private void OnDisable()
    {
        _trailRenderer.Clear();
        _trailRenderer.enabled = false;
        StopCoroutine(_currentCoroutine);
    }
}

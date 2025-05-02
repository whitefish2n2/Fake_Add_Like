using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private float waveTrim;
    [SerializeField] private float genTrim;
    
    //maxMonster의 1.3배 +a(보스,이벤트)까지 소환될 수 있음.
    [SerializeField] private int maxMonster;
    [SerializeField] private Vector2 randomSpawnRange;
    Random random = Random.CreateFromIndex((uint)(DateTime.Now.Second*DateTime.Now.Millisecond));
    public Coroutine CurrentRoutine;
    public void Awake()
    {
        
        
    }

    public void Start()
    {
        GameStatic.instance.phase = 0;
        GameStatic.instance.difficulty = 1;
        //variation Init
        StartWave();
    }
    public void StartWave()
    {
        CurrentRoutine = StartCoroutine(Wave());
        
    }

    IEnumerator Wave()
    {
        while (GameStatic.instance.gameState == GameState.Play)
        {
            GameStatic.instance.phase++;
            GameStatic.instance.difficulty *= 1.1f;
            for (int i = 0;
                 i < (math.max((int)GameStatic.instance.difficulty * 10, maxMonster)) * random.NextFloat(0.7f, 1.3f);
                 i++)
            {
                //범위 내 랜덤 위치로 몬스터 생성
                EnemyManager.Instance.GetRandomNormalMob(transform.position + new Vector3(random.NextFloat(-randomSpawnRange.x, randomSpawnRange.x),
                    0, random.NextFloat(-randomSpawnRange.y, randomSpawnRange.y)), transform.rotation);
                yield return new WaitForSeconds(genTrim);
            }

            if (GameStatic.instance.phase % 5 == 0)
            {
                //범위 내 샌덤 위치로 몬스터 생성
                EnemyManager.Instance.GetRandomBossMob(transform.position + new Vector3(random.NextFloat(-randomSpawnRange.x, randomSpawnRange.x),
                    transform.position.y, random.NextFloat(-randomSpawnRange.y, randomSpawnRange.y)), transform.rotation);
            }
            yield return new WaitForSeconds(waveTrim);
        }
    }
}

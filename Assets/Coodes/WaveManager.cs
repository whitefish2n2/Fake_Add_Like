using System;
using System.Collections;
using Source.MobGenerator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
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
            GameStatic.instance.difficulty += 0.1f;
            for (int i = 0;
                 i < (math.max((int)GameStatic.instance.difficulty * 10, maxMonster)) * random.NextFloat(0.7f, 1.3f);
                 i++)
            {
                //범위 내 랜덤 위치로 몬스터 생성
                EnemyManager.Instance.GetRandomNormalMob(transform.position + new Vector3(random.NextFloat(-randomSpawnRange.x, randomSpawnRange.x),
                    0, random.NextFloat(-randomSpawnRange.y, randomSpawnRange.y)), transform.rotation);
                yield return new WaitForSeconds(genTrim);
            }
            
            //5라운드에 1번 보스 생성
            if (GameStatic.instance.phase % 5 == 0)
            {
                //범위 내 랜덤 위치로 몬스터 생성
                EnemyManager.Instance.GetRandomBossMob(transform.position + new Vector3(random.NextFloat(-randomSpawnRange.x, randomSpawnRange.x),
                    transform.position.y, random.NextFloat(-randomSpawnRange.y, randomSpawnRange.y)), transform.rotation);
            }

            //3라운드에 한번 게이트 생성
            if (GameStatic.instance.phase % 3 == 0)
            {
                int v = random.NextInt(-10,10);
                v*=(int)GameStatic.instance.difficulty;
                var l= ObjectPool.instance.GetMob(ObjectPool.MobType.Gate,new Vector3(-1.5f,0,transform.position.z));
                l.GetComponent<Gate>().Init(v);
                var r = ObjectPool.instance.GetMob(ObjectPool.MobType.Gate,new Vector3(1.5f,0,transform.position.z));
                r.GetComponent<Gate>().Init(v);
            }
            
            //7라운드에 한번 아이템 생성
            if (GameStatic.instance.phase % 7 == 0)
            {
                //대충 이이템 생성하는 로직
            }
            yield return new WaitForSeconds(waveTrim);
        }
    }
}

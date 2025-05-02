using Source.MobGenerator;
using UnityEngine;

/// <summary>
/// 적들을 랜덤하게 소환하는 poolmanager와 연계된 싱글톤 클래스입니다.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    void Start()
    {
        Instance = this;
    }

    public GameObject GetRandomNormalMob(Vector3 position, Quaternion rotation)
    {
        return ObjectPool.instance.GetMob(ObjectPool.MobType.Enemy,position, rotation);//todo: 몹이 여러개 생기면 랜덤으로다가 호출하는 걸로 변경
    }

    public GameObject GetRandomBossMob(Vector3 position, Quaternion rotation)
    {
        return ObjectPool.instance.GetMob(ObjectPool.MobType.Boss,position, rotation);//todo:니도
        
    }
}


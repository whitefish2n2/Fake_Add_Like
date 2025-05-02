using System;
using UnityEngine;


public enum GameState{
    Play,
    Die,
    Pause
}
public class GameStatic : MonoBehaviour
{
    public static GameStatic instance;
    public float gateSpeed;
    public Player player;
    public int phase=0;
    public float difficulty;
    public GameState gameState;
    public float bulletSpeed;
    public IngamePlayerMouse playerMouse;
    public float enemySpeed;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Init");
        Debug.Log(GameStatic.instance.player.shotSpeed);
    }
}

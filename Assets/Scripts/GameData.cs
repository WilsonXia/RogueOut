using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Map,
    Battle
}

public class GameData : MonoBehaviour
{
    public static GameData instance;

    // References
    public GameState state;
    public MapData map;
    public PlayerData player;
    public PlayerController pController;

    public MapData Map { get { return map; } set { map = value; } }
    public PlayerData Player { get { return player; } set { player = value; } }
    public PlayerController PlayerController { get { return pController; } set { pController = value; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            pController = GetComponent<PlayerController>();
            player = GetComponent<PlayerData>();
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void ChangeState(GameState newState)
    {
        state = newState;
        switch (state)
        {
            case GameState.Map:
                map.gameObject.SetActive(true);
                break;
            case GameState.Battle:
                map.gameObject.SetActive(false);
                break;
        }
    }
}

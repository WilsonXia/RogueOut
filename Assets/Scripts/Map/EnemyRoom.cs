using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : Room
{
    // Enemy Prefabs
    public List<GameObject> enemyPrefabs;
    public int numberOfEnemies;

    private void Start()
    {
        type = RoomType.Enemy;
    }

    public void SetEnemies(List<GameObject> newList)
    {
        enemyPrefabs = newList;
    }
}

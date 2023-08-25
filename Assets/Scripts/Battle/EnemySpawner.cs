using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Fields
    public List<GameObject> enemyPrefabs;
    public float margin;

    public List<GameObject> SpawnEnemies()
    {
        List<GameObject> enemies = new();
        Vector2 pos = new (0,0);
        float centerFix = (enemyPrefabs.Count - 1f) / 2f;
        pos -= new Vector2(margin * centerFix, 0);
        foreach (GameObject enemy in enemyPrefabs)
        {
            enemies.Add(SpawnEnemy(enemy, pos));
            pos += new Vector2(margin, 0);
        }
        return enemies;
    }
    GameObject SpawnEnemy(GameObject enemyPrefab, Vector2 pos)
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = pos;
        return newEnemy;
    }
}

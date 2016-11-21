using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public int enemyCount;

    public GameObject spiderEasy;
    public GameObject spiderMedium;
    public GameObject spiderHard;
    public GameObject player;

    Vector3 startPoint;



	// Use this for initialization
	void Start () {

        startPoint = new Vector3(250, 10, 250);
        
        
    }
	
	// Update is called once per frame
	void Update () {

	
	}

    public void createEnemies(enemyData enemies)
    {
        for(int i = 0; i < enemies.easy; i++)
        {
            enemyCount += 1;
            float x = startPoint.x + Random.Range(-enemies.spawnRange, enemies.spawnRange);
            float z = startPoint.z + Random.Range(-enemies.spawnRange, enemies.spawnRange);
            float y = 25;
            Vector3 position = new Vector3(x, y, z);
            GameObject.Instantiate(spiderEasy, position, new Quaternion());
        }
        
        for (int i = 0; i < enemies.medium; i++) {
            enemyCount += 1;
            float x = startPoint.x + Random.Range(-enemies.spawnRange, enemies.spawnRange);
            float z = startPoint.z + Random.Range(-enemies.spawnRange, enemies.spawnRange);
            float y = 25;
            Vector3 position = new Vector3(x, y, z);
            GameObject.Instantiate(spiderMedium, position, new Quaternion());

        }

        for(int i = 0; i < enemies.hard; i++)
        {
            enemyCount += 1;
            float x = startPoint.x + Random.Range(-enemies.spawnRange, enemies.spawnRange);
            float z = startPoint.z + Random.Range(-enemies.spawnRange, enemies.spawnRange);
            float y = 25;
            Vector3 position = new Vector3(x, y, z);
            GameObject.Instantiate(spiderHard, position, new Quaternion());
        }
    }







}

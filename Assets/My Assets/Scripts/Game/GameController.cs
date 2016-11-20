using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    private bool isSpawning = false;
    public float maxTime = 6.0f;
    public float minTime = 2.0f;

    private int difficultyRating = 50;
    private int enemyIndex;

    private void Start()
    {
        enemies = new GameObject[2];
        enemies[0] = enemy1;
        enemies[1] = enemy2;
        enemies[2] = enemy3;
    }

    private IEnumerator SpawnObject(int index, float seconds)
    {
        // Debug.Log ("Waiting for " + seconds + " seconds");

        yield return new WaitForSeconds(seconds);
        Instantiate(enemies[index], transform.position, transform.rotation);

        //We've spawned, so now we could start another spawn     
        isSpawning = false;
    }

    private void Update()
    {
        //We only want to spawn one at a time, so make sure we're not already making that call
        if (!isSpawning)
        {
            isSpawning = true; //Yep, we're going to spawn
            int spawnRating = Random.Range(0, difficultyRating);
            Debug.Log(spawnRating);
            if (spawnRating <= 20)
                enemyIndex = 0;
            else if(spawnRating >=21 && spawnRating <= 44)
                enemyIndex = 1;
            else
                enemyIndex = 2;

            StartCoroutine(SpawnObject(enemyIndex, Random.Range(minTime, maxTime)));
        }
    }
}
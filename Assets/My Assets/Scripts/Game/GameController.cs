using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Upgrade Attributes")][Space(5)]
    public float IDpowerUpTime = 30f;
    public float IDpowerUpTimer = 30f;
    private int IDgoldCost = 50;
    private bool IDisActive = false;

    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private float currentTankLvl = 1f;
    private float maxTankLvl = 5f;


    public GameObject[] enemies;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    private bool isSpawning = false;
   [SerializeField] private bool stopSpawning = false; //debug bool to stop all enemies from spawning
    private float maxTime = 8f;
    private float minTime = 4f;

    private int enemyIndex;
    private int diffRating = 100;

    public int playerGold = 0;
    public Text goldText;

    public bool bossPresent = false; 

    private void Start()
    {
        GameObject.Find("IDTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
        GameObject.Find("RSTTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
        GameObject.Find("TankLevelBar").GetComponent<Image>().fillAmount = (currentTankLvl / maxTankLvl);
        GameObject.Find("TankLevel").GetComponent<Text>().text = "Tank Level: " +  currentTankLvl.ToString();
        enemies = new GameObject[3]; //the actual number of elements you want the array to conatin. The array will start a t index 0, and proceed to the defined limit
        enemies[0] = enemy1;
        enemies[1] = enemy2;
        enemies[2] = enemy3;
    }

    private IEnumerator SpawnObject(int index, float seconds)
    {
         //Debug.Log ("Waiting for " + seconds + " seconds");

        yield return new WaitForSeconds(seconds);
        Instantiate(enemies[index], transform.position, transform.rotation);

        //We've spawned, so now we could start another spawn     
        isSpawning = false;
    }

    void EnemySpawner()
    {
        if (!isSpawning && !stopSpawning)
        {
            if (bossPresent)
            {
                isSpawning = true;
                //Debug.Log("Boss Battle!");
            }
            else
            {
                //Debug.Log("ResumingSpawn!");
                isSpawning = true; //Yep, we're going to spawn
                int spawnRating = Random.Range(0, diffRating);


                if (spawnRating <= 30)
                    enemyIndex = 0; //trooper
                else if (spawnRating >= 21 && spawnRating <= 44)
                    enemyIndex = 1; //spaceship
                else if (spawnRating >= 70)
                {
                    enemyIndex = 2; //boss
                    bossPresent = true;

                }

                StartCoroutine(SpawnObject(enemyIndex, Random.Range(minTime, maxTime)));
            }

        }
    }

    void IncreaseDamageUpgrade()
    {
        if (IDisActive) //is the powerup active?
        {
            if (IDpowerUpTimer >= 0)
            {
                IDpowerUpTimer -= Time.deltaTime;
                GameObject.Find("IDTimeLeft").GetComponent<Text>().text = "Time Left: " + IDpowerUpTimer.ToString("F1");
                //continue powerup
            }
            else
            {
                //end powerup
                Debug.Log("Ending powerup");
                GameObject.Find("Character").GetComponent<PlayerController>().ID = true;
                GameObject.Find("IDStatus").GetComponent<Image>().sprite = inactiveSprite;
                GameObject.Find("IDTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
                IDisActive = false;
            }

        }
    }

    public void IDCheck()
    {
        if (playerGold >= IDgoldCost)
        {
            if (!IDisActive)
            {
                Debug.Log("You have enough gold, beginning powerup");

                IDpowerUpTimer = IDpowerUpTime;
                playerGold -= IDgoldCost;
                IDisActive = true;
                GameObject.Find("Character").GetComponent<PlayerController>().ID = true;
                GameObject.Find("IDStatus").GetComponent<Image>().sprite = activeSprite;
            }
            else
            {
                Debug.Log("Continuing powerup");

                playerGold -= IDgoldCost;
                IDpowerUpTimer += IDpowerUpTime;
            }


        }
        else
        {
            Debug.Log("You don't have enough gold");
        }
    }

    public void IncreaseTankLevel()
    {
        
    }

    private void Update()
    {
        goldText.text = "Gold: " + playerGold.ToString();
        //We only want to spawn one at a time, so make sure we're not already making that call
        EnemySpawner();
        IncreaseDamageUpgrade();
    }

}
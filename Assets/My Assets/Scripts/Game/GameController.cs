using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Increase Damage")][Space(5)]
    public float IDpowerUpTime = 30f;
    public float IDpowerUpTimer = 30f;
    private int IDgoldCost = 50;
    private bool IDisActive = false;


    [Header("Reduce Cooldown")] [Space(5)]
    public float RCDTimer = 30f;
    public float RCDTime = 30f;
    private int RCDGoldCost = 30;
    private bool RCDisActive = false;

    [Header("Tank Upgrade")][Space(5)]
    private float currentTankLvl = 1f;
    private float maxTankLvl = 5f;
    private int TLGoldCost = 100;

    [Header("HUD Components")][Space(5)]
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    [Header("Enemy Components")][Space(5)]
    public GameObject[] enemies;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    [SerializeField] private GameObject Explosion;
    public AudioClip explosionClip;
    private AudioSource audio;

    [Header("Debug Tools")][Space(5)]
    [SerializeField] private bool stopSpawning = false; //debug bool to stop all enemies from spawning
    public bool bossPresent = false;

    private float maxTime = 8f;
    private float minTime = 4f;
    private bool isSpawning = false;
    private int enemyIndex;
    private int diffRating = 100;

    [Header("Player Properties")][Space(5)]
    public int playerGold = 0;
    public Text goldText;

    float destroyTime = 1f;
    float destroyTimer = 1f;


    public bool isGameOver = false;
    private void Start()
    {
        GameObject.Find("IDTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
        GameObject.Find("RSTTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
        GameObject.Find("TankLevelBar").GetComponent<Image>().fillAmount = (currentTankLvl / maxTankLvl);
        GameObject.Find("TankLevel").GetComponent<Text>().text = "Tank Level: " +  currentTankLvl;
        enemies = new GameObject[3]; //the actual number of elements you want the array to conatin. The array will start a t index 0, and proceed to the defined limit
        enemies[0] = enemy1;
        enemies[1] = enemy2;
        enemies[2] = enemy3;
        audio = GetComponent<AudioSource>();
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
                GameObject.Find("Character").GetComponent<PlayerController>().ID = false;
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
        if (playerGold >= TLGoldCost)
        {
            if (currentTankLvl<maxTankLvl)
            {
                Debug.Log("You have enough gold, upgrading tank");
                IDpowerUpTimer = IDpowerUpTime;
                playerGold -= TLGoldCost;
                GameObject.Find("Character").GetComponent<PlayerController>().maxPressure += 100;
                
                    GameObject.Find("Character").GetComponent<PlayerController>().currentPressure =
                        GameObject.Find("Character").GetComponent<PlayerController>().maxPressure;


                currentTankLvl++;
                GameObject.Find("TankLevelBar").GetComponent<Image>().fillAmount = currentTankLvl / maxTankLvl;
                GameObject.Find("TankLevel").GetComponent<Text>().text = "Tank Level: " + currentTankLvl;
            }
            else
            {
                Debug.Log("You've reached the maximum tank level");
            }
        }
        else
        {
            Debug.Log("You don't have enough gold");
        }
    }

    public void RCDCheck()
    {
        if (playerGold >= RCDGoldCost)
        {
            if (!RCDisActive)
            {
                    Debug.Log("Starting CoolDownUpgrade");
                    RCDTimer = RCDTime;
                    playerGold -= RCDGoldCost;
                    RCDisActive = true;
                    GameObject.Find("Character").GetComponent<PlayerController>().RCD = true;
                    GameObject.Find("RSTStatus").GetComponent<Image>().sprite = activeSprite;
                }
                else
                {
                    Debug.Log("Continuing poweup");
                    playerGold -= RCDGoldCost;
                    RCDTimer += RCDTime;
                }
            }
    }

    void ReduceCoolDown()
    {
        if (RCDisActive)
        {
            if (RCDTimer >= 0)
            {
                RCDTimer -= Time.deltaTime;
                GameObject.Find("RSTTimeLeft").GetComponent<Text>().text = "Time Left: " + RCDTimer.ToString("F1");
                //continue powerup
            }
            else
            {
                Debug.Log("Ending powerup");
                GameObject.Find("Character").GetComponent<PlayerController>().RCD = false;
                GameObject.Find("RSTStatus").GetComponent<Image>().sprite = inactiveSprite;
                GameObject.Find("RSTTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
                RCDisActive = false;
            }
        }
    }

    public void DestroyGameObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        
        foreach (GameObject target in gameObjects)
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= Random.Range(0f,1.25f))
            {
                GameObject explObj =Instantiate(Explosion, target.GetComponent<Transform>().position,target.GetComponent<Transform>().rotation) as GameObject;
                audio.Play(); //plays the explsoion sound
                destroyTimer = destroyTime;
                GameObject.Destroy(target);
                Destroy(explObj, explosionClip.length);
            }


        }
    }

    private void Update()
    {
        if (isGameOver)
        {
            DestroyGameObjectsWithTag("Enemy");
            GameObject.Find("IDTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
            GameObject.Find("RSTTimeLeft").GetComponent<Text>().text = "Time Left: N/A";
            GameObject.Find("ScoreVal").GetComponent<Text>().text = (playerGold * 10).ToString();
        }
        else
        {
            goldText.text = "Gold: " + playerGold;
            //We only want to spawn one at a time, so make sure we're not already making that call
            EnemySpawner();
            IncreaseDamageUpgrade();
            ReduceCoolDown();
        }
    }

}
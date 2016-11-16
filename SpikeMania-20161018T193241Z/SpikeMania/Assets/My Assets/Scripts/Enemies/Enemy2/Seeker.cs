using System.Collections;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    public float angle = 0f;
    public GameObject bomb;

    [SerializeField] private float bombInterval;

    private int bombsSpawned = 0; //how many bombs has the seeker spawned?
    private int bombsToSpawn; //how many bombs to spawn, random number between one, and two
    public float circRad = 0.005f;
    private float delayTime = 2f; //how long the seeker waits around after dropping bombs before leaving

    [SerializeField] private float distanceToTarget = 1.5f;
        //How far away the seeker has to be before activating attack functions

    private Vector3 myPos;
    private bool isInRange = false;

    private void Start()
    {
        bombsToSpawn = Mathf.FloorToInt(Random.Range(1f, 2f));
        bombInterval = Mathf.Round(Random.Range(3f, 4f));
        //Debug.Log("Waiting for " + bombInterval + " seconds");
         //Debug.Log("Spawning " + bombsToSpawn + " bombs.");
        //StartCoroutine(DropBombs()); 
    }

    private IEnumerator DropBombs()
    {
        if (bombsSpawned <= bombsToSpawn)
        {
            yield return new WaitForSeconds(bombInterval);
            var bombObj = Instantiate(bomb, transform.position, transform.rotation) as GameObject;
            bombInterval = Random.Range(2f, 4f);
            bombsSpawned += 1;
        }
    }
    
    private void DistanceCheck()
    {
        if (!isInRange)
        {
            if (Vector2.Distance(transform.position, GameObject.Find("Character").transform.position) <= distanceToTarget)
            {
                isInRange = true;
            Debug.Log("In range");
                StartCoroutine(DropBombs());
            }
            
        }
            
    }

    private void Update()
    {
        DistanceCheck();
        if (bombsSpawned == bombsToSpawn)
            if (delayTime <= 0)
            {
                transform.Translate(myPos += new Vector3(0f, 0.05f, 0f));
                if (transform.position.y > 5f)
                    Destroy(gameObject);
            }
            else
            {
                delayTime -= Time.deltaTime;
                //Debug.Log(delayTime);
            }

        var target = GameObject.Find("Character").transform.position + new Vector3(0.0f, 1.5f, 0.0f);

        transform.position = Vector3.MoveTowards(transform.position, target, 0.8f * Time.deltaTime);
        myPos.x = Mathf.Cos(angle) * circRad;
        myPos.y = Mathf.Sin(angle) * circRad;
        transform.Translate(myPos);
        angle += 0.05f;
    }
}
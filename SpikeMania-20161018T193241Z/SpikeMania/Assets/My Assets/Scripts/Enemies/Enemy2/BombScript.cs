using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class BombScript : MonoBehaviour {

    // Use this for initialization

    float minTime = 1f;
    float maxtime = 3f;
    float waitTime;
    float currentTime = 10;
    public Sprite bomb;
    public Sprite bombwarn;
	void Start () {
        waitTime = Random.Range(minTime, maxtime);
        StartCoroutine(Explosion());
	
	}
	
    public List<GameObject> potentialTargets = new List<GameObject>();
    void OnTriggerEnter(Collider other)
    {
        potentialTargets.Add(other.gameObject);
        Debug.Log(potentialTargets);
    }
    void OnTriggerExit(Collider other) 
    {
        potentialTargets.Remove(other.gameObject);
    }
    public void InflictDamage()
    {
       
    }
    



    IEnumerator Explosion()
    {        
        yield return new WaitForSeconds(3f);
        foreach(GameObject t in potentialTargets)
        {
            Debug.Log(t);
            t.SendMessage("ApplyDamage", 10f,SendMessageOptions.DontRequireReceiver);
            //GameObject.FindGameObjectWithTag("Player").SendMessage("ApplyDamage", 10,SendMessageOptions.DontRequireReceiver);
        }


        Destroy(gameObject);

       
    }

	// Update is called once per frame
	void Update () 
    {
        //Debug.Log(potentialTargets);
       // Debug.Log("MOD result:" + Mathf.Round((currentTime) % 1f));
        if (Mathf.Round((currentTime) % 1) ==  0)
            gameObject.GetComponent<SpriteRenderer>().sprite = bomb;
        else if(Mathf.Round((currentTime) % 1) == 1)
            gameObject.GetComponent<SpriteRenderer>().sprite = bombwarn;

        if (currentTime >=0)
            currentTime -= Time.deltaTime;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class BombScript : MonoBehaviour {

    // Use this for initialization

    float minTime = 3f;
    float maxtime = 5f;
    float waitTime;
    float currentTime = 10;
    public Sprite bomb;
    public Sprite bombwarn;
	void Start () {
        waitTime = Random.Range(minTime, maxtime);
        StartCoroutine(Explosion());
	
	}
	
    public List<Collider> potentialTargets = new List<Collider>();
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.tag == "Player") //in the future extend to be used for everything in range, whether it is enemy or player related. Gor realism, and for interesting player tactics
        {
            //potentialTargets.Add(other.gameObject.collider);
            

        }
        
    }
    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            //potentialTargets.Remove(other.gameObject);
            Debug.Log(potentialTargets);
        }
        
    } 

        IEnumerator Explosion()
    {        
        yield return new WaitForSeconds(waitTime);

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, 1.5f); //checks within a specific radius (in a 3d space, so it spans multiple layers) for objects with Colliders, then proceeds to store them in an array.
        int i = 0;
        while (i < objectsInRange.Length)
        {
            potentialTargets.Add(objectsInRange[i]); //add each element of the array into the list(could get really laggy later, with significant numbers of colliders)
            //Debug.Log(potentialTargets);
            //Debug.Log(objectsInRange);
            i++;
        }
        foreach (Collider t in potentialTargets) //increments through each element in list. Stored in list for future updates, lasting effects and such.
        {
            Debug.Log(t);
            t.SendMessage("ApplyDamage", 10f,SendMessageOptions.DontRequireReceiver); //for all objects with scripts attached to them, call the function 'AppluDamage' *Note: only gets cast to objects that contain this function
        }


        Destroy(gameObject);       
    }

    

	// Update is called once per frame
	void Update () 
    {
        if (Mathf.Round((currentTime) % 1) ==  0) //odd or even result, change bomb sprite depending on result. To let player know something is happening with the bomb, and to get out of the way.
            gameObject.GetComponent<SpriteRenderer>().sprite = bomb;
        else if(Mathf.Round((currentTime) % 1) == 1)
            gameObject.GetComponent<SpriteRenderer>().sprite = bombwarn;

        if (currentTime >=0)
            currentTime -= Time.deltaTime;
	}
}

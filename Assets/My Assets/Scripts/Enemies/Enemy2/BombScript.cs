using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombScript : MonoBehaviour
{
    public Sprite bomb;
    public Sprite bombwarn;
    private float currentTime = 10;
    private readonly float maxtime = 5f;
    private readonly float minTime = 3f;
    [SerializeField]private GameObject explosion;
    public AudioClip explosionClip;
    private AudioSource audio;

    public List<Collider> potentialTargets = new List<Collider>();
    private float waitTime;

    private void Start()
    {
        waitTime = Random.Range(minTime, maxtime);
        audio = GetComponent<AudioSource>();
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(waitTime);

        var objectsInRange = Physics.OverlapSphere(transform.position, 1.5f); //checks within a specific radius (in a 3d space, so it spans multiple layers) for objects with Colliders, then proceeds to store them in an array.
        var i = 0;
        while (i < objectsInRange.Length)
        {
            potentialTargets.Add(objectsInRange[i]); //add each element of the array into the list(could get really laggy later, with significant numbers of colliders)
            //Debug.Log(potentialTargets);
            //Debug.Log(objectsInRange);
            i++;
        }
        foreach (var t in potentialTargets)
            //increments through each element in list. Stored in list for future updates, lasting effects and such.
        {
            //Debug.Log(t);
            t.SendMessage("ApplyDamage", 10f, SendMessageOptions.DontRequireReceiver); //for all objects with scripts attached to them, call the function 'AppluDamage' *Note: only gets cast to objects that contain this function
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        audio.Play(); //plays the explsoion sound
        Instantiate(explosion, transform.position, transform.rotation); //instantiates an explosion effect at the bomb's location
        Destroy(gameObject,explosionClip.length); //destroys the bombobject after the audioclip has played
    }


    // Update is called once per frame
    private void Update()
    {
        if (Math.Abs(Mathf.Round(currentTime % 1)) < 0.1)
            //odd or even result, change bomb sprite depending on result. To let player know something is happening with the bomb, and to get out of the way.
            gameObject.GetComponent<SpriteRenderer>().sprite = bomb;
        else if (Math.Abs(Mathf.Round(currentTime % 1) - 1) < 0.1)
            gameObject.GetComponent<SpriteRenderer>().sprite = bombwarn;

        if (currentTime >= 0)
            currentTime -= Time.deltaTime;
    }
}
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {


    private int takeDmg;
    private int myHealth = 100;

    //[SerializeField]
    public Transform Character;
    [SerializeField]
    private float distanceToTarget = 2;
    bool moving = true;
	// Use this for initialization


	void Start () {
        //GameObject.Find("Character").transform.position = Character.position;
	}
	
	// Update is called once per frame
	void Update () {
        //DistanceCheck();
        Movement();
	}
    void DistanceCheck()
    {
        if (Vector2.Distance(transform.position,Character.position) <= distanceToTarget)
        {
            moving = false;
        }
    }
    void Movement()
    {
        while(moving)
        {
            transform.Translate(Vector2.left * 3f * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            takeDmg = collision.gameObject.GetComponent<BulletScript>().myDmg;
            myHealth -= takeDmg;
            Debug.Log("My health is at:" + myHealth);
            if (myHealth <= 0)
                Destroy(gameObject);
        }
    }
}

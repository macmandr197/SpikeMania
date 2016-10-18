using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {


    private int takeDmg;
    private int myHealth = 100;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

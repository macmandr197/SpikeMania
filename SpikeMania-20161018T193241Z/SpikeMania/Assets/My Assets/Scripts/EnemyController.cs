using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    Animator anim;

    private int takeDmg;
    private int myHealth = 100;

    [SerializeField]
    public float movementSpeed;
    [SerializeField]
    private float distanceToTarget = 2;

    bool moving = true;
	// Use this for initialization


	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        DistanceCheck();
        Movement();
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }
	}
    void DistanceCheck()
    {
        if (Vector2.Distance(transform.position,GameObject.Find("Character").transform.position) <= distanceToTarget)
        {
            moving = false;
            anim.Play("Enemy1Idle");
        }
    }
    void Movement()
    {
        if (moving)
        {
            transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
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

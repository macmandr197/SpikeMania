using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    Animator anim;

    public Transform gunBarrel;

    private int takeDmg;
    private int enemyDmg = 1;
    private float shotTime = 2.5f;
    private float timer = 2.5f;


    [SerializeField]
    private int myHealth = 10;
    [SerializeField]
    public float movementSpeed;
    [SerializeField]
    private float distanceToTarget = 2;

    bool moving = true;
    bool isFiring = false;
	

    public GameObject EnemyBullet;


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
        Attack();   
	}


    private void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject bulletObj = Instantiate(EnemyBullet, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
            bulletObj.GetComponent<Rigidbody>().AddForce(Vector2.left * 2f, ForceMode.Impulse); 
            bulletObj.GetComponent<BulletScript>().myDmg = enemyDmg;
            timer = shotTime;
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
            Debug.Log("Enemy health is at:" + myHealth);
            if (myHealth <= 0)
                Destroy(gameObject);
        }
    }
}

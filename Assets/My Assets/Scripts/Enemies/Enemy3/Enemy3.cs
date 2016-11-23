using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Enemy3 : MonoBehaviour {

    [SerializeField] private float distanceToTarget = 2f;
    private Animator anim;
    private Rigidbody rb;
    public bool isGrounded;
    [SerializeField]
    private float jumpheight;

    [SerializeField]private float range = 1.5f;
    private Vector3 offset;
    private Vector3 leader;

    public Transform gunBarrel;
    private float timer = 1f; //current time
    private float shotTime = 1f; //the base time for each shot to reset to
    public GameObject EnemyBullet; //the bullet to instantiate
    private int enemyDmg = 2;
    private GameObject Player;
    private float myHealth = 15f; //overridden by inspector
    private float maxhealth = 15f;

    public Text pressureText;
    public Image healthBar;

    private float healthTime = 2f;
    private float healthTimeCount = 2f;




    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        offset = (transform.position - leader);
        Player = GameObject.Find("Character");
        healthBar.enabled = false;

    }

    public static float CalculateVertJump(float jumptarget)
    {
        return Mathf.Sqrt(8f * jumptarget * 9.83f);
    }


    private void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            var bulletObj = Instantiate(EnemyBullet, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject; //instantiating the bullet to shoot at player
            bulletObj.GetComponent<Rigidbody>().AddForce(Vector2.left * 3f, ForceMode.Impulse); //adding force to the bullet so it moves
            bulletObj.GetComponent<BulletScript>().myDmg = enemyDmg; //setting the damage of the bullet inherited from enemyDmg.
            timer = shotTime;
        }
    }

    void UpdateUI()
    {
        healthTimeCount -= Time.deltaTime;
        healthBar.fillAmount = (myHealth / maxhealth);
        if (healthTimeCount >= 0)
        {
            if (myHealth / maxhealth > 0.5)
            {
                Color mycol = Color.Lerp(Color.red, Color.green, (myHealth / maxhealth));
                healthBar.GetComponent<Image>().color = mycol;
            }
            else
            {
                Color mycol = Color.Lerp(Color.red, Color.yellow, (myHealth / maxhealth));
                healthBar.GetComponent<Image>().color = mycol;
            }
        }
        else
        {
            healthBar.enabled = false;
        }


    }

    void ApplyDamage(float damage) //used for bomb damage only
    {
        myHealth -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            healthBar.enabled = true;
            healthTimeCount = healthTime;
            myHealth -= collision.gameObject.GetComponent<BulletScript>().myDmg; //taking damage when hit by player bullet
            //Debug.Log("Enemy health is at:" + myHealth);

        }
    }

    void Movement()
    {
        if (isGrounded)
        //checks to see if the player is touching the ground, if true then the player can use jump again
        {
            if (GameObject.FindGameObjectWithTag("Bullet") != null)
            {
                if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Bullet").transform.position) <= 1.5f)
                {
                    var velocity = rb.GetComponent<Rigidbody>().velocity;
                    velocity.y = CalculateVertJump(jumpheight);
                    //returns values that range in a parabola to give realistic jump action
                    rb.velocity = velocity;
                }
            }
        }
    }

    void DistanceCheck()
    {
        Vector3 targetposition = Player.transform.position + offset / 2; //controls how far back to stay from the player. The '/2' is halving the distance to player

        targetposition.y = transform.position.y;
        if (transform.position.x > -3.2f && transform.position.x < 3.2f) //if the enemy is within the player's bounds, move towards player
        {
            transform.position += (targetposition - transform.position) * 0.05f;
        }
        else if (transform.position.x >= 3.2f && Vector2.Distance(transform.position,leader) >= targetposition.x) //controlling behaviour if player corners enemy, its not hopping around everywhere (right side of screen)
        {
            transform.position -= new Vector3(0.1f,0f,0f);
        }
        else if (transform.position.x <= -3.2f && Vector2.Distance(transform.position, leader) >= targetposition.x)//controlling behaviour if player corners enemy, its not hopping around everywhere (right side of screen)
        {
            transform.position += new Vector3(0.1f, 0f, 0f);
        }

    }
	// Update is called once per frame
	void Update ()
	{
        if (myHealth <= 0)
            Destroy(gameObject);
	    if (!Player)
	    {
	        Player = GameObject.Find("Character");
	    }
	    UpdateUI();
        Movement();
        DistanceCheck();
        {
            var move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }
	    Attack();

	}
}

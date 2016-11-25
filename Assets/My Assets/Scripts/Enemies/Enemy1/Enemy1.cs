using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float distanceToTarget = 2; //how far away the enemy has to be away from the player before it'll start shooting
    [SerializeField] public float movementSpeed; //how fast the enemy will move

    private bool moving = true;

    private float healthTime = 2f; //the time (in seconds) that the healthbar is displayed for on the enemy
    private float healthTimeCount = 2f;  


    private float shotTime = 1.5f; //the value to reset to, after enemy has fired
    private float timer = 1.5f; //the current time before the enemy can shoot again

    public Transform gunBarrel;
    private float takeDmg; //how much damage the enemy will take from the player's bullet
    public GameObject EnemyBullet; //the bullet prefab the enemy will fire
    private int enemyDmg = 1; //the ammount of damage the enmy does

    [SerializeField] private float myHealth = 5f; //overridden in inspector
    private float maxhealth = 5f;//maximum health enemy has. future implementations may include harder enemies

    public Image healthBar;
    

    private int goldVal = 5;



    private void Start()
    {
        anim = GetComponent<Animator>();
        healthBar.enabled = false;
    }


    private void ApplyDamage(float damage)
    {
        //Debug.Log("Hit for: " + damage + " health.");
        myHealth -= damage;
        
        //UpdateHealth();
    }

    void UpdateUI()
    {
        healthTimeCount -= Time.deltaTime;
        healthBar.fillAmount = (myHealth / maxhealth);
        if ( healthTimeCount >= 0)
        {
            if (myHealth / maxhealth > 0.5)
            {
                Color mycol = Color.Lerp(Color.red, Color.green, (myHealth / maxhealth)); //if the enemy's health is above 50%, the healthbar will transition from green to red
                healthBar.GetComponent<Image>().color = mycol;
            }
            else
            {
                Color mycol = Color.Lerp(Color.red, Color.yellow, (myHealth / maxhealth)); //else, it'll transition from yellow to red
                healthBar.GetComponent<Image>().color = mycol;
            }
        }
        else
        {
            healthBar.enabled = false;
        }
        

    }

    private void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            var bulletObj = Instantiate(EnemyBullet, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject; //instantiating the bullet to shoot at player
            bulletObj.GetComponent<Rigidbody>().AddForce(Vector2.left * 2f, ForceMode.Impulse); //adding force to the bullet so it moves
            bulletObj.GetComponent<BulletScript>().myDmg = enemyDmg; //setting the damage of the bullet inherited from enemyDmg.
            timer = shotTime;
        }
    }


    private void DistanceCheck()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Character").transform.position) <= distanceToTarget && transform.position.x < 3.2) //if Player is within range of the player
        {
            moving = false;
            anim.Play("Enemy1Idle");
            Attack();
        }
    }

    private void Movement()
    {
        if (moving)
        {
            transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            healthBar.enabled = true;
            healthTimeCount = healthTime;
            takeDmg = collision.gameObject.GetComponent<BulletScript>().myDmg; //getting the amount of damage to take from the player's bullet
            myHealth -= takeDmg;
            Debug.Log("Hit for: " + takeDmg);

        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (myHealth <= 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().playerGold += goldVal;
            Destroy(gameObject);
        }
            
        UpdateUI();
        DistanceCheck();
        Movement();
        {
            var move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }
        
    }
}

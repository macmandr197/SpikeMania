using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float distanceToTarget = 2;
    public GameObject EnemyBullet;
    private int enemyDmg = 1;

    public Transform gunBarrel;

    [SerializeField] public float movementSpeed;

    private bool moving = true;


    [SerializeField] private float myHealth = 5;

    private float shotTime = 1.5f;

    private float takeDmg;
    private float timer = 1.5f;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void ApplyDamage(float damage)
    {
        //Debug.Log("Hit for: " + damage + " health.");
        myHealth -= damage;
        //UpdateHealth();
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
            takeDmg = collision.gameObject.GetComponent<BulletScript>().myDmg;
            myHealth -= takeDmg;
            //Debug.Log("Enemy health is at:" + myHealth);
            
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (myHealth <= 0)
            Destroy(gameObject);

        DistanceCheck();
        Movement();
        {
            var move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }
        
    }
}

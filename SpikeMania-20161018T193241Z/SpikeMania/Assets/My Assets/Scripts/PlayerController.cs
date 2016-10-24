using UnityEngine;
using System.Collections;
using Image=UnityEngine.UI.Image;


public class WeaponClass
{
    //fireRate(or mod)
    //bulletType
    //damage
    //pressure cost

    //create objct of weapontype with all of bullet's attribute stored within (damage calculation stored within weapontype object)
    //construct 3 weapons for each weapon type in array (when firing, spawns bullet with attributes stored in array at weaponarray[index]
    // abstract out all hardcoded names and variables and replace with call to weaponarray and use those attributes

	/// I'm of the opinion everything here should be private or protected ///
    public float fireRate;
    public float fireRateMod; // might not need this now
    public float pressureCost;
    public int bulletDmg;
    public float currentPressure = 100f;
    private float maxPressure = 100f;
    private float lastShot = 0f; // might want to use logic for this instead of hardcoding it
    public GameObject bulletType;
    public Transform playerBarrel;
    public Vector3 playerShotDir;
    public float playerShotPow;
    private Image pressureBar;

	/// You are going to want a constructor

    public WeaponClass (float RoF, float pCo, int dmg, GameObject bulletPrefab, Transform gBarrel, Vector3 shotDir,float shotPow,Image pBar)
	{
		this.fireRate = RoF;
		this.pressureCost = pCo;
		this.bulletDmg = dmg;
		this.bulletType = bulletPrefab;
        this.playerBarrel = gBarrel;
        this.playerShotDir = shotDir;
        this.playerShotPow = shotPow;
        this.pressureBar = pBar;
	}
	//Might not work, but something like that
	
	
    void SpawnBullet(int dmg)
    {
        GameObject bulletObj = MonoBehaviour.Instantiate(bulletType,playerBarrel.transform.position, playerBarrel.transform.rotation) as GameObject;
        bulletObj.GetComponent<Rigidbody>().AddForce(playerShotDir * playerShotPow, ForceMode.Impulse); 
        bulletObj.GetComponent<BulletScript>().myDmg = dmg;
    }

    public void Shoot()
    {
        if (Time.time > fireRate + lastShot)
        {
            if (currentPressure > pressureCost)
            {
                currentPressure -= pressureCost;
                pressureBar.fillAmount -= (pressureCost / maxPressure);
                Debug.Log("My current Pressure is:" + currentPressure);

            }
            else
                {
                    bulletDmg = 1;
                Debug.Log("Not enough pressure to fire at full power! Setting damage to 1");
                if (pressureBar.fillAmount <= 0.1f)
                    pressureBar.fillAmount = 0.1f;
                    currentPressure = 10f;
                }


            SpawnBullet(bulletDmg);
            lastShot = Time.time;
        }
    }
}

public class PlayerController:MonoBehaviour{
    Animator anim;
    Rigidbody rb;


    public float jumplimit;  
    public float shotPower;
    public bool isGrounded;
    public Transform gunBarrel;
    public Image PressureBar;
    private int takeDmg;



    public Vector3 shotDirection = Vector3.right;


    [SerializeField]
    private int myHealth = 25;
    [SerializeField]
    private float jumpheight;
    [SerializeField]
    private float playerBounds;
    int currentWeapon = 0; // Let's choose a meaningful name like "currentWeapon"
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;



    public WeaponClass[] myWeapons = new WeaponClass[3]; //Creates a new Array Class of Type WeaponClass to be filled in below.
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        myWeapons[0] = new WeaponClass(1f, 10f, 1, bullet1,gunBarrel,shotDirection,shotPower,PressureBar); //RateofFire,PressureCost,Damage,prefab,GunBarrel,ShotDirection,ShotPower
        myWeapons[1] = new WeaponClass(1.5f,20f,2,bullet2,gunBarrel,shotDirection,shotPower,PressureBar);
        myWeapons[2] = new WeaponClass(2f,30f,3,bullet3,gunBarrel,shotDirection,shotPower,PressureBar);// creates a new instance of the Weapon class and fills in the specific index of the array class
        //This method will save quite a bit of nested if statements, and is quite elegant
        anim.SetFloat("Health",myHealth);

    }



    void Update()
    {
        PlayerLimits();
        SwitchWeapon();
        FireWeapon();
        Movement();
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }
    }



    void PlayerLimits()
    {
        if (transform.position.x <= -playerBounds) { //if player is going to the left
            transform.position = new Vector2(-playerBounds, transform.position.y);
        } else if (transform.position.x >= playerBounds) { //if the player is going to the right
            transform.position = new Vector2(playerBounds, transform.position.y);
        }

    }


    public static float CalculateVertJump( float jumptarget)
    {
        return Mathf.Sqrt((2f * jumptarget)  /** Physics.gravity.y*/);
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if ( currentWeapon < 2)
            {
                currentWeapon+=1;
                Debug.Log("Weapon type:" + currentWeapon);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentWeapon > 0)
            {
                currentWeapon-=1;
                Debug.Log("Weapon type:" + currentWeapon);
            }
        }
		// I would add a safety here to keep from going out of bounds
    }

    void FireWeapon()
    {
        if (Input.GetKey(KeyCode.Space))
        {
			myWeapons[currentWeapon].Shoot();                      
        }
        

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            takeDmg = collision.gameObject.GetComponent<BulletScript>().myDmg;
            myHealth -= takeDmg;
            Debug.Log("My health is at:" + myHealth);
            if (myHealth <= 0)
            {
                anim.SetFloat("Health", myHealth); // value to trigger the death animation
                Destroy(gameObject,2f); //destroys player gameobject after 2 seconds
            }                
        }
    }
    void Movement()
    {
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * 3f * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
            gunBarrel.transform.localPosition = new Vector3(0.316f, 0f, 0f);
            shotDirection = Vector3.right; //shoots in whatever direction the player ios facing, as I do not have appropriate assets to indicate which direction the player is facing
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * 3f * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
            gunBarrel.transform.localPosition = new Vector3(-0.316f, 0f, 0f);
            shotDirection = Vector3.left;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (isGrounded) //checks to see if the player is touching the ground, if true then the player can use jump again
            {
                Vector3 velocity = rb.GetComponent<Rigidbody>().velocity;
                velocity.y = CalculateVertJump(jumpheight); //returns values that range in a parabola to give realistic jump action
                rb.velocity = velocity;
            }        
        } 
    }
}
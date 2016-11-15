using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Image=UnityEngine.UI.Image;
using Text=UnityEngine.UI.Text;


public class WeaponClass
{
	/// I'm of the opinion everything here should be private or protected ///
    public float fireRate;
    public float pressureCost;
    public float bulletDmg;
    public string weaponName;
    public int dmgMod;
    public int baseDmg;

    private GameObject bulletType;
    private Transform playerBarrel;
    public Vector3 playerShotDir;
    private float playerShotPow;



    public WeaponClass (float RoF, float pCo, float dmg,int dBase,int dMod, GameObject bulletPrefab, Transform gBarrel, Vector3 shotDir,float shotPow,string wName)
	{
		this.fireRate = RoF;
		this.pressureCost = pCo;
		this.bulletDmg = dmg;
        this.baseDmg = dBase;
        this.dmgMod = dMod;

		this.bulletType = bulletPrefab;
        this.playerBarrel = gBarrel;
        this.playerShotDir = shotDir;
        this.playerShotPow = shotPow;
        this.weaponName = wName;
	}

	
	
    void SpawnBullet(float dmg)
    {
        GameObject bulletObj = MonoBehaviour.Instantiate(bulletType,playerBarrel.transform.position, playerBarrel.transform.rotation) as GameObject;
        bulletObj.GetComponent<Rigidbody>().AddForce(playerShotDir * playerShotPow, ForceMode.Impulse); 
        bulletObj.GetComponent<BulletScript>().myDmg = dmg;
    }

    public void Shoot()
    {                
            SpawnBullet(bulletDmg);
    }
}




public class PlayerController:MonoBehaviour{
    Animator anim;
    Rigidbody rb;

    [Header ("UI Elements")]
    [Space(5)]
    public Image PressureBar;
    public Text PressureText;
    public Image HealthBar;
    public Text HealthText;
    [Header("Player Attributes")]
    [Space(5)]
    public float jumplimit;  
    public Vector3 shotDirection = Vector3.right;
    public bool isGrounded;
    [SerializeField]
    private float jumpheight;
    public float shotPower;
    public Transform gunBarrel;
    [SerializeField]
    private float myHealth = 25;
    private float maxHealth = 25;
    [SerializeField]
    private float playerBounds;
    [Space(5)]

    [Header("Weapon Attributes")]
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public float currentPressure = 100f;
    public float maxPressure = 100f;
    private float takeDmg;
    public int currentWeapon = 0;
    private float lastShot = 0f;

   



    public WeaponClass[] myWeapons = new WeaponClass[3]; //Creates a new Array Class of Type WeaponClass to be filled in below.
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        myWeapons[0] = new WeaponClass(1f, 10f, 0f,1,0, bullet1,gunBarrel,shotDirection,shotPower,"light"); //RateofFire,PressureCost,Damage,prefab,GunBarrel,ShotDirection,ShotPower
        myWeapons[1] = new WeaponClass(1.5f,20f,0f,1,1,bullet2,gunBarrel,shotDirection,shotPower,"medium");
        myWeapons[2] = new WeaponClass(2f,30f,0f,1,2,bullet3,gunBarrel,shotDirection,shotPower,"heavy");// creates a new instance of the Weapon class and fills in the specific index of the array class
        //This method will save quite a bit of nested if statements, and is quite elegant
        anim.SetFloat("Health",myHealth); // prevents player from instantly playing the death animation
        PressureText.text = ("Pressure:" + currentPressure);
        HealthText.text = ("Health: " + myHealth);

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
            currentWeapon = (currentWeapon + 1) % 3; //using mod to wrap the player's weapons after overflow
            Debug.Log("Weapon type:" + myWeapons[currentWeapon].weaponName);
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {/*
            if ((currentWeapon % 3) == 0)
            {
                currentWeapon = 2;
            }
            else
                currentWeapon -= 1;*/
            currentWeapon = (currentWeapon -1) % 3;
            currentWeapon = currentWeapon < 0 ? currentWeapon + 3 : currentWeapon;
                
            Debug.Log("Weapon type:" + myWeapons[currentWeapon].weaponName);
        }
        
    }
   
    void FireWeapon()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > myWeapons[currentWeapon].fireRate + lastShot)
            { 
                if (currentPressure > myWeapons[currentWeapon].pressureCost)
                {
                    currentPressure -= myWeapons[currentWeapon].pressureCost; //subtracting the bullet's pressure cost from the player's pressure
                    PressureText.text = ("Pressure:" + currentPressure); //updating the pressure text
                    if (PressureBar.fillAmount <= 0.1f)
                    {
                        PressureBar.fillAmount = 0.1f;
                    }
                    else
                    {
                        PressureBar.fillAmount -= (myWeapons[currentWeapon].pressureCost / maxPressure);
                    }
                    myWeapons[currentWeapon].bulletDmg = (myWeapons[currentWeapon].dmgMod + myWeapons[currentWeapon].baseDmg);
                }
                else
                {                    
                    Debug.Log("Not enough pressure to fire at full power! Setting damage to 1");
                    myWeapons[currentWeapon].bulletDmg = myWeapons[currentWeapon].baseDmg;                    
                    currentPressure = 10f;
                }
                myWeapons[currentWeapon].Shoot(); 
                //after checking to see whether or not the player has enough pressure in the tank to fire, the bullet's damage will then be assigned accordingly. After which, this bullet will be fired.
                lastShot = Time.time;
            }			
        }
        

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            if (myHealth > 0)
            {
                takeDmg = collision.gameObject.GetComponent<BulletScript>().myDmg; // getting the damage value from the bullet
                myHealth -= takeDmg; //subtract damage from bullet
                HealthBar.fillAmount -= (takeDmg/maxHealth); //update healthbar fill
                UpdateHealth();

                //Debug.Log("My health is at:" + myHealth);
                if (myHealth <= 0)
                {
                    anim.SetFloat("Health", myHealth); // value to trigger the death animation
                    Destroy(gameObject, 2f); //destroys player gameobject after 2 seconds
                }                
            }
        }
    }

    void ApplyDamage(float damage)
    {
        Debug.Log("Hit for: " + damage + " health.");
        myHealth -= damage;
        UpdateHealth();
    }
    void UpdateHealth()
    {
        HealthText.text = ("Health: " + myHealth);
        HealthBar.fillAmount = (myHealth / maxHealth);
    }
    void Movement()
    {
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * 3f * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
            gunBarrel.transform.localPosition = new Vector3(0.316f, 0f, 0f);
            shotDirection = Vector3.right; //shoots in whatever direction the player is facing, as I do not have appropriate assets to indicate which direction the player is facing
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
    void Update()
    {
        PlayerLimits();
        SwitchWeapon();
        myWeapons[currentWeapon].playerShotDir = shotDirection;
        FireWeapon();
        Movement();
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }
    }
}
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
    //PlayerController myPlayer = new PlayerController();

	/// You are going to want a constructor

    public WeaponClass (float RoF, float pCo, int dmg, GameObject bulletPrefab, Transform gBarrel, Vector3 shotDir,float shotPow)
	{
		this.fireRate = RoF;
		this.pressureCost = pCo;
		this.bulletDmg = dmg;
		this.bulletType = bulletPrefab;
        this.playerBarrel = gBarrel;
        this.playerShotDir = shotDir;
        this.playerShotPow = shotPow;
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
        ///Debug.Log("testing132");
        if (Time.time > fireRate + lastShot)
        {
            if (currentPressure / maxPressure >= pressureCost)
            {
                currentPressure -= pressureCost;
            }
            else
                {
                    bulletDmg = 1;
                    //Debug.Log("The result is:" + (currentPressure / maxPressure) + ", and the bullet damage is:" + bulletDmg);
               // if (myPlayer.PressureBar.fillAmount <= 0.1f)
                  //  myPlayer.PressureBar.fillAmount = 0.1f;
                   // else
                   // myPlayer.PressureBar.fillAmount -= 0.1f;

                    currentPressure = 10f;
                }


            SpawnBullet(bulletDmg);
            lastShot = Time.time;
            //Debug.Log(currentPressure);
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



    public WeaponClass[] myWeapons = new WeaponClass[2]; // Let's choose a meaningful name here. Like myWeapons.
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        myWeapons[0] = new WeaponClass(1f, 10f, 1, bullet1,gunBarrel,shotDirection,shotPower); //RateofFire,PressureCost,Damage,prefab,GunBarrel,ShotDirection,ShotPower
        //Debug.Log(myWeapons[0].bulletDmg);
        myWeapons[1] = new WeaponClass(1.5f,20f,2,bullet2,gunBarrel,shotDirection,shotPower);

    }



    void Update()
    {
        //Mathf.Clamp(this.transform.position.x, -2, 2);
        PlayerLimits();
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
            /// The whole point is so you don't have to set this stuff on the fly.
			/// You should have set this stuff up in start already
			myWeapons[currentWeapon].Shoot();

            if (PressureBar.fillAmount <= myWeapons[currentWeapon].pressureCost)
                PressureBar.fillAmount = myWeapons[currentWeapon].pressureCost;
            else
                PressureBar.fillAmount -= myWeapons[currentWeapon].pressureCost;
           
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
                Destroy(gameObject);
        }
    }
    void Movement()
    {
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * 3f * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
            gunBarrel.transform.localPosition = new Vector3(0.316f, 0f, 0f);
            shotDirection = Vector3.right; 
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
            if (isGrounded)
            {
                Vector3 velocity = rb.GetComponent<Rigidbody>().velocity;
                velocity.y = CalculateVertJump(jumpheight);
                rb.velocity = velocity;
            }
        
        }    


    }

}
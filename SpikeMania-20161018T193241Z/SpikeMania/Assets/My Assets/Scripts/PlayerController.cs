﻿using UnityEngine;
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
    public int selectedWeapon = 0; // should go in PlayerController
    public float fireRate;
    public float fireRateMod; // might not need this now
    public float pressureCost;
    public int bulletDmg;
    public float currentPressure = 100f;
    private float maxPressure = 100f;
    private float lastShot = 0f; // might want to use logic for this instead of hardcoding it
    public GameObject bulletType;
    public Image PressureBar; // definetly goes in PlayerController, possibly a statis UI class even


	/// You are going to want a constructor
	/*
	WeaponClass (float RoF, float pCo, int dmg, GameObject bulletPrefab)
	{
		this.fireRate = RoF;
		this.pressureCost = pCo;
		this.bulletDmg = dmg;
		this.bulletType = bulletPrefab;
	}
	Might not work, but something like that
	*/
	
    void SpawnBullet(int dmg)
    {
        GameObject bulletObj = Instantiate(bulletType, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
        bulletObj.GetComponent<Rigidbody>().AddForce(shotDirection * shotPower, ForceMode.Impulse); 
        bulletObj.GetComponent<BulletScript>().myDmg = dmg;
    }

    public void Shoot()
    {
        if (Time.time > (fireRate + fireRateMod) + lastShot)
        {
            //pressureCost = 10f;
            //bulletType = //figure out a way to set it to bullet(at selectedweapon)
            if (currentPressure / maxPressure >= pressureCost)
            {
                //bulletDmg = 1;
                currentPressure -= pressureCost;
                if (PressureBar.fillAmount <= pressureCost)
                    PressureBar.fillAmount = pressureCost;
                else
                    PressureBar.fillAmount -= pressureCost;
            }
            /*else
                {
                    bulletDmg = 1;
                    //Debug.Log("The result is:" + (currentPressure / maxPressure) + ", and the bullet damage is:" + bulletDmg);
                    if (PressureBar.fillAmount <= 0.1f)
                        PressureBar.fillAmount = 0.1f;
                    else
                        PressureBar.fillAmount -= 0.1f;

                    currentPressure = 10f;
                }*/



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

    private int takeDmg;



    Vector3 shotDirection = Vector3.right;


    [SerializeField]
    private int myHealth = 25;
    [SerializeField]
    private float jumpheight;
    [SerializeField]
    private float playerBounds;
    int weaponType = 0; // Let's choose a meaningful name like "currentWeapon"
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;



    public WeaponClass[] _WeaponClass = new WeaponClass[3]; // Let's choose a meaningful name here. Like myWeapons.
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
		// call WeaponClass contructor and assign to array[0]
		// call WeaponClass contructor and assign to array[1]
		// call WeaponClass contructor and assign to array[2]

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
            if ( weaponType < 2)
            {
                weaponType+=1;
                Debug.Log("Weapon type:" + weaponType);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (weaponType > 0)
            {
                weaponType-=1;
                Debug.Log("Weapon type:" + weaponType);
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
			// myWeapons[currentWeapon].Shoot();
            switch(weaponType)
            {
                case(0):
                    _WeaponClass[weaponType].selectedWeapon = 0;
                    _WeaponClass[weaponType].fireRateMod = 0;
                    _WeaponClass[weaponType].pressureCost = 10f;
                    _WeaponClass[weaponType].bulletDmg = 1;
                    _WeaponClass[weaponType].bulletType = bullet1; // figure out way to increment name with weapontype
                    _WeaponClass[weaponType].Shoot();
                    break;
                case(1):
                    _WeaponClass[weaponType].selectedWeapon = 1;
                    _WeaponClass[weaponType].fireRateMod = 0.5f;
                    _WeaponClass[weaponType].pressureCost = 20f;
                    _WeaponClass[weaponType].bulletDmg = 2;
                    _WeaponClass[weaponType].bulletType = bullet2; // figure out way to increment name with weapontype
                    _WeaponClass[weaponType].Shoot();
                    break;
            }
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
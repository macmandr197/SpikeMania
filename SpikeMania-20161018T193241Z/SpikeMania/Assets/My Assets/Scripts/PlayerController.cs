using UnityEngine;
using System.Collections;
using Image=UnityEngine.UI.Image;

public class ShotType
{
    //fireRate(or mod)
    //bulletType
    //damage
    //pressure cost

    //create objct of weapontype with all of bullet's attribute stored within (damage calculation stored within weapontype object)
    //construct 3 weapons for each weapon type in array (when firing, spawns bullet with attributes stored in array at weaponarray[index]
    // abstract out all hardcoded names and variables and replace with call to weaponarray and use those attributes
}

public class PlayerController:MonoBehaviour{
    Animator anim;
    Rigidbody rb;

    public float jumplimit;
    [SerializeField]
    private float playerBounds;
    public float shotPower;
    public bool isGrounded;
    public Transform gunBarrel;
    public float fireRate;

    public Image PressureBar;

    Vector3 shotDirection = Vector3.right;
    int shotType = 0;

    [SerializeField]
    private float currentPressure = 100f;
    private float maxPressure = 100f;
    private float pressureCost;
    [SerializeField]
    private float jumpheight;
    private float lastShot = 0f;
    private int bulletDmg;
    GameObject bulletType;
    public GameObject BaseBullet;
    public GameObject SecondBullet;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }



    void Update()
    {
        //Mathf.Clamp(this.transform.position.x, -2, 2);
        PlayerLimits();
        SwitchWeapon();
        Movement();
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
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
            if (shotType < 2)
            {
                shotType+=1;
                Debug.Log("Weapon type:" + shotType);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (shotType > 0)
            {
                shotType-=1;
                Debug.Log("Weapon type:" + shotType);
            }
        }
    }

    void SpawnBullet(int dmg)
    {
        GameObject bulletObj = Instantiate(bulletType, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
        bulletObj.GetComponent<Rigidbody>().AddForce(shotDirection * shotPower, ForceMode.Impulse); 
        bulletObj.GetComponent<BulletScript>().myDmg = dmg;

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

        if (Input.GetKey(KeyCode.Space))
        {
            switch (shotType)
            {
                case(0):
                    if (Time.time > fireRate + lastShot)
                    {
                        pressureCost = 10f;
                        bulletType = BaseBullet;
                        if (currentPressure / maxPressure >= 0.1f)
                        {
                            bulletDmg = 1;
                            currentPressure -= pressureCost;
                            if (PressureBar.fillAmount <= 0.1f)
                                PressureBar.fillAmount = 0.1f;
                            else
                                PressureBar.fillAmount -= 0.1f;
                        }
                        else
                        {
                            currentPressure = pressureCost;
                        }
                            
                        SpawnBullet(bulletDmg);


                        lastShot = Time.time;
                        //Debug.Log(currentPressure);
                    }

                break;
                case(1):
                    if(Time.time > (fireRate + 0.5) + lastShot)
                    {
                        pressureCost = 20f;
                        bulletType = SecondBullet;

                        if ((currentPressure / maxPressure) >= 0.2f)
                        {
                            bulletDmg = 2;

                            currentPressure -= pressureCost;

                            if (PressureBar.fillAmount <= 0.2f)
                                PressureBar.fillAmount = 0.2f;
                            else
                                PressureBar.fillAmount -= 0.2f;
                            //Debug.Log("The result is:" + (currentPressure / maxPressure) + ", and the bullet damage is:" + bulletDmg);
                        }
                        else 
                        {
                            bulletDmg = 1;
                            //Debug.Log("The result is:" + (currentPressure / maxPressure) + ", and the bullet damage is:" + bulletDmg);
                            if (PressureBar.fillAmount <= 0.1f)
                                PressureBar.fillAmount = 0.1f;
                            else
                                PressureBar.fillAmount -= 0.1f;
                            
                            currentPressure = 10f;
                        }
                        SpawnBullet(bulletDmg);
                        lastShot = Time.time;
                        //Debug.Log(currentPressure);
                    }
                    break;
            }
        }
    }

}
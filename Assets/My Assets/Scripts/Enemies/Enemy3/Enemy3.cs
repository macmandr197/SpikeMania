using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy3 : MonoBehaviour {

    [SerializeField] private float distanceToTarget = 2f;
    private Animator anim;
    private Rigidbody rb;
    public bool isGrounded;
    [SerializeField]
    private float jumpheight;

    [SerializeField]private float range = 1.5f;

    public List<Collider> potentialTargets = new List<Collider>();
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public static float CalculateVertJump(float jumptarget)
    {
        return Mathf.Sqrt(8f * jumptarget /** Physics.gravity.y*/);
    }

    void DetectBullets()
    {
        
        
        
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
        var target = GameObject.Find("Character").transform.position;
        if ((Vector2.Distance(transform.position, target) >= distanceToTarget) && (transform.position.x < 3.2)) //if enemy is out of range of player, but less than map bounds, move towards player
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 1f * Time.deltaTime);
        }
        else// if (Vector2.Distance(transform.position, target) < distanceToTarget && transform.position.x < 3.2 && transform.position.x > (-3.2))
        {
            Vector3 dir = transform.position - target;
            transform.Translate(dir * 0.5f * Time.deltaTime);
        }
        
    }
	// Update is called once per frame
	void Update ()
    {
        Movement();
        DetectBullets();
        DistanceCheck();
        {
            var move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", move);
            //Debug.Log(move);
        }

        
    }
}

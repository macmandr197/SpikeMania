using UnityEngine;
using System.Collections;

public class Seeker : MonoBehaviour {
    public float circRad = 0.005f;
    public float angle = 0f;
    Vector3 myPos;
    public GameObject bomb;

    [SerializeField]
    private float distanceToTarget = 2;
    [SerializeField]
    float bombInterval = 1f;


    void Start() { 
        StartCoroutine(DropBombs());
    } 

    IEnumerator DropBombs()
    {
        yield return new WaitForSeconds(bombInterval);
        GameObject bombObj = Instantiate(bomb, transform.position, transform.rotation) as GameObject;
    }

    void DistanceCheck()
    {
        if (Vector2.Distance(transform.position,GameObject.Find("Character").transform.position) <= distanceToTarget)
        {
            StartCoroutine(DropBombs());
        }
    }

    void Update() {
        Vector3 target = GameObject.Find ("Character").transform.position + new Vector3(0.0f,1.5f,0.0f); 

        transform.position = Vector3.MoveTowards(transform.position,target,0.8f*Time.deltaTime);
        myPos.x = Mathf.Cos(angle) * circRad;
        myPos.y = Mathf.Sin(angle) * circRad;
        transform.Translate(myPos);
        angle += 0.05f;
    }
}
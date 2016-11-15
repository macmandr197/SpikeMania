using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bounds")
            Destroy(gameObject);
    }
}

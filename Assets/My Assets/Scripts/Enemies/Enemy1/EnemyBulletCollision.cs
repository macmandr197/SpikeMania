using UnityEngine;
using System.Collections;

public class EnemyBulletCollision : MonoBehaviour {
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
    
     else if (collision.gameObject.tag == "Bounds")
            Destroy(gameObject);
}
}

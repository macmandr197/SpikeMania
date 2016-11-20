using UnityEngine;
using System.Collections;

public class EnemyDetectGroundHit : MonoBehaviour {

    public Enemy3 refEnemy;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ground")
        {
            refEnemy.isGrounded = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ground")
        {
            refEnemy.isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        refEnemy.isGrounded = false;

    }
}

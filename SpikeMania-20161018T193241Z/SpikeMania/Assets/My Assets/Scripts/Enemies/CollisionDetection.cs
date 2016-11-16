// 2016  11 14 Andrew MacMillan

using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Bounds"))
            Destroy(gameObject);
    }
}
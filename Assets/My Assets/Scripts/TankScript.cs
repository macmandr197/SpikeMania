using UnityEngine;

public class TankScript : MonoBehaviour
{
    private readonly int pressureVal = 25;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if ((other.gameObject.GetComponent<PlayerController>().currentPressure += pressureVal) <=
                other.gameObject.GetComponent<PlayerController>().maxPressure)
            {
                other.gameObject.GetComponent<PlayerController>().currentPressure += pressureVal;
            }
            
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Bounds")
            Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(Vector2.left * 3.5f * Time.deltaTime);
    }
}
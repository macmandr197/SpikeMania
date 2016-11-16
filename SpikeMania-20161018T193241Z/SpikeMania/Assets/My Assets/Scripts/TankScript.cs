using UnityEngine;

public class TankScript : MonoBehaviour
{
    private readonly int pressureVal = 25;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().currentPressure += pressureVal;
            other.gameObject.GetComponent<PlayerController>().pressureText.text = "Pressure:" + other.gameObject.GetComponent<PlayerController>().currentPressure;
            other.gameObject.GetComponent<PlayerController>().pressureBar.fillAmount +=other.gameObject.GetComponent<PlayerController>().myWeapons[other.gameObject.GetComponent<PlayerController>().currentWeapon].pressureCost /other.gameObject.GetComponent<PlayerController>().maxPressure;
            Debug.Log(other.gameObject.GetComponent<PlayerController>().currentPressure);
            Debug.Log("Pressure get!");
            Destroy(gameObject);
        }
    }
}
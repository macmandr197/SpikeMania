using UnityEngine;
using System.Collections;

public class TankScript : MonoBehaviour {


    private int pressureVal = 25;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().currentPressure += pressureVal;
            other.gameObject.GetComponent<PlayerController>().PressureText.text = ("Pressure:" + other.gameObject.GetComponent<PlayerController>().currentPressure);
            other.gameObject.GetComponent<PlayerController>().PressureBar.fillAmount += (other.gameObject.GetComponent<PlayerController>().myWeapons[other.gameObject.GetComponent<PlayerController>().currentWeapon].pressureCost / other.gameObject.GetComponent<PlayerController>().maxPressure);
            Debug.Log(other.gameObject.GetComponent<PlayerController>().currentPressure);
            Debug.Log("Pressure get!");
            Destroy(gameObject);
        }
    }
}

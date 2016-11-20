using UnityEngine;
using System.Collections;

public class DetectGroundHit : MonoBehaviour {


        public PlayerController refPlayerController;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "ground") {
            refPlayerController.isGrounded = true;
            }
        }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "ground") {
            refPlayerController.isGrounded = true;
            }
        }

    void OnTriggerExit(Collider other) {
        
            refPlayerController.isGrounded=false;
            
        }
}

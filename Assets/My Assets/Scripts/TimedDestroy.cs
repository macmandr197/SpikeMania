using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour
{

    [SerializeField] private float DestroyTime = 0.5f;
	// Use this for initialization
	void Start () {
	Destroy(gameObject,DestroyTime);
	}
	
}

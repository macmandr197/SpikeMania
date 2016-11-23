using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.5f;
	// Use this for initialization
	void Start () {
	Destroy(gameObject,destroyTime);
	}
	

}

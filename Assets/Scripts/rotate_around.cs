using UnityEngine;
using System.Collections;

public class rotate_around : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		transform.RotateAround(Vector3.zero, new Vector3(0,0,1), 90f);
	}
}
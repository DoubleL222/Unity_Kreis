using UnityEngine;
using System.Collections;

public class Shot_controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Shot") {
			Destroy(gameObject);
		}
	}

}

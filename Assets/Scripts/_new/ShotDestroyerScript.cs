using UnityEngine;
using System.Collections;

public class ShotDestroyerScript : MonoBehaviour {
	private GameObject ExplosionEffect;
	// Use this for initialization
	void Start () {
		ExplosionEffect = Resources.Load ("_new/ExplosionEffect") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Segment") {
			//Destroy(col.transform.parent.parent.gameObject);
		}
		else if (col.gameObject.tag == "Player") {
			//Destroy(col.transform.parent.parent.gameObject);
		}
		//Destroy (gameObject.transform.parent.parent.gameObject);
	}
	/*void OnCollisionEnter2D(Collision2D col){
		Debug.Log ("ONCOLLISIONENTER");
	}*/
}

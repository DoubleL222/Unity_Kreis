using UnityEngine;
using System.Collections;

public class ShotDestroyerScript : MonoBehaviour {
	public GameObject ExplosionEffect;
	public Transform ShotTransform;
	public bool IsUsed = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerEnter2D(Collider2D col){
		if (!IsUsed) {
			if (col.gameObject.tag == "Shot") {
				IsUsed = true;
				if (col != null) {
					if (col.gameObject != null) {

						//Destroy(col.gameObject.transform.root.gameObject);
					}
				}
				//ExplosionEffect = Resources.Load ("_new/ExplosionEffect") as GameObject;
				if (ExplosionEffect != null && ShotTransform != null) {
					Instantiate (ExplosionEffect, ShotTransform.position, Quaternion.identity);
				}
				if (transform.root.gameObject != null) {
					Destroy (transform.root.gameObject);
				}
			}
		}
	}
}

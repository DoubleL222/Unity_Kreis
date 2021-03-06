﻿using UnityEngine;
using System.Collections;

public class ShotDestroyerScript : MonoBehaviour {
	public GameObject ExplosionEffect;
	public Transform ShotTransform;
	public bool IsUsed = false;
	public Transform root;

	private static CamShakeManager CamShakeM;

	// Use this for initialization
	void Awake ()
  	{
		CamShakeM = FindObjectOfType<CamShakeManager> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!IsUsed) {
			if (col.gameObject.tag == "Shot") {
				CamShakeManager.PlayShake (0.1f, 0.1f);
				IsUsed = true;
				if (col != null) {
					if (col.gameObject != null) {
						//Destroy(col.gameObject.transform.root.gameObject);
					}
				}
				//ExplosionEffect = Resources.Load ("_new/ExplosionEffect") as GameObject;
				if (ExplosionEffect != null && ShotTransform != null) {
					GameObject ee = Instantiate (ExplosionEffect, ShotTransform.position, Quaternion.identity) as GameObject;
					ee.transform.SetParent (GameManager.GMInstance.root.transform);
				}
				if (transform.root.gameObject != null) {
					Destroy (root.gameObject);
				}
			}
		}
		if (col.gameObject.tag == "Boundary") {
			Destroy (root.gameObject);
		}
		if (col.gameObject.tag == "Segment" && gameObject.tag != "PiercingShot") {
			if(transform.root.gameObject != null)
				MonoBehaviour.Destroy(root.gameObject);
		}
	}
}

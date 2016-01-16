using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentController : PolarPhysicsObject{
	private static int count = 0;
	
	public GameObject ExplosionEffect;

	// Use this for initialization
	IList <SegmentTickBehaviour> tickBehaviours;
	IList <SegmentCollisionBehaviour> collisionBehaviours;
	IList <SegmentTriggerBehaviour> triggerBehaviours;

	void Awake() {
		base.Awake();
		count++;
		tickBehaviours = new List<SegmentTickBehaviour> ();
		collisionBehaviours = new List<SegmentCollisionBehaviour> ();
		triggerBehaviours = new List<SegmentTriggerBehaviour> ();
		//Debug.Log ("Number of segments: " + count);
	}

	public void addBehaviour(SegmentTickBehaviour segmentTickBehaviour){
		tickBehaviours.Add (segmentTickBehaviour);
	}

	public void addBehaviour(SegmentCollisionBehaviour segmentCollisionBehaviour){
		collisionBehaviours.Add (segmentCollisionBehaviour);
	}

	public void addBehaviour(SegmentTriggerBehaviour segmentTriggerBehaviour){
		triggerBehaviours.Add (segmentTriggerBehaviour);
	}

	public void SetPosition(Vector2 pos){
		//Debug.Log ("SetPosition called with " + pos);
		float angle = pos.x;
		float distance = pos.y;

		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		//Debug.Log ("Calculated new position at " + mx + ", " + my + " and rotation at " + (angle + Mathf.PI/2f));

		mesh.transform.position = new Vector3(mx, my, 0);
		mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);

		float sc = scaleMultiplier / pos.y;
		physics.transform.position = new Vector2(pos.x * PolarPhysicsObject.widthMultiplier, pos.y);
		physics.transform.localScale = new Vector2(sc, sc);	

		//Debug.Log ("Segment pos set to " + mesh.transform.position + " " + physics.transform.position);
	}
		
	void FixedUpdate () {
		StartUpdate ();

		foreach (SegmentTickBehaviour segmentTickBehaviour in tickBehaviours) {
			segmentTickBehaviour.FixedTick(this);
		}

		EndUpdate();
	}


	void OnCollisionEnter2D(Collision2D col){
		foreach (SegmentCollisionBehaviour segmentCollisionBehaviour in collisionBehaviours) {
			segmentCollisionBehaviour.Enter (col, this);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		foreach (SegmentTriggerBehaviour segmentTriggerBehaviour in triggerBehaviours) {
			segmentTriggerBehaviour.Enter (other, this);
		}
		//LUKA
		if (other.gameObject.tag == "Shot") {
			Destroy(other.transform.root.gameObject);
			Debug.Log("MAKING EXPLOSION");
			Vector3 spawnPos = transformToPolar(other.transform.position);
			GameObject explosionInstance = Instantiate(ExplosionEffect, spawnPos, Quaternion.identity) as GameObject;
			mesh.transform.GetChild(0).gameObject.SetActive(false);
			explosionInstance.transform.SetParent(mesh.transform);
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			//Destroy(transform.root.gameObject);
			//explosionInstance.transform.SetParent(transform);

		}
		//END LUKA
	}
	//LUKA
	Vector3 transformToPolar(Vector3 pos){
		
		float angle = pos.x / 10.0f;
		float distance = pos.y;
		
		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		
		return new Vector3 (mx, my, 0.0f);
	}
	//END LUKA
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class representing a segment
/// </summary>

public class SegmentController : PolarPhysicsObject{

    spawnAnim spawnAnim;
    public GameObject ExplosionEffect;

	//Behaviours
	IList <SegmentTickBehaviour> tickBehaviours;
	IList <SegmentCollisionBehaviour> collisionBehaviours;
	IList <SegmentTriggerBehaviour> triggerBehaviours;

	bool isDestroyed = false;

	void Awake() {
		base.Awake();
		tickBehaviours = new List<SegmentTickBehaviour> ();
		collisionBehaviours = new List<SegmentCollisionBehaviour> ();
		triggerBehaviours = new List<SegmentTriggerBehaviour> ();
        spawnAnim = mesh.GetComponent<spawnAnim>();
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

	/// <summary>
	/// Sets the position of the physics in polar coordinates and the position of mesh in cartesian coordinates.
	/// </summary>
	/// <param name="pos">Position</param>
	public void SetPosition(Vector2 pos){
		
        float angle = pos.x;
		float distance = pos.y;

		float mx = distance * Mathf.Cos (angle); //x = r * cos(phi) 
		float my = distance * Mathf.Sin (angle); //y = r * sin(phi)	

		mesh.transform.position = new Vector3(mx, my, 0);
		mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f); //Keeps the segment rotated perpendicular to the center

		float sc = scaleMultiplier / pos.y;	//Polar physics objects need to scale based on distance from center
		physics.transform.position = new Vector2(pos.x * PolarPhysicsObject.widthMultiplier, pos.y);
		physics.transform.localScale = new Vector2(sc, sc);
    }
		
	void FixedUpdate () {
		StartUpdate ();

		foreach (SegmentTickBehaviour segmentTickBehaviour in tickBehaviours) { //Apply all tick behaviours
			segmentTickBehaviour.FixedTick(this);
		}

		EndUpdate();
	}


	void OnCollisionEnter2D(Collision2D col){
		foreach (SegmentCollisionBehaviour segmentCollisionBehaviour in collisionBehaviours) { //Apply all collision behaviours
			segmentCollisionBehaviour.Enter (col, this);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		foreach (SegmentTriggerBehaviour segmentTriggerBehaviour in triggerBehaviours) { //Applies all trigger behaviours
			segmentTriggerBehaviour.Enter (other, this);
		}
	}

	//LUKA
	public void DestroySegment(Vector3 explodePosition){
		if (!isDestroyed) {
			isDestroyed = true;
			GameObject explosionInstance = Instantiate (ExplosionEffect, explodePosition, Quaternion.identity) as GameObject;
			CamShakeManager.PlayTestShake (0.1f, 0.5f);
			SoundManager.PlayExplosionClip ();
			mesh.transform.GetChild (0).gameObject.SetActive (false);
			explosionInstance.transform.SetParent (mesh.transform);
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	public IEnumerator DestroySegmentAFter(float delay){
		//Debug.Log ("startedCoroutine");
		yield return new WaitForSeconds (delay);
		DestroySegment (UtilityScript.transformToCartesian (transform.position));
	}
	//END LUKA
	
}

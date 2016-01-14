using UnityEngine;
using System.Collections;

public class ShotController : PolarPhysicsObject {
	private Vector2 moveVelocity;
	private bool velocitySet;
	private float shotSpeed = 30.0f;
	// Use this for initialization
	void Awake () {
		base.Awake ();
	}

	public void setVelocity(Vector2 v){
		moveVelocity = v;
		velocitySet = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (velocitySet) {
			StartUpdate ();
			if (rigidbody.velocity != (moveVelocity * shotSpeed)) {
				rigidbody.velocity = moveVelocity * shotSpeed;
			}
			EndUpdate ();
		}
	}
}

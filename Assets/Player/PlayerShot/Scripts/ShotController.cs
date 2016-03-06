using UnityEngine;
using System.Collections;

public class ShotController : PolarPhysicsObject {

	public Transform root;
	private Vector2 moveVelocity;
	private bool velocitySet;
	private float shotSpeed = 30.0f;
	public ParticleSystem[] allParticleSystems;
	bool firstIter;
	// Use this for initialization
	void Awake () {
		base.Awake ();
		firstIter = true;
	}

	public void setVelocity(Vector2 v){
		moveVelocity = v;
		velocitySet = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (velocitySet) {
			StartUpdate ();
			if (firstIter) {
				foreach (ParticleSystem PS in allParticleSystems) {
					PS.Play ();
				}
				firstIter = false;
			}
			if (rigidbody.velocity != (moveVelocity * shotSpeed)) {
				rigidbody.velocity = moveVelocity * shotSpeed;
			}
			EndUpdate ();
		}
	}
}

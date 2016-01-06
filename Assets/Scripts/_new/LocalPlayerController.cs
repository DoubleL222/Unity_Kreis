using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : PolarPhysicsObject {
	private int gravity;

	private float lastGravityChangeTime;
	private static float gravityChangeDelay = 0.3f;

	private static float movementForce = 400f;
	private static float gravityForce = 30f;

	private IDictionary<string,string> keys;

	public void setKeys(IDictionary<string,string> keys){
		this.keys = keys;
	}

	// Use this for initialization
	void Awake() {
		Debug.Log ("Start called");
		base.Awake();
		gravity = 1;
		oldscale = scaleMultiplier/rigidbody.position.y;
		if (keys == null) {
			IDictionary<string,string> defaultKeys = new Dictionary<string,string> ();
			defaultKeys.Add ("left", "a");
			defaultKeys.Add ("right", "d");
			defaultKeys.Add ("gravityChange", "w");
			setKeys (defaultKeys);
		}
	}
		
	void FixedUpdate () {
		StartUpdate ();
		if (Input.GetKey (keys["left"])) {
			Vector2 f = new Vector2 (-movementForce, 0);
			Vector2 sc = new Vector2 (Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey (keys["right"])) {
			Vector2 f = new Vector2 (movementForce, 0);
			Vector2 sc = new Vector2 (Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey (keys["gravityChange"]) && lastGravityChangeTime + gravityChangeDelay < Time.time) {
			lastGravityChangeTime = Time.time;
			gravity = -gravity;
		}
		Vector2 grav = new Vector2(0f, gravityForce * gravity);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		rigidbody.AddForce(grav);
		EndUpdate ();
	}

}

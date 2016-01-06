using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : PolarPhysicsObject {

	public GameObject mesh;
	public GameObject physics;
	public Rigidbody rigidbody;

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
	void Start () {
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
		StartUpdate (mesh, physics, rigidbody);
		if (Input.GetKey (keys["left"])) {
			Vector3 f = new Vector3 (-movementForce, 0, 0);
			Vector3 sc = new Vector3 (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey (keys["right"])) {
			Vector3 f = new Vector3 (movementForce, 0, 0);
			Vector3 sc = new Vector3 (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey (keys["gravityChange"]) && lastGravityChangeTime + gravityChangeDelay < Time.time) {
			lastGravityChangeTime = Time.time;
			gravity = -gravity;
		}
		Vector3 grav = new Vector3(0f, gravityForce * gravity, 0f);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		rigidbody.AddForce(grav);
		EndUpdate (mesh, physics, rigidbody);
	}

}

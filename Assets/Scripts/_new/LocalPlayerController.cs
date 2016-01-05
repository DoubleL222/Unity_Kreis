using UnityEngine;
using System.Collections;

public class LocalPlayerController : PolarPhysicsObject {

	public GameObject mesh;
	public GameObject physics;

	Rigidbody rigidbody;

	private int gravity;

	private float lastGravityChangeTime;
	private static float gravityChangeDelay = 0.3f;

	private static float movementForce = 400f;
	private static float gravityForce = 30f;

	// Use this for initialization
	void Start () {
		rigidbody = physics.GetComponent<Rigidbody> ();
		gravity = 1;
		oldscale = scaleMultiplier/rigidbody.position.y;
	}
		
	void FixedUpdate () {
		StartUpdate (mesh, physics, rigidbody);
		if (Input.GetKey ("a")) {
			Vector3 f = new Vector3 (-movementForce, 0, 0);
			Vector3 sc = new Vector3 (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey ("d")) {
			Vector3 f = new Vector3 (movementForce, 0, 0);
			Vector3 sc = new Vector3 (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey ("w") && lastGravityChangeTime + gravityChangeDelay < Time.time) {
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

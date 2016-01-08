using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : PolarPhysicsObject {
	private int gravity;

	private float lastGravityChangeTime;
	private static float gravityChangeDelay = 0.4f;

	private static float movementForce = 1200;//400f;
	private static float gravityForce = 30f;

	private IDictionary<string,string> keys;

	//LUKA
	private float shotOffset = 1f;
	private float fireRate = 2.0f;
	private float lastShoot = 0.0f;
	GameObject shotPrefab;
	//END LUKA

	public void setKeys(IDictionary<string,string> keys){
		this.keys = keys;
	}

	// Use this for initialization
	void Awake() {
		//Debug.Log ("Start called");
		//LUKA
		shotPrefab = Resources.Load ("_new/PlayerShot") as GameObject;
		//END LUKA
		base.Awake();
		gravity = 1;
		oldscale = scaleMultiplier/rigidbody.position.y;
		if (keys == null) {
			IDictionary<string,string> defaultKeys = new Dictionary<string,string> ();
			defaultKeys.Add ("left", "a");
			defaultKeys.Add ("right", "d");
			defaultKeys.Add ("gravityChange", "w");
			//LUKA
			defaultKeys.Add ("shoot", "s");
			//END LUKA
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
		if (Input.GetKey (keys["gravityChange"]) && lastGravityChangeTime + gravityChangeDelay < Time.fixedTime) {
			lastGravityChangeTime = Time.fixedTime;
			gravity = -gravity;
		}
		if (Input.GetKey (keys["shoot"]) && (lastShoot+fireRate) < Time.fixedTime) {
			lastShoot = Time.fixedTime;
			GameObject shotInstance = MonoBehaviour.Instantiate(shotPrefab, physics.transform.position + new Vector3(0.0f, -gravity*shotOffset ,0.0f), new Quaternion()) as GameObject;
			Vector2 shotVel = new Vector2(0.0f, -gravity);
			Debug.Log(shotVel);
			shotInstance.GetComponent<ShotController>().setVelocity(shotVel);
		}
		Vector2 grav = new Vector2(0f, gravityForce * gravity);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		rigidbody.AddForce(grav);

		//rotate player mesh
		{
			float rotationTime = (Time.fixedTime - lastGravityChangeTime) / (gravityChangeDelay);
			rotationTime = Mathf.Clamp01 (rotationTime);
			Vector3 tmp = mesh.transform.rotation.eulerAngles;
			if (gravity < 0)
				tmp.z += rotationTime * 180;
			else
				tmp.z += (1 - rotationTime) * 180;
			Quaternion rot = mesh.transform.rotation;
			rot.eulerAngles = tmp;
			mesh.transform.rotation = rot;
		}
		EndUpdate ();
	}

}

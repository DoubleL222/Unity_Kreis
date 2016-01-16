using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : PolarPhysicsObject {
	private int gravity;

	private float lastGravityChangeTime;
	private static float gravityChangeDelay = 0.3f;

	private static float movementForce = 1200;//400f;
	private static float gravityForce = 30f;

	private IDictionary<string,string> keys;

	//LUKA
	private float shotOffset = 1f;
	private float fireRate = 0.1f;
	private float lastShoot = 0.0f;
	GameObject shotPrefab;
	//END LUKA


	// medo

	public AudioClip[] sfx_crashes;
	public AudioClip[] sfx_gravity;
	public AudioClip sfx_spawn;
	public AudioClip sfx_death;
	public AudioClip sfx_shoot;
	public AudioClip sfx_drive;

	// end medo

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

		Debug.Log ("mf" + movementForce);

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

			// sound
			if(this.gravity < 0) 
				//StartCoroutine(Sound_mgr_script.PlaySingle(sfx_gravity[1], 0.0f));
				Sound_mgr_script.instance.PlaySingle(sfx_gravity[0], 0.0f);
			else 
				//StartCoroutine(Sound_mgr_script.PlaySingle(sfx_gravity[0], 0.0f));
				Sound_mgr_script.instance.PlaySingle(sfx_gravity[1], 0.0f);
		}
		if (Input.GetKey (keys["shoot"]) && (lastShoot+fireRate) < Time.time) {
			lastShoot = Time.time;
			GameObject shotInstance = MonoBehaviour.Instantiate(shotPrefab, physics.transform.position + new Vector3(0.0f, -gravity*shotOffset ,0.0f), new Quaternion()) as GameObject;
			Vector2 shotVel = new Vector2(0.0f, -gravity);
			Debug.Log(shotVel);
			shotInstance.GetComponent<ShotController>().setVelocity(shotVel);

			// sound
			//StartCoroutine( Sound_mgr_script.PlaySingle(sfx_shoot, 0.0f));
			Sound_mgr_script.instance.PlaySingle(sfx_shoot, 0.0f);

		}
		Vector2 grav = new Vector2(0f, gravityForce * gravity);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		
		rigidbody.AddForce(grav);
		Vector2 vel_vector = rigidbody.velocity;
		float vel = vel_vector.magnitude;
		//Sound_mgr_script.instance.PlaySingle (sfx_drive, vel);
		EndUpdate ();
	}

}

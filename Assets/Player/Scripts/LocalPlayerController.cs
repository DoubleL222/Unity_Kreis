using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : PolarPhysicsObject {
	public int gravity;
	private LayerMask WhatIsGround;

	bool isGrounded = true;
	private static SoundManager SoundM;

	private float descelerationRate = 0.995f;

	private static float gravityChangeDelay = 0.4f;

	private static float movementForce = 1200f;//400f;
	private static float gravityForce = 120f;//30f

	private IDictionary<string,KeyCode> keys;

  // powerups
  [HideInInspector]
  public bool hasShield = false;
  public GameObject shieldSprite;

	//LUKA
	public string PlayerName;
	public GameObject boosterEmiter;
  private float shotOffset = 1.5f;
	private float gravityChangeRate = 0.2f;
	private float lastGravityChange = 0.0f;

	private float fireRate = 1.5f;
	private float lastShoot = 0.0f;
	public GameObject shotPrefab;
	//END LUKA

	public void setKeys(IDictionary<string,KeyCode> keys){
		this.keys = keys;
	}

	// Use this for initialization
	void Awake() {
		SoundM = FindObjectOfType<SoundManager> ();
		base.Awake();
		//Debug.Log ("Start called");
		gravity = 1;
		oldscale = scaleMultiplier/rigidbody.position.y;
		if (keys == null) {
			IDictionary<string,KeyCode> defaultKeys = new Dictionary<string,KeyCode> ();
			defaultKeys.Add ("left", KeyCode.A);
			defaultKeys.Add ("right", KeyCode.D);
			defaultKeys.Add ("gravityChange",KeyCode.S);
			defaultKeys.Add ("shoot", KeyCode.W);
			setKeys (defaultKeys);
		}
	}
		
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (physics.transform.position, 1.0f);
	}

	void FixedUpdate () {
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.


		Collider2D[] colliders = Physics2D.OverlapCircleAll(physics.transform.position, 1.0f);
		if (!isGrounded) {
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject.tag == "Segment") {
					isGrounded = true;
					//Debug.Log ("Grounded");
				}
			}
		}

		StartUpdate ();
		if (Input.GetKey (keys ["left"])) {
			if(!boosterEmiter.activeSelf){
				boosterEmiter.SetActive(true);
			}
			Vector2 f = new Vector2 (-movementForce, 0);
			Vector2 sc = new Vector2 (Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		} else if (Input.GetKey (keys ["right"])) {
			if(!boosterEmiter.activeSelf){
				boosterEmiter.SetActive(true);
			}
			Vector2 f = new Vector2 (movementForce, 0);
			Vector2 sc = new Vector2 (Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		} else {
			if(boosterEmiter.activeSelf){
				boosterEmiter.SetActive(false);
			}
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x*descelerationRate, rigidbody.velocity.y);
		}
		if (Input.GetKey (keys["gravityChange"]) && isGrounded && (lastGravityChange+gravityChangeRate)<Time.fixedTime) {
			lastGravityChange = Time.fixedTime;
			gravity = -gravity;
			SoundM.PlayJumpClip ();
		}
		if (Input.GetKey (keys["shoot"]) && isGrounded &&(lastShoot+fireRate) < Time.fixedTime) {
			lastShoot = Time.fixedTime;
			GameObject shotInstance = MonoBehaviour.Instantiate(shotPrefab, physics.transform.position + new Vector3(0.0f, -gravity*shotOffset ,0.0f), new Quaternion()) as GameObject;
			Vector2 shotVel = new Vector2(0.0f, -gravity);
			shotInstance.GetComponent<ShotController>().setVelocity(shotVel);
			SoundM.PlayShotClip ();
		}
		Vector2 grav = new Vector2(0f, gravityForce * gravity);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		rigidbody.AddForce(grav);

		//rotate player mesh
		{
			float rotationTime = (Time.fixedTime - lastGravityChange) / (gravityChangeDelay);
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

  // enable shield
  public void EnableShield()
  {
    hasShield = true;
    StartCoroutine(ShieldScaleIn());
	}

  IEnumerator ShieldScaleIn()
  {
    // enable and scale down shield
    shieldSprite.SetActive(true);
    shieldSprite.transform.localScale = new Vector3(.0f, .0f, .0f);

    float elapsed = 0.0f;
    // animation duration
    float duration = .5f;
    Vector3 newScale;

    // increase scale for duration
    while (elapsed < duration)
    {
      elapsed += Time.deltaTime;

      float scale = Mathf.SmoothStep(.0f, 1.0f, elapsed / duration);
      newScale = new Vector3(scale, scale, .0f);
      shieldSprite.transform.localScale = newScale;

      yield return null;
    }

    // set scale to 1
    newScale = new Vector3(1.0f, 1.0f, .0f);
    shieldSprite.transform.localScale = newScale;
  }

  // disable shield
  public void DisableShield()
  {
    hasShield = false;
    shieldSprite.SetActive(false);
  }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : PolarPhysicsObject, IDestroyable
{
	[HideInInspector]
	public int gravity;
	private LayerMask WhatIsGround;

	[HideInInspector]
	public int playerIndex;

	bool isGrounded = true;
	private static SoundManager SoundM;
	[HideInInspector]
	public int team;

	private float descelerationRate = 0.995f;

	private static float gravityChangeDelay = 0.4f;

	private static float movementForce = 5000f;
//400f;
	private static float gravityForce = 120f;
//30f

	private IDictionary<string,string> keys;
	[HideInInspector]
	public bool MultiBool = false;

  public GameObject[] glows;
  private Vector3[] glowScales;
  private float interpolateT = 1;



	//LUKA
	public GameObject ExplosionEffect;
	private GameManager gManager;
	[HideInInspector]
	public string PlayerName;

  public GameObject boosterEmiter;
  private float shotOffset = 1.5f;
  private float gravityChangeRate = 0.2f;
  private float lastGravityChange = 0.0f;

  private int shootCharges = 3;
  private int shots = 0;
  private float fireRate = 0.2f;
  private float lastShoot = 0.0f;
  private float rechargeRate = 2.0f;
  private float lastRecharge = 0.0f;
  private int jumpCharge = 0;

	public GameObject shotPrefab;
  //END LUKA

  // keys
  [HideInInspector]
  public bool firePressed = false;
  [HideInInspector]
  public bool leftPressed = false;
  [HideInInspector]
  public bool rightPressed = false;
  [HideInInspector]
  public bool jumpPressed = false;

	private PlayerCollisionDetector myPCD;
	private Rigidbody2D myRBD2D;

  // powerups
  [Header("PowerUp Settings:")]
  public GameObject shieldSprite;
  [HideInInspector]
  public bool hasShield = false;
  [HideInInspector]
  public bool piercingShot = false;
  public GameObject piercingShotSprite;
  public GameObject piercingShotPrefab;
  [HideInInspector]
  public bool isBulldozer = false;
  public float bulldozerDuration;

  public void setKeys(IDictionary<string,string> keys){

		this.keys = keys;
	}

	public void DestroyObject ()
	{
		GameObject ee = Instantiate (ExplosionEffect, mesh.transform.position, Quaternion.identity) as GameObject;
		ee.transform.SetParent (GameManager.GMInstance.root.transform);
		gameObject.SetActive (false);
		GameManager.GMInstance.PlayerDied (gameObject);
		cameraLoc.updatePlayers = true;
		CamShakeManager.PlayShake (0.5f, 1);
    SoundManager.play_death();

  }

	// Use this for initialization
	void Awake ()
	{
		myRBD2D = GetComponentInChildren<Rigidbody2D> ();
		myPCD = GetComponentInChildren<PlayerCollisionDetector> ();
		gManager = FindObjectOfType<GameManager> ();
		SoundM = FindObjectOfType<SoundManager> ();
		glowScales = new Vector3[glows.Length];
		for (int i = 0; i < glows.Length; i++) {
			glowScales [i] = new Vector3 (glows [i].transform.localScale.x, glows [i].transform.localScale.y, glows [i].transform.localScale.z);
		}
		base.Awake ();
		//Debug.Log ("Start called");
		gravity = 1;
		oldscale = scaleMultiplier / rigidbody.position.y;
		if (keys == null) {
			IDictionary<string,KeyCode> defaultKeys = new Dictionary<string,KeyCode> ();
			defaultKeys.Add ("left", KeyCode.A);
			defaultKeys.Add ("right", KeyCode.D);
			defaultKeys.Add ("gravityChange", KeyCode.S);
			defaultKeys.Add ("shoot", KeyCode.W);
			//setKeys (defaultKeys);
		}
	}


	public void PlayerLeftControll ()
	{
		//StartUpdate ();

		if (!boosterEmiter.activeSelf) {
			boosterEmiter.SetActive (true);
		}
        
		Vector2 f = new Vector2 (-movementForce * Mathf.Abs(Mathf.Clamp(Input.GetAxis(keys["movement"]) + Input.GetAxis(keys["movementKey"]), -1, 1)), 0);
		Vector2 sc = new Vector2 (Time.fixedDeltaTime, Time.fixedDeltaTime);
		f.Scale (sc);
		rigidbody.AddForce (f);
	}

	public void PlayerRightControll ()
	{
		if (!boosterEmiter.activeSelf) {
			boosterEmiter.SetActive (true);
		}

        Vector2 f = new Vector2 (movementForce * Mathf.Abs(Mathf.Clamp(Input.GetAxis(keys["movement"]) + Input.GetAxis(keys["movementKey"]), -1, 1)), 0);
		Vector2 sc = new Vector2 (Time.fixedDeltaTime, Time.fixedDeltaTime);
		f.Scale (sc);
		rigidbody.AddForce (f);
	}

	public void PlayerStopMovingControll ()
	{
		if (boosterEmiter.activeSelf) {
			boosterEmiter.SetActive (false);
		}
		rigidbody.velocity = new Vector2 (rigidbody.velocity.x * descelerationRate, rigidbody.velocity.y);
	}

	public void PlayerGravityShiftControll ()
	{
		if (jumpCharge > 0) {
			lastGravityChange = Time.fixedTime;
			gravity = -gravity;

      if (gravity < 0)
        SoundManager.play_antigrav();
      else
        SoundManager.play_grav();

      jumpCharge = 0;
			/*if ((lastGravityChange + gravityChangeRate) < Time.fixedTime)
            {
                lastGravityChange = Time.fixedTime;
                gravity = -gravity;
                SoundM.PlayJumpClip();
                jumpCharge = 0;
            }*/
		}

	}

  public void PlayerShootControll()
  {
    if (shots < shootCharges)
    {
      if ((lastShoot + fireRate) < Time.fixedTime)
      {
        lastShoot = Time.fixedTime;
        GameObject shotInstance;
        if (piercingShot)
        {
          piercingShot = false;
          piercingShotSprite.SetActive(false);
          shotInstance = MonoBehaviour.Instantiate(piercingShotPrefab, physics.transform.position + new Vector3(0.0f, -gravity * shotOffset, 0.0f), new Quaternion()) as GameObject;
          SoundManager.play_pu_piercing_fire();
        }
        else
        {
          shotInstance = MonoBehaviour.Instantiate(shotPrefab, physics.transform.position + new Vector3(0.0f, -gravity * shotOffset, 0.0f), new Quaternion()) as GameObject;
        }
        Vector2 shotVel = new Vector2(0.0f, -gravity);
        shotInstance.GetComponent<ShotController>().setVelocity(shotVel);
        SoundManager.play_normal_shoot();
        shots = shots + 1;
      }
    }

        
		/*if (isGrounded && (lastShoot + fireRate) < Time.fixedTime) {
			lastShoot = Time.fixedTime;
			GameObject shotInstance = MonoBehaviour.Instantiate (shotPrefab, physics.transform.position + new Vector3 (0.0f, -gravity * shotOffset, 0.0f), new Quaternion ()) as GameObject;
			Vector2 shotVel = new Vector2 (0.0f, -gravity);
			shotInstance.GetComponent<ShotController> ().setVelocity (shotVel);
			SoundM.PlayShotClip ();
		}*/
  }



	void FixedUpdate ()
	{
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
        
		for (int i = 0; i < glows.Length; i++) {
			//Debug.Log(i);
			if (i < (shootCharges - shots)) {

                
				glows [i].SetActive (true);
				/*Vector3 tmp = new Vector3((glowScales[i] * interpolateT).x, (glowScales[i] * interpolateT).y, (glowScales[i] * interpolateT).z);
                glows[i].transform.localScale = new Vector3(tmp.x, tmp.y, tmp.z);
                if (interpolateT < 1)
                {
                    interpolateT += 0.1f;
                }*/
			} else {
				glows [i].SetActive (false);
				/*
                Vector3 tmp = new Vector3((glowScales[i]*interpolateT).x, (glowScales[i] * interpolateT).y, (glowScales[i] * interpolateT).z);
                glows[i].transform.localScale = new Vector3(tmp.x, tmp.y, tmp.z);
                if (interpolateT > 0)
                {
                    interpolateT -= 0.1f;
                }*/
			}
		}

		Collider2D[] colliders = Physics2D.OverlapCircleAll (physics.transform.position, 0.6f);
		if (!isGrounded || jumpCharge == 0) {
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject.tag == "Segment") {
					isGrounded = true;
					jumpCharge = 1;
					//Debug.Log ("Grounded");
				}
			}
		}

		StartUpdate ();
		myPCD.RecordSpeedNow();
        //if (!MultiBool) {
        if (shots == 0)
        {
            lastRecharge = Time.fixedTime;
        }
        else if(shots>0)
        {
            if ((lastRecharge + rechargeRate) < Time.fixedTime)
            {
                lastRecharge = Time.fixedTime;
                shots--;
            }
        }
        
        if (Input.GetAxis(keys["movement"]) > 0 || Input.GetAxis(keys["movementKey"]) > 0)
        {
            leftPressed = false;
            rightPressed = true;
            
        }
        else if (Input.GetAxis(keys["movement"]) < 0 || Input.GetAxis(keys["movementKey"]) < 0)
        {
            leftPressed = true;
            rightPressed = false;
        }
        else
        {
            leftPressed = false;
            rightPressed = false;
        }
            /*if (Input.GetKey(keys["left"]))
            {
                leftPressed = true;
            }
            else
            {
                leftPressed = false;
            }

            if (Input.GetKey(keys["right"]))
            {
                rightPressed = true;
            }
            else
            {
                rightPressed = false;
            }*/

            if (Input.GetButtonDown(keys["gravityChange"]))
        //if(Input.GetButtonDown("P1Jump"))
        {
            jumpPressed = true;
        }
        else
        {
            jumpPressed = false;
        }


        if (Input.GetButton(keys["shoot"]))
        {
            firePressed = true;
        }
        else
        {
            firePressed = false;
        }

        if (leftPressed)
        {
            PlayerLeftControll();
        }
        else if (rightPressed)
        {
            PlayerRightControll();
        }
        else
        {
            PlayerStopMovingControll();
        }

		if (jumpPressed)
        {
			PlayerGravityShiftControll();
		}

		if (firePressed) {
			PlayerShootControll ();
		}
		//}
		Vector2 grav = new Vector2 (0f, gravityForce * gravity);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		rigidbody.AddForce (grav);

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
	public void EnableShield ()
	{
		hasShield = true;
		StartCoroutine (ShieldScaleIn ());

    SoundManager.play_pu_shield_pickup();
  }

  IEnumerator ShieldScaleIn ()
	{
		// enable and scale down shield
		shieldSprite.SetActive (true);
		shieldSprite.transform.localScale = new Vector3 (.0f, .0f, .0f);

		float elapsed = 0.0f;
		// animation duration
		float duration = .5f;
		Vector3 newScale;

		// increase scale for duration
		while (elapsed < duration) {
			elapsed += Time.deltaTime;

			float scale = Mathf.SmoothStep (.0f, 1.0f, elapsed / duration);
			newScale = new Vector3 (scale, scale, .0f);
			shieldSprite.transform.localScale = newScale;

			yield return null;
		}

		// set scale to 1
		newScale = new Vector3 (1.0f, 1.0f, .0f);
		shieldSprite.transform.localScale = newScale;
	}

	// disable shield
	public void DisableShield ()
	{
		hasShield = false;
		shieldSprite.SetActive (false);

    SoundManager.play_pu_shield_destroyed();
  }
}

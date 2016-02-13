using UnityEngine;
using System.Collections;

public class PlayerCollisionDetector : MonoBehaviour
{
	private Vector2 prevVelocity;
	public Vector2 PrevVelocity
	{
		get
		{
			return prevVelocity;
		}
		set
		{
			prevVelocity = value;
		}
	}
	LocalPlayerController MyLCP;
	Rigidbody2D rigidBody;
	public GameObject bumpEffect;

  public GameObject shieldExplosion;

	private static CamShakeManager CameShakeM;
	private static SoundManager SoundM;
	private float MaxPlayerSpeed;

	public Transform meshTransform;

	private float BumpAwayMultiplyer = 2.0f;
	private float SelfBumpMultiplyer = 1.0f;
	private float YBumpForce = 5.0f;

	public void RecordSpeedNow(){
		prevVelocity = rigidBody.velocity;
	}

	void Awake()
	{
		SoundM = FindObjectOfType<SoundManager> ();
		CameShakeM = FindObjectOfType<CamShakeManager> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		MyLCP = GetComponentInParent<LocalPlayerController> ();
		MaxPlayerSpeed = MyLCP.WidthMultiplier * MyLCP.MaxHorizontalSpeed;
	}

	void IAmBumpKing(PlayerCollisionDetector HisPCD, Vector2 myVelocity, Collision2D coll){

		HisPCD.BumpAway(myVelocity);
		SelfBump(myVelocity);
		Vector2 contactPoint = coll.contacts[0].point;
		MakeBumpEffect (contactPoint);

	}
	void MakeBumpEffect(Vector2 cPoint)
	{
		SoundManager.PlayBumpClip ();
		CamShakeManager.PlayTestShake (0.2f, 0.2f);
		Vector3 spawnPos = UtilityScript.transformToCartesian(new Vector3(cPoint.x, cPoint.y, 0.0f));
		GameObject bumpMaker = Instantiate(bumpEffect, spawnPos, Quaternion.identity) as GameObject;
		bumpMaker.transform.SetParent(meshTransform);
	}
	void MaxSpeedBump(PlayerCollisionDetector HisPCD, Vector2 myVelocity, Collision2D coll)
	{
		HisPCD.BumpAway(myVelocity*999);
		SelfBump(myVelocity*999);
		Vector2 contactPoint = coll.contacts[0].point;
		MakeBumpEffect (contactPoint);
	}
	// Use this for initialization
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			PlayerCollisionDetector HisPCD = coll.gameObject.GetComponent<PlayerCollisionDetector>();
			Vector2 myVelocity = prevVelocity;
			if(HisPCD != null){
				Vector2 hisVelocity = HisPCD.PrevVelocity;
				Rigidbody2D hisRbd = coll.rigidbody;
					//Debug.Log("his velocity "+hisVelocity + " my velocity "+ myVelocity);
				if(myVelocity.magnitude >= hisVelocity.magnitude){
					//Debug.Log("My speed was: "+myVelocity.magnitude);
					if(myVelocity.magnitude>=MaxPlayerSpeed){
						if(hisVelocity.magnitude>=MaxPlayerSpeed)
						{
							MaxSpeedBump(HisPCD, myVelocity, coll);
							Debug.Log ("Max Speed Collision");
						}
						else
						{
							MakeBumpEffect (coll.contacts[0].point);
							coll.transform.root.gameObject.GetComponent<LocalPlayerController>().DestroyObject();

							Debug.Log("P1 At Max speed only");
						}
					} else{
						IAmBumpKing(HisPCD, myVelocity, coll);
					}

					//hisRbd.ad
				}
			}
		}
	}
	public void SelfBump(Vector2 SelfBumpForce){
		Vector2 yForce = new Vector2 (0.0f, YBumpForce*(-MyLCP.gravity));
		rigidBody.AddForce(((-SelfBumpForce+yForce)*SelfBumpMultiplyer), ForceMode2D.Impulse);
	}
	public void BumpAway(Vector2 BumpForce){
		Vector2 yForce = new Vector2 (0.0f, YBumpForce*(-MyLCP.gravity));
		rigidBody.AddForce (((BumpForce+yForce) * BumpAwayMultiplyer), ForceMode2D.Impulse);
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Shot")
    {
			ShotDestroyerScript SDS = other.gameObject.GetComponent<ShotDestroyerScript> ();
			if (!SDS.IsUsed)
      {
        if (MyLCP.hasShield)
        {
          LocalPlayerController LPC = gameObject.GetComponentInParent<LocalPlayerController>();
          Instantiate(shieldExplosion, LPC.mesh.transform.position, new Quaternion());
          LPC.DisableShield();
          Destroy(other.transform.root.gameObject);
        }
        else
        {
          SDS.IsUsed = true;
          Debug.Log("PLAYER HIT");
		  MyLCP.DestroyObject();

        }
				//explosionInstance.transform.SetParent(transform);
			}
		}
    else if (other.gameObject.tag == "Boundary")
    {
			Debug.Log ("PLAYER BOUNDARY");
			MyLCP.DestroyObject();
		}

	}
}

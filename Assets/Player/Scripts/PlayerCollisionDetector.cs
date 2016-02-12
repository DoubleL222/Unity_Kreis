using UnityEngine;
using System.Collections;

public class PlayerCollisionDetector : MonoBehaviour
{
	Vector2 prevVelocity;
	LocalPlayerController MyLCP;
	Rigidbody2D rigidBody;
	public GameObject bumpEffect;

  public GameObject shieldExplosion;

	private static CamShakeManager CameShakeM;
	private static SoundManager SoundM;

	public Transform meshTransform;

	private float BumpAwayMultiplyer = 5.0f;
	private float SelfBumpMultiplyer = 2.0f;
	private float YBumpForce = 5.0f;


	void Awake(){
		SoundM = FindObjectOfType<SoundManager> ();
		CameShakeM = FindObjectOfType<CamShakeManager> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		MyLCP = GetComponentInParent<LocalPlayerController> ();
	}
	void FixedUpdate(){
		prevVelocity = rigidBody.velocity;
	}
	public Vector2 getPrevVelocity(){
		return prevVelocity;
	}

	void IAmBumpKing(PlayerCollisionDetector HisPCD, Vector2 myVelocity, Collision2D coll){
		SoundManager.PlayBumpClip ();
		CamShakeManager.PlayTestShake (0.2f, 0.2f);
		HisPCD.BumpAway(myVelocity);
		SelfBump(myVelocity);
		Vector2 contactPoint = coll.contacts[0].point;
		Vector3 spawnPos = UtilityScript.transformToCartesian(new Vector3(contactPoint.x, contactPoint.y, 0.0f));
		GameObject bumpMaker = Instantiate(bumpEffect, spawnPos, Quaternion.identity) as GameObject;
		bumpMaker.transform.SetParent(meshTransform);
	}
	// Use this for initialization
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			PlayerCollisionDetector HisPCD = coll.gameObject.GetComponent<PlayerCollisionDetector>();
			Vector2 myVelocity = prevVelocity;
			if(HisPCD != null){
				Vector2 hisVelocity = HisPCD.getPrevVelocity();
				Rigidbody2D hisRbd = coll.rigidbody;
					//Debug.Log("his velocity "+hisVelocity + " my velocity "+ myVelocity);
				if(myVelocity.magnitude > hisVelocity.magnitude){
					Debug.Log("My speed was: "+myVelocity.magnitude);
					IAmBumpKing(HisPCD, myVelocity, coll);
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

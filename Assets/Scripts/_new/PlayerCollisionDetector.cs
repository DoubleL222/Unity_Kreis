using UnityEngine;
using System.Collections;

public class PlayerCollisionDetector : MonoBehaviour {
	Vector2 prevVelocity;
	LocalPlayerController MyLCP;
	Rigidbody2D rigidBody;
	public GameObject bumpEffect;
	public GameObject ExplosionEffect;

	public Transform meshTransform;

	private float BumpAwayMultiplyer = 2.0f;
	private float SelfBumpMultiplyer = 1.0f;
	private float YBumpForce = 5.0f;


	void Start(){
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		MyLCP = GetComponentInParent<LocalPlayerController> ();
		if (MyLCP != null) {
			Debug.Log ("local player controller found");
		}
	}
	void FixedUpdate(){
		prevVelocity = rigidBody.velocity;
	}
	public Vector2 getPrevVelocity(){
		return prevVelocity;
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
					Debug.Log ("relative Velocity " + coll.relativeVelocity);
					Debug.Log("pushing him for" +myVelocity*BumpAwayMultiplyer);
					HisPCD.BumpAway(myVelocity);
					SelfBump(myVelocity);
					Vector2 contactPoint = coll.contacts[0].point;
					Vector3 spawnPos = transformToPolar(new Vector3(contactPoint.x, contactPoint.y, 0.0f));

					GameObject bumpMaker = Instantiate(bumpEffect, spawnPos, Quaternion.identity) as GameObject;
					bumpMaker.transform.SetParent(meshTransform);
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
		if (other.gameObject.tag == "Shot") {
			ShotDestroyerScript SDS = other.gameObject.GetComponent<ShotDestroyerScript> ();
			if(!SDS.IsUsed){
				SDS.IsUsed = true;
				Destroy(other.transform.root.gameObject);
				GameObject explosionInstance = Instantiate(ExplosionEffect, meshTransform.position, Quaternion.identity) as GameObject;
				Destroy(transform.root.gameObject);
				Debug.Log ("PLAYER HIT");
			//explosionInstance.transform.SetParent(transform);
			}

		}
	}
	Vector3 transformToPolar(Vector3 pos){
		
		float angle = pos.x / 10.0f;
		float distance = pos.y;
		
		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		
		return new Vector3 (mx, my, 0.0f);
	}
}

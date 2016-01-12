using UnityEngine;
using System.Collections;

public class PlayerCollisionDetector : MonoBehaviour {
	Vector2 prevVelocity;
	LocalPlayerController MyLCP;
	Rigidbody2D rigidBody;

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
}

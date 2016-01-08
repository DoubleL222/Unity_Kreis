using UnityEngine;
using System.Collections;

public class PlayerCollisionDetector : MonoBehaviour {
	Rigidbody2D rigidBody;
	void Start(){
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
	}
	// Use this for initialization
	void OnCollisionEnter2D(Collision2D coll)
	{


		if (coll.gameObject.tag == "Player") {
			Debug.Log (" coll. rigidbody.velocity" + coll.rigidbody.velocity);
			//Debug.Log ("relative Velocity " + coll.relativeVelocity);
			Rigidbody2D myRbd = coll.gameObject.GetComponent<Rigidbody2D>();
			if(myRbd != null){
				Vector2 myVelocity = myRbd.velocity;
				Rigidbody2D hisRbd = coll.rigidbody;
				if(hisRbd!=null){
				Vector2 hisVelocity = hisRbd.velocity;
					//Debug.Log("his velocity "+hisVelocity + " my velocity "+ myVelocity);
					if(myVelocity.magnitude > hisVelocity.magnitude){
						//Debug.Log("pushing him for" +coll.relativeVelocity);
						hisRbd.AddForce(coll.relativeVelocity);
					}
				}
			}
		}
	}
}

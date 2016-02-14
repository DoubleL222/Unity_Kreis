using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
	// shield
	public GameObject shieldExplosion;

	public Transform root;

	void Start ()
	{
		StartUpdate ();
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (gameObject.tag == "Shield") {
			if (collider.gameObject.tag == "Player") {
				LocalPlayerController LPC = collider.gameObject.GetComponentInParent<LocalPlayerController> ();
				// player already has shield?
				if (!LPC.hasShield) {
					LPC.EnableShield ();
				}
				Destroy (root.gameObject);
			} else if (collider.gameObject.tag == "Shot") {
				GameObject se = Instantiate (shieldExplosion, mesh.transform.position, new Quaternion ()) as GameObject;
				se.transform.SetParent (GameManager.GMInstance.root.transform);
				ShotDestroyerScript sds = collider.gameObject.GetComponent<ShotDestroyerScript>();
				Destroy (sds.root.gameObject);
				Destroy (root.gameObject);
			}
		}
	}
}

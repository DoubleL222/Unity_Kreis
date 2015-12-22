using UnityEngine;
using System.Collections;

public class LocalPlayerController : MonoBehaviour {

	public GameObject mesh;
	public GameObject physics;

	Rigidbody rigidbody;

	private int gravity;

	private float lastGravityChangeTime;
	private static float gravityChangeDelay = 0.3f;

	// Use this for initialization
	void Start () {
		rigidbody = physics.GetComponent<Rigidbody> ();
		gravity = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey ("a")) {
			rigidbody.AddForce (-10f, 0, 0);
		}
		if (Input.GetKey ("d")) {
			rigidbody.AddForce (10f, 0, 0);
		}
		if (Input.GetKey ("w") && lastGravityChangeTime + gravityChangeDelay < Time.time) {
			lastGravityChangeTime = Time.time;
			gravity = -gravity;
		}

		Vector3 pos = physics.transform.position;

		float angle = pos.x / pos.y * Mathf.PI;
		float distance = pos.y;

		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);

		mesh.transform.position = new Vector3(mx, my, 0);
		mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);


		Vector3 grav = new Vector3 (-rigidbody.position.x, -rigidbody.position.y, 0);
		grav.Normalize ();
		float scale = 1f/grav.y * Physics.gravity.y * gravity * Time.deltaTime;
		grav.Scale (new Vector3(scale, scale, scale));
	
		rigidbody.velocity += grav;

		//mesh.transform.position = new Vector3 ();
	}
}

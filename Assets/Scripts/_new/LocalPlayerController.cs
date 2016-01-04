using UnityEngine;
using System.Collections;

public class LocalPlayerController : MonoBehaviour {

	public GameObject mesh;
	public GameObject physics;

	Rigidbody rigidbody;

	private int gravity;

	private float lastGravityChangeTime;
	private static float gravityChangeDelay = 0.3f;

	private Vector3 oldVelocity;
	private float oldscale;
	private static float scaleMultiplier = Mathf.PI*2;

	private static float movementForce = 400f;
	private static float maxHorizontalSpeed = 2f;
	private static float maxVerticalSpeed = 5f;
	private static float gravityForce = 30f;

	// Use this for initialization
	void Start () {
		rigidbody = physics.GetComponent<Rigidbody> ();
		gravity = 1;
		oldscale = scaleMultiplier/rigidbody.position.y;
	}
	
	// FixedUpdate is called before physics calculations
	void FixedUpdate () {

		if (physics.transform.position.x > Mathf.PI) {
			Vector3 tmp = physics.transform.position;
			tmp.x -= 2 * Mathf.PI;
			physics.transform.position = tmp;
		} else if (physics.transform.position.x < -Mathf.PI) {
			Vector3 tmp = physics.transform.position;
			tmp.x += 2 * Mathf.PI;
			physics.transform.position = tmp;
		}


		Vector3 tmpvel = rigidbody.velocity;
		tmpvel -= oldVelocity;
		tmpvel.Scale (new Vector3 (oldscale, oldscale, oldscale));
		oldVelocity += tmpvel;
		oldVelocity.Scale (new Vector3(1f/oldscale, 1f, 1f/oldscale));
		oldVelocity.x = Mathf.Clamp (oldVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
		oldVelocity.y = Mathf.Clamp (oldVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);
		rigidbody.velocity = oldVelocity;
		oldscale = scaleMultiplier / rigidbody.position.y;



		if (Input.GetKey ("a")) {
			Vector3 f = new Vector3 (-movementForce, 0, 0);
			Vector3 sc = new Vector3 (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey ("d")) {
			Vector3 f = new Vector3 (movementForce, 0, 0);
			Vector3 sc = new Vector3 (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
			f.Scale (sc);
			rigidbody.AddForce (f);
		}
		if (Input.GetKey ("w") && lastGravityChangeTime + gravityChangeDelay < Time.time) {
			lastGravityChangeTime = Time.time;
			gravity = -gravity;
		}

		Vector3 pos = physics.transform.position;

		float angle = pos.x;
		float distance = pos.y;

		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);

		mesh.transform.position = new Vector3(mx, my, 0);
		mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);

		Vector3 grav = new Vector3(0f, gravityForce * gravity, 0f);
		//Debug.Log("Grav: " + grav);
		//rigidbody.velocity += grav;
		rigidbody.AddForce(grav);

		oldVelocity = rigidbody.velocity;
		Debug.Log ("Actual velocity: " + rigidbody.velocity);
		oldVelocity.Scale(new Vector3(oldscale, 1f, oldscale));

		rigidbody.velocity = oldVelocity;
		Debug.Log ("Applied velocity: " + rigidbody.velocity);

		Vector3 tmpvec = rigidbody.velocity;
		tmpvec.Scale (new Vector3(rigidbody.position.y, 1f, rigidbody.position.y));
		Debug.Log("Velocity given position: " + tmpvec);

		float oldyscale = rigidbody.transform.localScale.y;
		rigidbody.transform.localScale = new Vector3 (oldscale, oldyscale, oldscale);
	}
}

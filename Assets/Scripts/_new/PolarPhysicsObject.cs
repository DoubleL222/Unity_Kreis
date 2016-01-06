using UnityEngine;
using System.Collections;

public class PolarPhysicsObject : MonoBehaviour{
	protected static readonly float widthMultiplier = 10f;
	protected static float scaleMultiplier = Mathf.PI*2;
	protected static float maxHorizontalSpeed = 2f;
	protected static float maxVerticalSpeed = 5f;

	protected Vector2 oldVelocity;
	protected float oldscale;

	public GameObject mesh;
	public GameObject physics;
	public GameObject physics_parent;
	public Rigidbody2D rigidbody;

	protected void Awake(){
		Vector3 tmp = physics_parent.transform.localScale;
		tmp.Scale (new Vector3(widthMultiplier, 1, 1));
		physics_parent.transform.localScale = tmp;
		oldscale = 1;
		oldVelocity = new Vector2 (0f, 0f);
		Debug.Log ("Start called!");
	}

	protected void StartUpdate(){
		if (physics != null) {
			if (physics.transform.position.x > Mathf.PI * widthMultiplier) {
				Vector3 tmp = physics.transform.position;
				tmp.x -= 2 * Mathf.PI * widthMultiplier;
				physics.transform.position = tmp;
			} else if (physics.transform.position.x < -Mathf.PI * widthMultiplier) {
				Vector3 tmp = physics.transform.position;
				tmp.x += 2 * Mathf.PI * widthMultiplier;
				physics.transform.position = tmp;
			}
		}
		if (rigidbody != null) {
			Vector2 tmpvel = rigidbody.velocity;
			tmpvel -= oldVelocity;
			tmpvel.Scale (new Vector2 (oldscale, oldscale));
			oldVelocity += tmpvel;
			oldVelocity.Scale (new Vector2 (1f / oldscale, 1f));
			oldVelocity.x = Mathf.Clamp (oldVelocity.x, -maxHorizontalSpeed * widthMultiplier, maxHorizontalSpeed * widthMultiplier);
			oldVelocity.y = Mathf.Clamp (oldVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);
			rigidbody.velocity = oldVelocity;
			oldscale = scaleMultiplier / rigidbody.position.y;
		}
	}


	protected void EndUpdate(){
		if (physics != null && mesh != null) {
			Vector3 pos = physics.transform.position;
	
			float angle = pos.x / widthMultiplier;
			float distance = pos.y;

			float mx = distance * Mathf.Cos (angle);
			float my = distance * Mathf.Sin (angle);

			mesh.transform.position = new Vector3 (mx, my, 0);
			mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);
		}
		if (rigidbody != null) {
			oldVelocity = rigidbody.velocity;
			//Debug.Log ("Actual velocity: " + rigidbody.velocity);
			oldVelocity.Scale (new Vector2 (oldscale, 1f));

			rigidbody.velocity = oldVelocity;
			//Debug.Log ("Applied velocity: " + rigidbody.velocity);

			//Vector3 tmpvec = rigidbody.velocity;
			//tmpvec.Scale (new Vector2 (rigidbody.position.y, 1f));
			//Debug.Log ("Velocity given position: " + tmpvec);

			float oldyscale = rigidbody.transform.localScale.y;
			rigidbody.transform.localScale = new Vector3 (oldscale, oldyscale, oldscale);
		}
	}
}

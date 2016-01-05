using UnityEngine;
using System.Collections;

public class PolarPhysicsObject : MonoBehaviour{

	protected Vector3 oldVelocity;
	protected float oldscale;
	protected static float scaleMultiplier = Mathf.PI*2;
	protected static float maxHorizontalSpeed = 2f;
	protected static float maxVerticalSpeed = 5f;



	protected void StartUpdate(GameObject mesh, GameObject physics, Rigidbody rigidbody){
		if (physics != null) {
			if (physics.transform.position.x > Mathf.PI) {
				Vector3 tmp = physics.transform.position;
				tmp.x -= 2 * Mathf.PI;
				physics.transform.position = tmp;
			} else if (physics.transform.position.x < -Mathf.PI) {
				Vector3 tmp = physics.transform.position;
				tmp.x += 2 * Mathf.PI;
				physics.transform.position = tmp;
			}
		}
		if (rigidbody != null) {
			Vector3 tmpvel = rigidbody.velocity;
			tmpvel -= oldVelocity;
			tmpvel.Scale (new Vector3 (oldscale, oldscale, oldscale));
			oldVelocity += tmpvel;
			oldVelocity.Scale (new Vector3 (1f / oldscale, 1f, 1f / oldscale));
			oldVelocity.x = Mathf.Clamp (oldVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
			oldVelocity.y = Mathf.Clamp (oldVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);
			rigidbody.velocity = oldVelocity;
			oldscale = scaleMultiplier / rigidbody.position.y;
		}
	}


	protected void EndUpdate(GameObject mesh, GameObject physics, Rigidbody rigidbody){
		if (physics != null && mesh != null) {
			Vector3 pos = physics.transform.position;
	
			float angle = pos.x;
			float distance = pos.y;

			float mx = distance * Mathf.Cos (angle);
			float my = distance * Mathf.Sin (angle);

			mesh.transform.position = new Vector3 (mx, my, 0);
			mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);
		}
		if (rigidbody != null) {
			oldVelocity = rigidbody.velocity;
			Debug.Log ("Actual velocity: " + rigidbody.velocity);
			oldVelocity.Scale (new Vector3 (oldscale, 1f, oldscale));

			rigidbody.velocity = oldVelocity;
			Debug.Log ("Applied velocity: " + rigidbody.velocity);

			Vector3 tmpvec = rigidbody.velocity;
			tmpvec.Scale (new Vector3 (rigidbody.position.y, 1f, rigidbody.position.y));
			Debug.Log ("Velocity given position: " + tmpvec);

			float oldyscale = rigidbody.transform.localScale.y;
			rigidbody.transform.localScale = new Vector3 (oldscale, oldyscale, oldscale);
		}
	}
}

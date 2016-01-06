using UnityEngine;
using System.Collections;

public class SegmentController : PolarPhysicsObject{
	private static int count = 0;

	// Use this for initialization
	void Awake() {
		Debug.Log ("Start called");
		base.Awake();
		count++;
		//Debug.Log ("Number of segments: " + count);
	}

	public void SetPosition(Vector2 pos){
		//Debug.Log ("SetPosition called with " + pos);
		float angle = pos.x;
		float distance = pos.y;

		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		//Debug.Log ("Calculated new position at " + mx + ", " + my + " and rotation at " + (angle + Mathf.PI/2f));

		mesh.transform.position = new Vector3(mx, my, 0);
		mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);

		float sc = scaleMultiplier / pos.y;
		physics.transform.position = new Vector2(pos.x, pos.y);
		physics.transform.localScale = new Vector2(sc, sc);	

		//Debug.Log ("Segment pos set to " + mesh.transform.position + " " + physics.transform.position);
	}

	// Update is called once per frame
	void Update () {
		StartUpdate ();


		EndUpdate();
	}
}

using UnityEngine;
using System.Collections;

public class SegmentManager : MonoBehaviour {

	public GameObject mesh;
	public GameObject physics;
	// Use this for initialization
	void Start () {
		
	}

	public void SetPosition(Vector2 pos){
		Debug.Log ("SetPosition called with " + pos);
		float angle = pos.x / pos.y * Mathf.PI;
		float distance = pos.y;

		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		Debug.Log ("Calculated new position at " + mx + ", " + my + " and rotation at " + (angle + Mathf.PI/2f));
		mesh.transform.position = new Vector3(mx, my, 0);
		mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);

		//mesh.transform.rotation.Set (rot.x, rot.y, rot.z, rot.w);
		physics.transform.position = new Vector3(pos.x, pos.y, 0);
		Debug.Log ("Segment pos set to " + mesh.transform.position + " " + physics.transform.position);
	}

	// Update is called once per frame
	void Update () {
	
	}
}

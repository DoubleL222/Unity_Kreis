using UnityEngine;
using System.Collections;

public class PolarStaticObject : MonoBehaviour {
	public GameObject objectMesh;
	protected static readonly float widthMultiplier = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected void StartUpdate () {
		if (objectMesh != null) {
			Vector3 pos = transform.position;
			
			float angle = pos.x / widthMultiplier;
			float distance = pos.y;
			
			float mx = distance * Mathf.Cos (angle);
			float my = distance * Mathf.Sin (angle);
			
			objectMesh.transform.position = new Vector3 (mx, my, 0);
			objectMesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);
		}
	}
}

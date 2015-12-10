using UnityEngine;
using System.Collections;

public class SegmentController : MonoBehaviour {

	public bool destroyed;
	public Mesh brokenMesh;
	// Use this for initialization
	void Start () {
		destroyed = false;
	}
	public void destroySegment(){
		MeshFilter mf = GetComponent<MeshFilter> ();
		mf.mesh = brokenMesh;
		destroyed = true;
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other) {
		Debug.Log ("On trigger ENtered");
		if (other.tag == "Shot") {
			if(destroyed == false){
				Destroy(other.gameObject);
				destroySegment();
			}
		}

	}
}

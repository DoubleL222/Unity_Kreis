/*

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour {



	public float originalRad = 285.0f;
	float rad1, rad2, rad3;
	public List<Lane> lanes;
	// Use this for initialization
	void Start () {
		lanes = new List<Lane> ();
		float r1 = 0.5f;
		float r2 = 1.0f;
		float r3 = 1.7f;
		float r4 = 2.6f;
		Ring core = new Ring (r1);
		core.spawnRing ();
		Ring inner = new Ring (r2);
		inner.spawnRing ();
		Ring middle = new Ring (r3);
		middle.spawnRing ();
		Ring outer = new Ring (r4);
		outer.spawnRing ();
		rad1 = (r2 - r1) / 2.0f;
		Lane l1 = new Lane (core, inner,originalRad*r1+originalRad*rad1 );
		rad2 = (r3 - r2) / 2.0f;
		Lane l2 = new Lane (inner, middle, originalRad*r2+originalRad*rad2);
		rad3 = (r4 - r3) / 2.0f;
		Lane l3 = new Lane (middle, outer, originalRad*r3+originalRad*rad3);
		lanes.Add (l1);
		lanes.Add (l2);
		lanes.Add (l3);

	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnDrawGizmos(){
		foreach(Lane l in lanes){
			Gizmos.DrawWireSphere(Vector3.zero, l.radius);
		}
	}
}

*/

using UnityEngine;
using System.Collections;

public class Lane{
	public Ring innerRing;
	public Ring outerRing;
	public float radius;
	// Use this for initialization

	public Lane(Ring ir, Ring or, float r){
		innerRing = ir;
		outerRing = or;
		radius = r;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Gizmos.DrawSphere (Vector3.zero, radius);
	}
}

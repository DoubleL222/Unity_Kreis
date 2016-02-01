using UnityEngine;
using System.Collections;

public class UtilityScript : MonoBehaviour {

	// Use this for initialization
	public static Vector3 transformToPolar(Vector3 pos){
		
		float angle = pos.x / 10.0f;
		float distance = pos.y;
		
		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		
		return new Vector3 (mx, my, 0.0f);
	}
}

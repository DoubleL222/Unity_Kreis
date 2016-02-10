using UnityEngine;
using System.Collections;

public class UtilityScript : MonoBehaviour {
	
	public static Vector3 transformToCartesian(Vector3 pos){
		float angle = pos.x / PolarPhysicsObject.widthMultiplier;
		float distance = pos.y;
		
		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		
		return new Vector3 (mx, my, 0.0f);
	}

	public static bool isVector2NanOrInf(Vector2 checkVector){
		if (float.IsNaN (checkVector.x) || float.IsNaN (checkVector.y) || float.IsInfinity(checkVector.x) || float.IsInfinity(checkVector.y)) { 
			return true;
		}
		return false;
	}
	public static bool isVector3Nan(Vector3 checkVector){
		if (float.IsNaN (checkVector.x) || float.IsNaN (checkVector.y) || float.IsNaN (checkVector.z)) { 
			return true;
		}
		return false;
	}
}

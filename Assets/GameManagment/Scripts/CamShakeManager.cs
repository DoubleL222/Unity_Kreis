using UnityEngine;
using System.Collections;

public class CamShakeManager : MonoBehaviour
{
	public static float testShakeDuration;
	public static float testShakeStrength;

	private static Vector3 originalCamPos;
	private static float originalCamSize;

	public static CamShakeManager Instance;
	public Camera shakeCam;
	void Awake()
	{
		Instance = this;
		//Camera cam = FindObjectOfType<Camera>();
		originalCamSize = shakeCam.orthographicSize;
		originalCamPos = shakeCam.transform.position;
	}

	void Update()
	{
        originalCamSize = cameraLoc.size;
        originalCamPos = cameraLoc.center;
    }

	public static void PlayShake(float duration, float strength)
	{
		Debug.Log ("CAMSHAKE CALLED");
		Instance.StopAllCoroutines ();
		Instance.shakeCam.orthographicSize = originalCamSize;
		Instance.shakeCam.transform.position = originalCamPos;
		Instance.StartCoroutine(Instance.Shake(duration, strength));
	}


	IEnumerator Shake(float duration, float strength)
	{
		float elapsed = 0.0f;
		float i = 0;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= strength * damper;
			y *= strength * damper;

			shakeCam.orthographicSize = originalCamSize - strength * damper;
			shakeCam.transform.position = new Vector3(cameraLoc.center.x + x, cameraLoc.center.y + y, originalCamPos.z);

			yield return null;
		}

		shakeCam.orthographicSize = originalCamSize;
		shakeCam.transform.position = originalCamPos;
	}
}
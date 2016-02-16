using UnityEngine;
using System.Collections;

public class CamShakeManager : MonoBehaviour
{
	public static float testShakeDuration;
	public static float testShakeStrength;

	private static Vector3 originalCamPos;
	private static float originalCamSize;

	public static CamShakeManager Instance;
	void Awake()
	{
		Instance = this;
		Camera cam = FindObjectOfType<Camera>();
		originalCamSize = cam.orthographicSize;
		originalCamPos = cam.transform.position;
	}

	void Update()
	{
        originalCamSize = cameraLoc.size;
        originalCamPos = cameraLoc.center;
    }

	public static void PlayShake(float duration, float strength)
	{
		Instance.StopAllCoroutines ();
		Camera.main.orthographicSize = originalCamSize;
		Camera.main.transform.position = originalCamPos;
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

			Camera.main.orthographicSize = originalCamSize - strength * damper;
			Camera.main.transform.position = new Vector3(cameraLoc.center.x + x, cameraLoc.center.y + y, originalCamPos.z);

			yield return null;
		}

		Camera.main.orthographicSize = originalCamSize;
		Camera.main.transform.position = originalCamPos;
	}
}
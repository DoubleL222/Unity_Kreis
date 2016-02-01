using UnityEngine;
using System.Collections;

public class CamShakeManager : MonoBehaviour
{
	public float testShakeDuration;
	public float testShakeStrength;

	private Vector3 originalCamPos;
	private float originalCamSize;

	void Awake()
	{
		Camera cam = GetComponent<Camera>();
		originalCamSize = cam.orthographicSize;
		originalCamPos = cam.transform.position;
	}

	void Update()
	{
        originalCamSize = cameraLoc.size;
        originalCamPos = cameraLoc.center;
    }

	public void PlayTestShake(float duration, float strength)
	{
		StopAllCoroutines();
		Camera.main.orthographicSize = originalCamSize;
		Camera.main.transform.position = originalCamPos;
		StartCoroutine(TestShake(duration, strength));
	}


	IEnumerator TestShake(float duration, float strength)
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
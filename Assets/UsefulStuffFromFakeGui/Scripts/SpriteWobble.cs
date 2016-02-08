using UnityEngine;
using System.Collections;

public class SpriteWobble : MonoBehaviour
{
  public enum ResizeState
  {
    scaleUp,
    scaleDown
  }

  public float minSize = 0.0f;
  public float maxSize = 1.0f;
  public float duration = 2.0f;
  public bool randomStart = false;
  public ResizeState spriteState = ResizeState.scaleDown;

  private float timer = .0f;
  private Vector3 originalScale;
  private Vector3 newScale;

  // Use this for initialization
  void Start ()
  {
    originalScale = transform.localScale;

    // random start
    if (randomStart)
    {
      timer = Random.value * duration;
      float scale = Mathf.SmoothStep(maxSize, minSize, timer / duration);
      newScale = new Vector3(originalScale.x * scale, originalScale.y * scale, .0f);
      transform.localScale = newScale;

      if (Random.value > 0.5)
        spriteState = ResizeState.scaleDown;
      else
        spriteState = ResizeState.scaleUp;
    }
  }
	
	// Update is called once per frame
	void Update ()
  {
    timer += Time.deltaTime;

    if (spriteState == ResizeState.scaleDown)
    {
      float scale = Mathf.SmoothStep(maxSize, minSize, timer / duration);
      newScale = new Vector3(originalScale.x * scale, originalScale.y * scale, .0f);
      transform.localScale = newScale;

      if (timer > duration)
      {
        spriteState = ResizeState.scaleUp;
        timer = .0f;
      }
    }
    else
    {
      float scale = Mathf.SmoothStep(minSize, maxSize, timer / duration);
      newScale = new Vector3(originalScale.x * scale, originalScale.y * scale, .0f);
      transform.localScale = newScale;

      if (timer > duration)
      {
        spriteState = ResizeState.scaleDown;
        timer = .0f;
      }
    }
	}
}

using UnityEngine;
using System.Collections;

public class ScaleInAndWobble : MonoBehaviour
{
  public enum ResizeState
  {
    scaleIn,
    scaleUp,
    scaleDown
  }

  public float minSize = 0.0f;
  public float maxSize = 1.0f;
  public float scaleInDuration = 0.3f;
  public float wobbleDuration = 2.0f;

  private ResizeState spriteState = ResizeState.scaleIn;
  private float timer = .0f;
  private Vector3 originalScale;
  private Vector3 newScale;

  // Use this for initialization
  void Start()
  {
    originalScale = transform.localScale;

    transform.localScale = new Vector3();
  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;

    if (spriteState == ResizeState.scaleIn)
    {
      float scale = Mathf.SmoothStep(.0f, maxSize, timer / scaleInDuration);
      newScale = new Vector3(originalScale.x * scale, originalScale.y * scale, .0f);
      transform.localScale = newScale;

      if (timer > scaleInDuration)
      {
        spriteState = ResizeState.scaleDown;
        timer = .0f;
      }
    }
    else if (spriteState == ResizeState.scaleDown)
    {
      float scale = Mathf.SmoothStep(maxSize, minSize, timer / wobbleDuration);
      newScale = new Vector3(originalScale.x * scale, originalScale.y * scale, .0f);
      transform.localScale = newScale;

      if (timer > wobbleDuration)
      {
        spriteState = ResizeState.scaleUp;
        timer = .0f;
      }
    }
    else
    {
      float scale = Mathf.SmoothStep(minSize, maxSize, timer / wobbleDuration);
      newScale = new Vector3(originalScale.x * scale, originalScale.y * scale, .0f);
      transform.localScale = newScale;

      if (timer > wobbleDuration)
      {
        spriteState = ResizeState.scaleDown;
        timer = .0f;
      }
    }
  }
}

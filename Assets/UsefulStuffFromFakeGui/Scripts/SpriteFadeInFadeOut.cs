using UnityEngine;
using System.Collections;

public class SpriteFadeInFadeOut : MonoBehaviour
{
  public enum FadeState
  {
    fadeIn,
    fadeOut
  }

  public float minAlpha = 0.0f;
  public float maxAlpha = 1f;
  public float duration = 2.0f;
  public bool randomStart = false;
  public FadeState spriteState = FadeState.fadeOut;

  private float timer = .0f;
  private SpriteRenderer sr;
  
	// Use this for initialization
	void Start ()
  {
    sr = gameObject.GetComponent<SpriteRenderer>(); 

    // random start
    if (randomStart)
    {
      timer = Random.value * duration;
      float alpha = Mathf.SmoothStep(minAlpha, maxAlpha, timer / duration);
      Color currentColor = sr.color;
      sr.color = new Color(currentColor.r, currentColor.b, currentColor.g, alpha);

      if (Random.value > 0.5)
        spriteState = FadeState.fadeOut;
      else
        spriteState = FadeState.fadeIn;
    }
	}
	
	// Update is called once per frame
	void Update ()
  {
    timer += Time.deltaTime;

    if (spriteState == FadeState.fadeOut)
    {
      float alpha = Mathf.SmoothStep(maxAlpha, minAlpha, timer / duration);
      Color currentColor = sr.color;
      sr.color = new Color(currentColor.r, currentColor.b, currentColor.g, alpha);

      if (timer > duration)
      {
        spriteState = FadeState.fadeIn;
        timer = .0f;
      }
    }
    else
    {
      float alpha = Mathf.SmoothStep(minAlpha, maxAlpha, timer / duration);
      Color currentColor = sr.color;
      sr.color = new Color(currentColor.r, currentColor.b, currentColor.g, alpha);

      if (timer > duration)
      {
        spriteState = FadeState.fadeOut;
        timer = .0f;
      }
    }
	}
}

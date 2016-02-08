using UnityEngine;
using System.Collections;

public class ButtonColorer : MonoBehaviour
{
  public Color colorOver;

  private SpriteRenderer sr;
  private Color originalColor;

 [HideInInspector]
  public bool mouseOver = false;

  void Start()
  {
    sr = gameObject.GetComponent<SpriteRenderer>();
    originalColor = sr.color;
  }

  void OnMouseEnter()
  {
    Color currentColor = sr.color;
    sr.color = new Color(colorOver.r, colorOver.g, colorOver.b, currentColor.a);
    mouseOver = true;
  }

  void OnMouseExit()
  {
    Color currentColor = sr.color;
    sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentColor.a);
    mouseOver = false;
  }
}

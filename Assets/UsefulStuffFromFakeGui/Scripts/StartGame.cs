using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour
{
  ButtonColorer bc;

  void Start()
  {
    bc = GetComponent<ButtonColorer>();
  }

  void OnMouseUp()
  {
    if (bc.mouseOver)
      Application.LoadLevel("GUI");
  }
}

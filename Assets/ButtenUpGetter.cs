using UnityEngine;
using System.Collections;

public class ButtenUpGetter : MonoBehaviour
{
	public InputHandler inputHandler;

	void OnMouseDown()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.jumpPressed = true;
		inputHandler.inputChanged = true;
	}
	
	void OnMouseUp()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.jumpPressed = false;
		inputHandler.inputChanged = true;
	}
}
using UnityEngine;
using System.Collections;

public class ButtonLeftGetter : MonoBehaviour
{
	public InputHandler inputHandler;

	void OnMouseDown()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.leftPressed = true;
		inputHandler.inputChanged = true;
	}
	
	void OnMouseUp()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.leftPressed = false;
		inputHandler.inputChanged = true;
	}
}
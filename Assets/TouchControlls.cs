using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchControlls : MonoBehaviour {
    public Image DebugImage;
	// Use this for initialization
	void Start () {
	
	}
	
    Color mixColor(Color a, Color b)
    {
        return new Color((a.r + b.r) / 2, (a.g + b.g) / 2, (a.b + b.b) / 2);
    }

	// Update is called once per frame
	void Update () {
        DebugImage.color = new Color(255, 255, 255);
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetMouseButton(0))
            {
                //Debug.Log(Input.mousePosition+" "+Screen.width+" "+Screen.height);
                if (Input.mousePosition.x > (float)Screen.width / 2 && Input.mousePosition.y > (float)Screen.height / 2)
                {
                    DebugImage.color = mixColor(DebugImage.color, new Color(0, 255, 0));
                    Debug.Log("shoot");
                }
                else if (Input.mousePosition.x < (float)Screen.width / 2 && Input.mousePosition.y > (float)Screen.height / 2)
                {
                    DebugImage.color = mixColor(DebugImage.color, new Color(255, 0, 0));
                    Debug.Log("change side");
                }
                else if (Input.mousePosition.x < (float)Screen.width / 2 && Input.mousePosition.y < (float)Screen.height / 2)
                {
                    DebugImage.color = mixColor(DebugImage.color, new Color(0, 0, 255));
                    Debug.Log("left");
                }
                else if (Input.mousePosition.x > (float)Screen.width / 2 && Input.mousePosition.y < (float)Screen.height / 2)
                {
                    DebugImage.color = mixColor(DebugImage.color, new Color(255, 0, 255));
                    Debug.Log("right");
                }

            }
            else
            {
                //serviceInput(0);
            }
        }
        else
        {
            
            if (Input.touchCount > 0)
            {
                //Touch[] touches = Input.touches;
                foreach (Touch touch in Input.touches)
                {
                    //DebugImage.color = new Color(255, 255, 0);
                    if (touch.position.x > (float)Screen.width / 2 && touch.position.y > (float)Screen.height / 2)
                    {
                        DebugImage.color = mixColor(DebugImage.color, new Color(0, 255, 0));
                        Debug.Log("shoot");
                    }
                    else if (touch.position.x < (float)Screen.width / 2 && touch.position.y > (float)Screen.height / 2)
                    {
                        DebugImage.color = mixColor(DebugImage.color, new Color(255, 0, 0));
                        Debug.Log("change side");
                    }
                    else if (touch.position.x < (float)Screen.width / 2 && touch.position.y < (float)Screen.height / 2)
                    {
                        DebugImage.color = mixColor(DebugImage.color, new Color(0, 0, 255));
                        Debug.Log("left");
                    }
                    else if (touch.position.x > (float)Screen.width / 2 && touch.position.y < (float)Screen.height / 2)
                    {
                        DebugImage.color = mixColor(DebugImage.color, new Color(255, 0, 255));
                        Debug.Log("right");
                    }
                }

            }
            else
            {
                //serviceInput(0);
            }
        }
    }
}

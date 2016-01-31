using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour
{
  void OnCollisionEnter2D(Collision2D coll)
  {
    if (coll.gameObject.tag == "Player")
    {
      Debug.Log("PLAYER!");
    }
    else if (coll.gameObject.tag == "Shot")
    {
      Debug.Log("SHOT!");
    }
  }
}

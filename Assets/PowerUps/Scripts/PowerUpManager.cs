using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
  public GameObject parentObject;

  // shield
  public GameObject shieldSprite;
  public GameObject shieldCollider;

  void Start()
  {
    StartUpdate();
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (gameObject.tag == "Shield" && collider.gameObject.tag == "Player")
    {
      LocalPlayerController LPC = collider.gameObject.GetComponentInParent<LocalPlayerController>();
      GameObject newShieldSprite = (GameObject)Instantiate(shieldSprite, LPC.mesh.transform.position, new Quaternion());
      newShieldSprite.transform.parent = LPC.mesh.transform;
      GameObject newShieldCollider = (GameObject)Instantiate(shieldCollider, LPC.physics.transform.position, new Quaternion());
      newShieldCollider.transform.parent = LPC.physics.transform;
      Destroy(parentObject);
    }
  }
}

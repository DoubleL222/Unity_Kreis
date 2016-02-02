using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
  // shield
  public GameObject shieldSprite;
  public GameObject shieldCollider;
  public GameObject shieldExplosion;

  void Start()
  {
    StartUpdate();
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (gameObject.tag == "Shield")
    {
      if (collider.gameObject.tag == "Player")
      {
        LocalPlayerController LPC = collider.gameObject.GetComponentInParent<LocalPlayerController>();
        // player already has shield?
        if (!LPC.hasShield)
        {
          GameObject newShieldSprite = (GameObject)Instantiate(shieldSprite, LPC.mesh.transform.position, new Quaternion());
          newShieldSprite.transform.parent = LPC.mesh.transform;
          // scale hack
          newShieldSprite.transform.localRotation = new Quaternion();
          newShieldSprite.transform.localScale = new Vector3(0.5f, 1.67f, 1.0f);
          LPC.shieldSprite = newShieldSprite;
          GameObject newShieldCollider = (GameObject)Instantiate(shieldCollider, LPC.physics.transform.position, new Quaternion());
          newShieldCollider.transform.parent = LPC.physics.transform;
          LPC.shieldCollider = newShieldCollider;
          // shiled flag
          LPC.hasShield = true;
        }
        Destroy(transform.root.gameObject);
      }
      else if (collider.gameObject.tag == "Shot")
      {
        Instantiate(shieldExplosion, mesh.transform.position, new Quaternion());
        Destroy(collider.transform.root.gameObject);
        Destroy(transform.root.gameObject);
      }
    }
  }
}

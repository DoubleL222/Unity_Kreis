using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
  // shield
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
          LPC.shieldSprite.SetActive(true);
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

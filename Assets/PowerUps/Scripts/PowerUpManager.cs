using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
  // shield
  public GameObject explosion;

  void Start()
  {
    StartUpdate();
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.tag == "Shot" || collider.gameObject.tag == "PiercingShot")
    {
      Instantiate(explosion, mesh.transform.position, new Quaternion());
      Destroy(collider.transform.root.gameObject);
      Destroy(transform.root.gameObject);
    }
    else if (collider.gameObject.tag == "Player")
    {
      LocalPlayerController LPC = collider.gameObject.GetComponentInParent<LocalPlayerController>();

      if (gameObject.tag == "Shield")
        LPC.EnableShield();
      else if (gameObject.tag == "PiercingShotPickup")
      {
        LPC.piercingShot = true;
        LPC.piercingShotSprite.SetActive(true);
      }
      else if (gameObject.tag == "DeathPowerUp")
      {
        Instantiate(explosion, mesh.transform.position, new Quaternion());
        LPC.DestroyObject();
      }
      Destroy(transform.root.gameObject);
    }
  }
}
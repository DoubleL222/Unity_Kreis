using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
  // shield
  public GameObject explosion;
	
  // GM
  private GameManager GM;

  void Start()
  {
    StartUpdate();

    //GM
	GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.tag == "Shot" || collider.gameObject.tag == "PiercingShot")
    {
	  Destroy(collider.transform.root.gameObject);
	  DestroyPowerUp();
    }
    else if (collider.gameObject.tag == "Player")
    {
      LocalPlayerController LPC = collider.gameObject.GetComponentInParent<LocalPlayerController>();

      if (gameObject.tag == "Shield")
	  {
		GM.activePowerUps.Remove(this);
		LPC.EnableShield();
	  }
	  else if (gameObject.tag == "PiercingShotPickup")
      {
		GM.activePowerUps.Remove(this);
		LPC.piercingShot = true;
        LPC.piercingShotSprite.SetActive(true);
      }
      else if (gameObject.tag == "DeathPowerUp")
      {
		GM.activePowerUps.Remove(this);
		Instantiate(explosion, mesh.transform.position, new Quaternion());
        LPC.DestroyObject();
      }
      Destroy(transform.root.gameObject);
    }
  }

  public void DestroyPowerUp()
  {
    GM.activePowerUps.Remove(this);
    Instantiate(explosion, mesh.transform.position, new Quaternion());
	Destroy(transform.root.gameObject);
  }

  public IEnumerator DestroyPowerUpAfter(float delay)
  {
	yield return new WaitForSeconds (delay);
	DestroyPowerUp();
  }
}
using UnityEngine;
using System.Collections;

public class PowerUpManager : PolarPhysicsObject
{
  public enum PowerUpType
  {
    shield,
    piercingShot,
    death,
    bulldozer
  }

  // type
  public PowerUpType powerUpType;

  // explosion
  public GameObject explosion;

  // GM
  private GameManager GM;

  void Start()
  {
    StartUpdate();
    EndUpdate();

    //GM
    GM = FindObjectOfType<GameManager>();
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.tag == "Shot")
    {
      Destroy(collider.transform.root.gameObject);
      DestroyPowerUp();
    }
    else if (collider.gameObject.tag == "PiercingShot" || collider.gameObject.tag == "StarPulse")
    {
      DestroyPowerUp();
    }
    else if (collider.gameObject.tag == "Player")
    {
      LocalPlayerController LPC = collider.gameObject.GetComponentInParent<LocalPlayerController>();

      if (powerUpType == PowerUpType.shield)
      {
        GM.activePowerUps.Remove(this);
        LPC.EnableShield();

        SoundManager.play_pu_shield_pickup();
      }
      else if (powerUpType == PowerUpType.piercingShot)
      {
        GM.activePowerUps.Remove(this);
        LPC.piercingShot = true;
        LPC.piercingShotSprite.SetActive(true);

        SoundManager.play_pu_piercing_pickup();
      }
      else if (powerUpType == PowerUpType.death)
      {
        GM.activePowerUps.Remove(this);
        Instantiate(explosion, mesh.transform.position, new Quaternion());
        LPC.DestroyObject();

        SoundManager.play_death();
      }
      else if (powerUpType == PowerUpType.bulldozer)
      {
        LPC.isBulldozer = true;

        SoundManager.play_pu_bulldozer_pickup();
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
    yield return new WaitForSeconds(delay);
    DestroyPowerUp();
  }
}


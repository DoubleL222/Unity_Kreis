using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager
{
  // powerups span at random intervals from min to max duration
  public float minSpawnDuration;
  public float maxSpawnDuration;

  public int minNoOfEffects;
  public int maxNoOfEffects;

  // list of prefabs
  public List<GameObject> environmentEffects;

  // switch for enabling/disabling the powerup spawner
  public bool enableEnvironmentEffects = false;

  private float spawnDuration;
  private float timer = .0f;
  private int noOfEffects = 0;

  // GM
  private GameManager GM;

  public EnvironmentManager()
  {
    environmentEffects = new List<GameObject>();
  }

  public EnvironmentManager(GameManager GM, List<GameObject> environmentEffects, float minSpawnDuration, float maxSpawnDuration, int minNoOfEffects, int maxNoOfEffects)
  {
    this.GM = GM;
    this.environmentEffects = environmentEffects;
    noOfEffects = environmentEffects.Count;
    this.minSpawnDuration = minSpawnDuration;
    this.maxSpawnDuration = maxSpawnDuration;
    this.minNoOfEffects = minNoOfEffects;
    this.maxNoOfEffects = maxNoOfEffects;
    spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);
  }

  public void Update()
  {
    if (enableEnvironmentEffects == true)
    {
      timer += Time.deltaTime;

      if (timer > spawnDuration)
      {
        // spawn new powerup
        int randomEffect = Random.Range(0, noOfEffects);

        // spawn
        GameObject effect = environmentEffects[randomEffect];

        if (effect.name == "StarPulse")
        {
          // random position at the edge of the star
          float angle = Random.Range(PolarPhysicsObject.widthMultiplier * Mathf.PI, -PolarPhysicsObject.widthMultiplier * Mathf.PI);
          Vector3 pos = new Vector3(angle, 2.0f, 0.0f);
          GameObject starPulse = MonoBehaviour.Instantiate(effect, pos, new Quaternion()) as GameObject;
          StarPulseManager SPM = starPulse.GetComponent<StarPulseManager>();
          SPM.minNoOfBlasts = minNoOfEffects;
          SPM.maxNoOfBlasts = maxNoOfEffects;

          // TODO Star Pulse SFX
        }

        // reset timer
        spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);
        timer = .0f;
      }
    }
  }
}
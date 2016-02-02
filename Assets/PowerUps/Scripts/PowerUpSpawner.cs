﻿using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
  // powerups span at random intervals from min to max duration
  public float minSpawnDuration = 2.0f;
  public float maxSpawnDuration = 5.0f;
  // list of prefabs
  public List<GameObject> powerUps;
  // list of distances
  public List<float> spawnDistances;
  // shield
  public GameObject shield;

  // switch for enabling/disabling the powerup spawner
  [HideInInspector]
  public bool spawnPowerups = false;

  private float spawnDuration;
  private float timer = .0f;
  private int noOfPowerUps = 0;
  private int noOfDistances = 0;

  // Use this for initialization
  void Start ()
  {
    // duration of next spawn
    spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);

    noOfPowerUps = powerUps.Count;
    noOfDistances = spawnDistances.Count;
  }
	
	// Update is called once per frame
	void Update ()
  {
    if (spawnPowerups == true)
    {
      timer += Time.deltaTime;

      if (timer > spawnDuration)
      {
        // spawn new powerup
        int randomPowerup = Random.Range(0, noOfPowerUps);

        // random angle
        float angle = Random.Range(-10.0f * Mathf.PI, 10.0f * Mathf.PI);

        // random distance
        int randomDistance = Random.Range(0, noOfDistances);
        Vector3 pos = new Vector3(angle, spawnDistances[randomDistance], 0.0f);

        GameObject powerup = (GameObject)Instantiate(powerUps[randomPowerup], pos, new Quaternion());

        // reset timer
        spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);
        timer = .0f;
      }
    }
  }
}
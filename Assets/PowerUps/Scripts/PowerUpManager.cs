using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
  // powerups span at random intervals from min to max duration
  public float minSpawnDuration = 5.0f;
  public float maxSpawnDuration = 10.0f;
  // list of prefabs
  public List<GameObject> powerUps;
  // shield
  public GameObject shield;

  // switch for enabling/disabling the powerup spawner
  [HideInInspector]
  public bool spawnPowerups = false;

  private float spawnDuration;
  private float timer = .0f;
  private int noOfPowerUps = 0;

	// Use this for initialization
	void Start ()
  {
    // duration of next spawn
    spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);

    noOfPowerUps = powerUps.Count;
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
        int randomPowerup = Random.Range(0, noOfPowerUps - 1);
        GameObject[] segments = GameObject.FindGameObjectsWithTag("SegmentMesh");

        int randomSegment = Random.Range(0, segments.Length - 1);
        GameObject segment = segments[randomSegment];
		if(noOfPowerUps>0){
	        GameObject powerup = (GameObject)Instantiate(powerUps[randomPowerup], segment.transform.position, new Quaternion());

	        powerup.transform.parent = segment.transform;
		}
        // reset timer
        spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);
        timer = .0f;
      }
    }
  }
}

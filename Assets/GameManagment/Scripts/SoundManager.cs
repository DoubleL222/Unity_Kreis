using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

  public AudioClip[] audio_clips;
  public AudioClip[] audio_clips_crashes;
  public AudioClip[] audio_clips_deaths;

  public AudioSource sfx_audio;
  public static SoundManager SM_instance;

  // -----------------------------------------------------------------
  // -----------------------------------------------------------------

  /*

	   METHOD CALLS: 'SoundManager.play_<CLIP_DESCRIPTION>();'

	   special calls:

			SoundManager.play_crash()	- plays a random bump sound (player vs. player)
			SoundManager.play_death()	- plays a random death sound (player dies)

	   --------------------/
	   

	FORMAT:
	clip number - CLIP_DESCRIPTION

	0 - antigrav				x
	1 - grav					x
	2 - indestructable_segment					x	
	3 - max_speed				x
	4 - pu_shield_destroyed		x
	5 - pu_shield_pickup		x
	6 - pu_spawn				x
	7 - pu_bulldozer_pickup		x
	8 - pu_piercing_fire		x
	9 - pu_piercing_pickup		x
	10 - segment_destroyed		x
	11 - normal_shoot			x
	12 - spawn					x
	13 - victory_song					x			
	14 - big_boom				x

	*/

  // -----------------------------------------------------------------
  // -----------------------------------------------------------------

  void Awake()
  {
    SM_instance = this;
  }

  // PLAY METHODS
  public static void play_antigrav()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[0]);
  }

  public static void play_grav()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[1]);
  }

  public static void play_indestructable_segment()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[2]);
  }

  public static void play_max_speed()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[3]);
  }

  public static void play_pu_shield_destroyed()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[4]);
  }

  public static void play_pu_shield_pickup()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[5]);
  }

  public static void play_pu_spawn()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[6]);
  }

  public static void play_pu_bulldozer_pickup()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[7]);
  }

  public static void play_pu_piercing_fire()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[8]);
  }

  public static void play_pu_piercing_pickup()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[9]);
  }

  public static void play_segment_destroyed()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[10]);
  }

  public static void play_normal_shoot()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[11]);
  }

  public static void play_spawn()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[12]);
  }

  public static void play_victory_song()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[13]);
  }

  public static void play_big_boom()
  {
    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[14]);
  }


  // specials:

  public static void play_crash()
  {

    //int clip_index = SoundManager.get_random_interval (audio_clips_deaths.Length);

    int interval_count = 8;

    float step = 1.0f / interval_count;
    float current_pos = step / 2.0f;

    // prepare interval representatives
    float[] interval_mids = new float[interval_count];

    for (int i = 0; i < interval_count; i++)
    {

      interval_mids[i] = current_pos;
      current_pos += step;
    }


    float random_val = Random.value;

    // calculate distances from random value
    float[] distances = new float[interval_count];

    for (int i = 0; i < interval_count; i++)
    {

      distances[i] = Mathf.Abs(random_val - interval_mids[i]);
    }


    // find the interval index
    float min_distance = distances[0];
    int min_index = 0;

    for (int i = 1; i < interval_count; i++)
    {

      if (distances[i] < min_distance)
      {

        min_distance = distances[i];
        min_index = i;
      }
    }


    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips_crashes[min_index]);
    //SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips_crashes[clip_index]);
  }

  public static void play_death()
  {

    //int clip_index = SoundManager.get_random_interval (audio_clips_deaths.Length);

    int interval_count = 4;

    float step = 1.0f / interval_count;
    float current_pos = step / 2.0f;

    // prepare interval representatives
    float[] interval_mids = new float[interval_count];

    for (int i = 0; i < interval_count; i++)
    {

      interval_mids[i] = current_pos;
      current_pos += step;
    }


    float random_val = Random.value;

    // calculate distances from random value
    float[] distances = new float[interval_count];

    for (int i = 0; i < interval_count; i++)
    {

      distances[i] = Mathf.Abs(random_val - interval_mids[i]);
    }


    // find the interval index
    float min_distance = distances[0];
    int min_index = 0;

    for (int i = 1; i < interval_count; i++)
    {

      if (distances[i] < min_distance)
      {

        min_distance = distances[i];
        min_index = i;
      }
    }


    SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips_deaths[min_index]);
    //SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips_deaths[clip_index]);
  }


  // UTIL METHODS

  private int get_random_interval(int interval_count)
  {

    float step = 1.0f / interval_count;
    float current_pos = step / 2.0f;

    // prepare interval representatives
    float[] interval_mids = new float[interval_count];

    for (int i = 0; i < interval_count; i++)
    {

      interval_mids[i] = current_pos;
      current_pos += step;
    }


    float random_val = Random.value;

    // calculate distances from random value
    float[] distances = new float[interval_count];

    for (int i = 0; i < interval_count; i++)
    {

      distances[i] = Mathf.Abs(random_val - interval_mids[i]);
    }


    // find the interval index
    float min_distance = distances[0];
    int min_index = 0;

    for (int i = 1; i < interval_count; i++)
    {

      if (distances[i] < min_distance)
      {

        min_distance = distances[i];
        min_index = i;
      }
    }


    return min_index;
  }

  /*

	// normal shoot
	public static void PlayShotClip(){
		SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[0]);
	}


	public static void PlayJumpClip(){
		SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[1]);
	}

	// crash
	public static void PlayBumpClip(){
		SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[2]);
	}

	// spawn
	public static void PlaySpawnClip(){
		SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[3]);
	}

	// segment destroyed
	public static void PlayExplosionClip(){
		SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[4]);
	}

	// big boom
	public static void PlayBigBoomClip(){
		SM_instance.sfx_audio.PlayOneShot(SM_instance.audio_clips[5]);
	}


	*/
}
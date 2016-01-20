using UnityEngine;
using System.Collections;

public class Sound_mgr_script : MonoBehaviour {

	public AudioSource fx_sounds;
	public AudioSource music_sound;
	public static Sound_mgr_script instance = null;

	public float low_pitch = 1.0f;
	public float high_pitch = 3.0f;

	public float low_pitch_rand = 0.95f;
	public float high_pitch_rand = 1.05f;

	// Use this for initialization
	void Awake () {
	
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}
	
	public void PlaySingle(AudioClip clip, float pitch) {

		//fx_sounds.clip = clip;

		//float pitch_span = high_pitch - low_pitch;
		//float curr_pitch = low_pitch + (pitch * pitch_span);
		//fx_sounds.pitch = curr_pitch;
		//fx_sounds.PlayOneShot (clip);

		//fx_sounds.Play ();



		AudioSource curr_source = gameObject.AddComponent<AudioSource> ();
		curr_source.clip = clip;
		curr_source.Play ();

		/*

		//fx_sounds.PlayOneShot (clip);
		yield return new WaitForSeconds (clip.length);
		int dummy = 0;

		*/
	}

	public void Randomize_Sfx(params AudioClip[] clips) {

		int random_index = Random.Range (0, clips.Length);
		float random_pitch = Random.Range (low_pitch_rand, high_pitch_rand);

		fx_sounds.pitch = random_pitch;
		fx_sounds.clip = clips[random_index];
		fx_sounds.PlayOneShot (clips[random_index]);
	}
}

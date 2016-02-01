using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public AudioClip[] audioClips;
	public AudioSource sfxAudio;

	// Use this for initialization
	void Start ()
	{
	}

	public void PlayShotClip(){
		sfxAudio.PlayOneShot(audioClips[0]);
	}

	public void PlayJumpClip(){
		sfxAudio.PlayOneShot(audioClips[1]);
	}

	public void PlayBumpClip(){
		sfxAudio.PlayOneShot(audioClips[2]);
	}

	public void PlaySpawnClip(){
		sfxAudio.PlayOneShot(audioClips[3]);
	}
	public void PlayExplosionClip(){
		sfxAudio.PlayOneShot(audioClips[4]);
	}
	public void PlayBigBoomClip(){
		sfxAudio.PlayOneShot(audioClips[5]);
	}
	// Update is called once per frame
	void Update ()
	{

	}
}
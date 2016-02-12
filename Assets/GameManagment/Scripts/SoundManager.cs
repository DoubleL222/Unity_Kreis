using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public AudioClip[] audioClips;
	public AudioSource sfxAudio;
	public static SoundManager SMInstance;

	void Awake()
	{
		SMInstance = this;
	}
	// Use this for initialization
	void Start ()
	{
	}

	public static void PlayShotClip(){
		SMInstance.sfxAudio.PlayOneShot(SMInstance.audioClips[0]);
	}

	public static void PlayJumpClip(){
		SMInstance.sfxAudio.PlayOneShot(SMInstance.audioClips[1]);
	}

	public static void PlayBumpClip(){
		SMInstance.sfxAudio.PlayOneShot(SMInstance.audioClips[2]);
	}

	public static void PlaySpawnClip(){
		SMInstance.sfxAudio.PlayOneShot(SMInstance.audioClips[3]);
	}
	public static void PlayExplosionClip(){
		SMInstance.sfxAudio.PlayOneShot(SMInstance.audioClips[4]);
	}
	public static void PlayBigBoomClip(){
		SMInstance.sfxAudio.PlayOneShot(SMInstance.audioClips[5]);
	}
	// Update is called once per frame
	void Update ()
	{

	}
}
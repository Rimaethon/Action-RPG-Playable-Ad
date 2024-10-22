using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using _Scripts.Scriptable_Objects;
using UnityEngine;

namespace _Scripts.Managers
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField]
		private AudioLibrarySO audioLibrarySO;
		[SerializeField]
		private AudioSource musicSource;
		[SerializeField]
		private GameObject audioSourcePrefab;
		private AudioClip[] musicClips;
		private AudioClip[] sfxClips;
		private List<AudioSource> sfxSources;
		private const int initial_sfx_count = 5;

		private void Awake()
		{
			sfxSources = new List<AudioSource>();
			for (int i = 0; i < initial_sfx_count; i++)
			{
				GameObject newAudioSourceObject = Instantiate(audioSourcePrefab, transform);
				sfxSources.Add(newAudioSourceObject.GetComponent<AudioSource>());
			}
			musicClips = audioLibrarySO.MusicClips;
			sfxClips = audioLibrarySO.SFXClips;
			PlayMusic(MusicClips.BackgroundMusic);
		}

		private void PlayMusic(MusicClips clipEnum)
		{
			if (musicSource.isPlaying) musicSource.Stop();
			musicSource.clip = musicClips[(int) clipEnum];
			musicSource.Play();
		}

		public AudioSource PlaySFX(SFXClips clipEnum, bool isLooping = false)
		{
			AudioSource availableSource = sfxSources.FirstOrDefault(source => !source.isPlaying);

			// If there is no available AudioSource, create a new one
			if (availableSource == null)
			{
				GameObject newAudioSourceObject = Instantiate(audioSourcePrefab, transform);
				availableSource = newAudioSourceObject.GetComponent<AudioSource>();
				sfxSources.Add(availableSource);
			}

			availableSource.clip = sfxClips[(int) clipEnum];
			availableSource.loop = isLooping;

			if (isLooping)
			{
				availableSource.Play();
			}
			else
			{
				availableSource.PlayOneShot(availableSource.clip);
			}

			return availableSource;
		}
	}
}

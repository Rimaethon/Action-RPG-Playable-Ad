﻿using UnityEngine;

namespace _Scripts.Scriptable_Objects
{
	[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library", order = 1)]
	public class AudioLibrarySO : ScriptableObject
	{
		public AudioClip[] SFXClips;
		public AudioClip[] MusicClips;
	}
}

using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class AudioManager : MonoBehaviour
	{
		private static AudioManager _instance;

		private bool _isMusicDisable;
		private bool _isSoundDisable;
		private AppPrefs _appPrefs;

		public AudioClip Change;
		public AudioClip Click;
		public AudioClip NextLevel;
		public AudioClip Fail;
		public AudioClip Punch;
		public AudioClip Turn;

		public AudioSource ClockAudioSource;
		public AudioSource MusicAudioSource;

		public void Awake()
		{
			_instance = this;
			_appPrefs = new AppPrefs();
			_appPrefs.Load();
			_isMusicDisable = _appPrefs.IsMusicDisable;
			_isSoundDisable = _appPrefs.IsSoundDisable;
		}

		public void Start()
		{
			if (!_isMusicDisable)
				MusicAudioSource.Play();
		}

		private void PlaySound(Sound sound)
		{
			if (_isSoundDisable)
				return;

			switch (sound)
			{
				case Sound.Click:
					audio.PlayOneShot(Click);
					break;
				case Sound.Match:
					audio.PlayOneShot(Change);
					break;
				case Sound.Clock:
					ClockAudioSource.Play();
					break;
				case Sound.NextLevel:
					audio.PlayOneShot(NextLevel);
					break;
				case Sound.Fail:
					audio.PlayOneShot(Fail);
					break;
				case Sound.Punch:
					audio.PlayOneShot(Punch);
					break;
				case Sound.Turn:
					audio.PlayOneShot(Turn);
					break;
				default:
					throw new NotSupportedException(sound.ToString());
			}
		}

		private void StopSound(Sound sound)
		{
			switch (sound)
			{
				case Sound.Clock:
					ClockAudioSource.Stop();
					break;
				default:
					throw new NotSupportedException(sound.ToString());
			}
		}

		public static void Play(Sound sound)
		{
			_instance.PlaySound(sound);
		}

		public static void Stop(Sound sound)
		{
			_instance.StopSound(sound);
		}

		public static void SoundSwitchOnOff()
		{
			_instance.SoundSwitchOnOffInternal();
		}

		private void SoundSwitchOnOffInternal()
		{
			_isSoundDisable = !_isSoundDisable;
			_appPrefs.IsSoundDisable = _isSoundDisable;
			_appPrefs.Save();
		}

		public static void MusicSwitchOnOff()
		{
			_instance.MusicSwitchOnOffInternal();
		}

		private void MusicSwitchOnOffInternal()
		{
			_isMusicDisable = !_isMusicDisable;
			if (_isMusicDisable)
				MusicAudioSource.Stop();
			else
				MusicAudioSource.Play();

			_appPrefs.IsMusicDisable = _isMusicDisable;
			_appPrefs.Save();
		}
		
	}
}

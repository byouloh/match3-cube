using Assets.Scripts.Events;
using Assets.Scripts.ToastManagement;
using UnityEngine;

namespace Assets.Scripts
{
	public class TimerManager : MonoBehaviour
	{
		private const string BONUS_SECONDS_TOAST_STYLE = "BonusSecondsToastStyleKey";
		private const string PENALTY_SECONDS_TOAST_STYLE = "PenaltySecondsToastStyleKey";
		private const string SECONDS_TOAST_CATEGORY = "SecondsToastCategory";
		private float _startTime;
		private int _bonusSeconds;
		private int _penaltySeconds;
		private bool _isOver;
		private bool _isNearToOver;
		private GameObject _cameraFade;

		public int SecondsOnStart;
		public int SecondsNearToOver;
		public GUIStyle BonusToastStyle;
		public GUIStyle PenaltyToastStyle;
		public Texture2D FadeBackground;
		public int NextLevelBonusTime;

		public static TimerManager Instance { get; private set; }

		public int RemainSeconds { get; private set; }

		public void Awake()
		{
			Instance = this;
			_cameraFade = iTween.CameraFadeAdd(FadeBackground);
		}

		public void Start ()
		{
			Reset();

			ToastManager.RegisterStyle(BONUS_SECONDS_TOAST_STYLE,
			                           new ToastStyle
				                           {
					                           Effect = Effect.Transparency,
					                           Duration = 2f,
											   GUIStyle = BonusToastStyle
				                           });

			ToastManager.RegisterStyle(PENALTY_SECONDS_TOAST_STYLE,
									   new ToastStyle
									   {
										   Effect = Effect.Transparency,
										   Duration = 2f,
										   GUIStyle = PenaltyToastStyle
									   });

			GameEvents.MatchesRemoved.Subscribe(OnMatchesRemoved);
			GameEvents.NextLevel.Subscribe(OnNextLevel);
		}

		private void OnMatchesRemoved(MatchesEventArgs matchesEventArgs)
		{
			AddBonusSeconds(matchesEventArgs.Matches.Count);
		}

		private void AddBonusSeconds(int seconds)
		{
			_bonusSeconds += seconds;
			ToastManager.Push(
				string.Format("+{0} sec", seconds),
				new Vector2(120, 90),
				BONUS_SECONDS_TOAST_STYLE,
				SECONDS_TOAST_CATEGORY);
		}

		public void AddPenaltySeconds(int seconds)
		{
			PenaltyFade();
			_penaltySeconds += seconds;
			ToastManager.Push(
				string.Format("-{0} sec", seconds),
				new Vector2(120, 90),
				PENALTY_SECONDS_TOAST_STYLE, 
				SECONDS_TOAST_CATEGORY);
		}

		private void PenaltyFade()
		{
			iTween.Stop(_cameraFade);
			iTween.CameraFadeTo(0.5f, 0.1f);
			iTween.CameraFadeTo(iTween.Hash(iT.CameraFadeTo.amount, 0.0f,
											  iT.CameraFadeTo.time, 0.5f,
											  iT.CameraFadeTo.delay, 0.2f));
			AudioManager.Play(Sound.Fail);
		}

		public void Update()
		{
			RemainSeconds = Mathf.Clamp(SecondsOnStart + _bonusSeconds - _penaltySeconds - (int) (Time.time - _startTime),
			                                0, int.MaxValue);

			if (RemainSeconds == 0 && !_isOver)
			{
				_isOver = true;
				GameEvents.GameOver.Publish(GameEventArgs.Empty);
			}

			if (RemainSeconds > 0 && RemainSeconds <= SecondsNearToOver && !_isNearToOver)
			{
				_isNearToOver = true;
				AudioManager.Play(Sound.Clock);
			}

			if ((RemainSeconds == 0 || RemainSeconds > SecondsNearToOver) && _isNearToOver)
			{
				_isNearToOver = false;
				AudioManager.Stop(Sound.Clock);
			}
		}

		public static void Reset()
		{
			Instance._startTime = Time.time;
			Instance._bonusSeconds = 0;
			Instance._penaltySeconds = 0;
			Instance._isOver = false;
			Instance._isNearToOver = false;
		}

		private void OnNextLevel(GameEventArgs gameEventArgs)
		{
			AddBonusSeconds(NextLevelBonusTime);
		}


	}
}

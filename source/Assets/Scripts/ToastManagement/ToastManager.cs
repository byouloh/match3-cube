using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Levels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.ToastManagement
{
	public class ToastManager : MonoBehaviourBase
	{
		private static readonly Dictionary<string, ToastStyle> _toastStyles;
		public static ToastManager Instance { get; private set; }

		private const string DEFAULT_TOAST_STYLE_NAME = "DefaultToastStyleName";
		private List<Toast> _activeToasts;

		public UILabel BonusSecondsToast;
		public UILabel PenaltySecondsToast;
		public UILabel LevelTitleToast;
		public UILabel ScoreToast;
		public UILabel CubeScoreToast;
		public Transform Hoster;

		public Color[] Colors;

		static ToastManager()
		{
			_toastStyles = new Dictionary<string, ToastStyle>();
		}

		public void Awake()
		{
			_activeToasts = new List<Toast>();
			Instance = this;
			RegisterDefaultToastStyle();
		}

		private void RegisterDefaultToastStyle()
		{
			RegisterStyle(DEFAULT_TOAST_STYLE_NAME, ToastStyle.CreateDefault());
		}

		public void Update()
		{
			for (int i = 0; i < _activeToasts.Count; i++)
			{
				Toast toast = _activeToasts[i];
				toast.Update();
				if (!toast.IsActive)
				{
					RemoveToast(toast);
					i--;
				}
			}
		}

		private void RemoveToast(Toast toast)
		{
			_activeToasts.Remove(toast);
			Destroy(toast.Label.gameObject);
		}

		public static void RegisterStyle(string key, ToastStyle toastStyle)
		{
			_toastStyles.Add(key, toastStyle);
		}

		private static Vector2 GetDefaultStartPosition()
		{
			return new Vector2(Screen.width/2, Screen.height/2);
		}

		public static void Push(string message)
		{
			Instance.PushInternal(message, GetDefaultStartPosition(), DEFAULT_TOAST_STYLE_NAME);
		}

		public static void Push(string message, Vector2 position, string key)
		{
			Instance.PushInternal(message, position, key);
		}

		public static void Push(string message, string key)
		{
			Instance.PushInternal(message, null, key);
		}

		private void PushInternal(string message, Vector2? position, string key)
		{
			UILabel toastPrefab = GetToastPrefab(key);
			UILabel label = Instantiate<UILabel>(toastPrefab);
			label.text = message;
			label.cachedTransform.parent = Hoster;
			if (position.HasValue)
				label.transform.localPosition = position.Value;
			else
				label.transform.localPosition = toastPrefab.transform.localPosition;
			label.transform.localScale = toastPrefab.transform.localScale;

			ToastStyle toastStyle = _toastStyles[key];

			if (toastStyle.IsRandomColor)
				label.color = GetRandomColor();

			Toast toast = new Toast(label, toastStyle);

			if (toastStyle.Category != ToastStyle.EMPTY_CATEGORY)
				ClearCategory(toastStyle.Category);

			_activeToasts.Add(toast);
		}

		private void ClearCategory(string category)
		{
			foreach (Toast toast in _activeToasts.Where(t => t.Style.Category == category).ToArray())
				RemoveToast(toast);
		}

		private UILabel GetToastPrefab(string key)
		{
			switch (key)
			{
				case TimerManager.BONUS_SECONDS_TOAST_STYLE:
					return BonusSecondsToast;
				case TimerManager.PENALTY_SECONDS_TOAST_STYLE:
					return PenaltySecondsToast;
				case LevelsManager.LEVEL_TOAST_STYLE:
					return LevelTitleToast;
				case ScoreManager.SCORE_TOAST_STYLE:
					return ScoreToast;
				case ScoreManager.CUBE_SCORE_TOAST_STYLE:
					return CubeScoreToast;
				default:
					throw new NotSupportedException(key);
			}
		}

		private Color GetRandomColor()
		{
			return Colors[Random.Range(0, Colors.Length)];
		}
	}
}

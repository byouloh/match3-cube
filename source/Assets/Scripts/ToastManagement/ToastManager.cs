using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ToastManagement
{
	public class ToastManager : MonoBehaviour
	{
		private static readonly Dictionary<string, ToastStyle> _toastStyles;
		public static ToastManager Instance { get; private set; }

		private const string DEFAULT_TOAST_STYLE_NAME = "DefaultToastStyleName";
		private const string EMPTY_CATEGORY = "EmptyCategory";
		private List<Toast> _activeToasts;

		public GUIStyle Style;


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

		public void OnGUI()
		{
			foreach (Toast toast in _activeToasts.ToArray())
			{
				toast.OnGUI();
				if (!toast.IsActive)
					_activeToasts.Remove(toast);
			}
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
			Instance.PushInternal(message, GetDefaultStartPosition(), DEFAULT_TOAST_STYLE_NAME, EMPTY_CATEGORY);
		}

		public static void Push(string message, Vector2 position, string key)
		{
			Instance.PushInternal(message, position, key, EMPTY_CATEGORY);
		}

		public static void Push(string message, Vector2 position, string key, string category)
		{
			Instance.PushInternal(message, position, key, category);
		}

		private void PushInternal(string message, Vector2 position, string key, string category)
		{
			ToastStyle toastStyle = _toastStyles[key];
			Toast toast = new Toast(message, position, toastStyle, category);

			if (category != EMPTY_CATEGORY)
				_activeToasts.RemoveAll(t => t.Category == category);

			_activeToasts.Add(toast);
		}
	}
}

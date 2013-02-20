using UnityEngine;

namespace Assets.Scripts.ToastManagement
{
	public class Toast
	{
		private readonly float _startTime;
		private readonly ToastStyle _style;
		private readonly string _message;
		private readonly Vector2 _startPosition;
		private int? _fontOriginalSize;


		public string Category { get; private set; }
		public bool IsActive { get; private set; }
		
		public Toast(string message, Vector2 position, ToastStyle toastStyle, string category)
		{
			_startTime = Time.time;
			_style = toastStyle;
			_message = message;
			_startPosition = position;
			Category = category;
			IsActive = true;
		}

		public void OnGUI()
		{
			float lifeTime = Time.time - _startTime;
			float lifeTimePercent = Mathf.Clamp(lifeTime * 100 / _style.Duration, 0, 100);
			
			float width = 200;
			float height = 40;

			float yOffset = OnBubble(lifeTime);
			OnTransparency(lifeTimePercent);
			OnOncoming(lifeTimePercent);

			GUI.Label(new Rect(_startPosition.x - width/2, _startPosition.y - height/2 + yOffset, width, height),
			          _message, _style.GUIStyle);
			IsActive = lifeTime <= _style.Duration;
		
		}

		private float OnBubble(float lifeTime)
		{
			if ((_style.Effect & Effect.Bubble) == Effect.Bubble)
			{
				return -lifeTime * _style.BubbleSpeed;
			}
			return 0;
		}

		private void OnTransparency(float lifeTimePercent)
		{
			if ((_style.Effect & Effect.Transparency) == Effect.Transparency)
			{
				float alpha = Mathf.Lerp(1, 0, lifeTimePercent / 100);
				_style.GUIStyle.normal.textColor = new Color(
					_style.GUIStyle.normal.textColor.r,
					_style.GUIStyle.normal.textColor.g,
					_style.GUIStyle.normal.textColor.b,
					alpha);
			}
		}

		private void OnOncoming(float lifeTimePercent)
		{
			if ((_style.Effect & Effect.Oncoming) == Effect.Oncoming)
			{
				if (_fontOriginalSize == null)
					_fontOriginalSize = _style.GUIStyle.fontSize;

				int fontSize = (int) Mathf.Lerp(1, _fontOriginalSize.Value, lifeTimePercent/100);
				_style.GUIStyle.fontSize = fontSize;
			}
		}

	}
}

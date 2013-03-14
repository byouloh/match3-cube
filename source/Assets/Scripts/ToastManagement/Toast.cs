using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.ToastManagement
{
	public class Toast
	{
		
		private readonly TimeEvent _finishTime;
		private readonly Vector2 _startLocalPosition;
		private readonly Vector2 _startLocalScale;

		public ToastStyle Style { get; private set; }
		public UILabel Label { get; private set; }
		public bool IsActive { get; private set; }

		private readonly Vector2 _startPosition;
	
		public Toast(UILabel label, ToastStyle style)
		{
			Style = style;
			_finishTime = new TimeEvent(Style.Duration);
			Label = label;
			IsActive = true;
			_startLocalPosition = Label.transform.localPosition;
			_startLocalScale = Label.transform.localScale;
		}

		public void Update()
		{
			if (IsActive)
			{
				OnTransparency();
				OnBubble();
				OnOncoming();
				IsActive = !_finishTime.PopIsOccurred();
			}
		}

		private void OnTransparency()
		{
			if ((Style.Effect & Effect.Transparency) == Effect.Transparency)
			{
				float alpha = Mathf.Lerp(1, 0, _finishTime.LifeTimePercent / 100);
				Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, alpha);
			}
		}

		private void OnBubble()
		{
			if ((Style.Effect & Effect.Bubble) == Effect.Bubble)
			{
				float yOffset = _finishTime.LifeTime * Style.BubbleSpeed;
				Label.cachedTransform.localPosition = new Vector3(_startLocalPosition.x, _startLocalPosition.y + yOffset);
			}
		}

		private void OnOncoming()
		{
			if ((Style.Effect & Effect.Oncoming) == Effect.Oncoming)
			{
				float xScale = _startLocalScale.x*_finishTime.LifeTimePercent/100f;
				float yScale = _startLocalScale.y*_finishTime.LifeTimePercent/100f;
				Label.transform.localScale = new Vector3(xScale, yScale);
			}
		}

		public override string ToString()
		{
			return Label.text;
		}

	}
}

using UnityEngine;

namespace Assets.Scripts.Common
{
	public class TimeEvent
	{
		private readonly float _duration;
		private float _lastTimeOccured;

		public float LifeTime{ get { return Time.time - _lastTimeOccured; } }
		public float LifeTimePercent{ get { return Mathf.Clamp(LifeTime*100/_duration, 0, 100); } }

		public TimeEvent(float duration)
		{
			_duration = duration;
			_lastTimeOccured = Time.time;
		}

		public bool PopIsOccurred()
		{
			bool isOccurred = _lastTimeOccured + _duration <= Time.time;
			if (isOccurred)
				_lastTimeOccured = Time.time;
			return isOccurred;
		}

	}
}

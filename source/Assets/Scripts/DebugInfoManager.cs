using UnityEngine;

namespace Assets.Scripts
{
	public class DebugInfoManager : MonoBehaviour 
	{
		private const float _updateInterval = 0.5f;
		private float _lastInterval; // Last interval end time
		private int _frames = 0; // Frames over current interval
		private float _fps ; // Current FPS

		public void OnGUI()
		{
			GUILayout.BeginVertical();
			GUILayout.Label(_fps.ToString());
			//GUILayout.Label(string.Format("Is web player:{0}", Application.isWebPlayer));
			GUILayout.EndVertical();
		} 

		public void Update ()
		{
			TrackFPS();
		}

		private void TrackFPS()
		{
			++_frames;
			float timeNow = Time.realtimeSinceStartup;
			if (timeNow > _lastInterval + _updateInterval)
			{
				_fps = _frames / (timeNow - _lastInterval);
				_frames = 0;
				_lastInterval = timeNow;
			}
		}
	}
}

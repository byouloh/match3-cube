using UnityEngine;

public class FPSManager : MonoBehaviour 
{
	private const float _updateInterval = 0.5f;
	private float _lastInterval; // Last interval end time
	private int _frames = 0; // Frames over current interval
	private float _fps ; // Current FPS

	void Start () 
	{
	
	}

	void OnGUI()
	{
		GUILayout.Label(_fps.ToString());
	} 

	void Update ()
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

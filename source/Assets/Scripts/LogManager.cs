using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts
{
	public class LogManager : MonoBehaviourBase
	{
		private List<string> _logs; 

		public void Awake()
		{
			Application.RegisterLogCallback(LogHandler);
			_logs = new List<string>();
		}

		private void LogHandler(string condition, string stackTrace, LogType type)
		{
			if(type == LogType.Error || type == LogType.Exception)
				_logs.Add(condition);
		}

		public void OnGUI()
		{
			GUILayout.BeginVertical();
			foreach (string log in _logs)
				GUILayout.Label(log);
			GUILayout.EndVertical();
		}
	}
}

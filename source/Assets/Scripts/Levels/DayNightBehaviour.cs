using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class DayNightBehaviour : LevelBehaviour
	{
		public static DayNightBehaviour Instance { get; private set; }

		private Color _defaultColor;
		private GameObject _gameObject;
		private GUIText _guiText;

		public Light MainLight;
		public float LoopTime;

		public void Awake()
		{
			Instance = this;
			_defaultColor = RenderSettings.ambientLight;
			_gameObject = new GameObject("DayNightColor");
			_guiText = _gameObject.AddComponent<GUIText>();
		}

		public void Update()
		{
			if (IsActive)
			{
				RenderSettings.ambientLight = _guiText.material.color;
			}
		}


		public override void OnRun()
		{
			MainLight.gameObject.SetActive(false);
			iTween.ColorTo(_gameObject, iTween.Hash(iT.ColorTo.color, Color.black,
			                                        iT.ColorTo.time, LoopTime,
			                                        iT.ColorTo.looptype, iTween.LoopType.pingPong,
													iT.ColorTo.easetype, iTween.EaseType.easeInQuad));
		}

		public override void OnStop()
		{
			MainLight.gameObject.SetActive(true);
			RenderSettings.ambientLight = _defaultColor;
		}
	}
}

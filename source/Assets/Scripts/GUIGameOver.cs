using UnityEngine;

namespace Assets.Scripts
{
	public class GUIGameOver : MonoBehaviour
	{
		private bool _isActive;

		public static GUIGameOver Instance { get; set; }

		void Start()
		{
			Instance = this;
		}

		void OnGUI()
		{
			if (_isActive)
				GUI.Window(0, new Rect(200, 200, 200, 200), CreateWindow, string.Empty);
		}

		private void CreateWindow(int id)
		{
			GUI.Label(new Rect(0, 0, 100, 40), "Time is up");
			GUI.Label(new Rect(0, 40, 100, 40), string.Format("Score:{0}", ScoreManager.Instance.Score));
			if (GUI.Button(new Rect(0, 80, 120, 40), "Again"))
			{
				_isActive = false;
				FieldManager.Instance.StartGame();
			}
		}

		public static void Show()
		{
			Instance._isActive = true;
		}
	}
}

using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts
{
	public class TryBoomScript : MonoBehaviour
	{
		private const float BUTTON_WIDTH = 80;

		private GameObject _gameObject;
		private bool _isDeadEnd;
		private float _deadEndStartTime;
		private bool _isHintShowing;
		private bool _shouldCheckOnDeadEnd;

		public int HintsOnStart;
		public int RemainHints;
		public float NoDefaultHintPeriod;//in seconds

		public static TryBoomScript Instance { get; private set; }

		public void Awake()
		{
			Instance = this;

			_gameObject = new GameObject();
			_gameObject.transform.position = new Vector3(Screen.width / 2 - BUTTON_WIDTH / 2, 40);
		}

		public void Start()
		{
			GameEvents.StateChanged.Subscribe(args => _shouldCheckOnDeadEnd = true);
		}

		public void Update()
		{
			if (_shouldCheckOnDeadEnd)
			{
				CheckOnDeadEnd();
			}

			if (_isDeadEnd && !_isHintShowing && _deadEndStartTime + NoDefaultHintPeriod <= Time.time)
			{
				ShowHint();
			}
		}

		private void CheckOnDeadEnd()
		{
			_shouldCheckOnDeadEnd = false;
			bool isDeadEnd = !FieldManager.Instance.HasAvailableMoves();
			if (isDeadEnd && !_isDeadEnd) //dead end just started
			{
				_isDeadEnd = true;
				_deadEndStartTime = Time.time;

				if (RemainHints > 0)
				{
					RemainHints--;
					ShowHint();
				}
			}
		}

		private void ShowHint()
		{
			_isHintShowing = true;
			iTween.ShakePosition(_gameObject, new Vector3(5, 5), 100);
		}

		public void OnGUI()
		{
			if (GUI.Button(
				new Rect(_gameObject.transform.position.x, _gameObject.transform.position.y, BUTTON_WIDTH, 25), "Try BOOM!"))
			{
				if (FieldManager.Instance.HasAvailableMoves())
					FailBoom();
				else
					Boom();
			}
		}

		private void FailBoom()
		{
			TimerManager.Instance.AddPenaltySeconds(5);
			FieldManager.Instance.ShowAvailableMove();
		}

		private void Boom()
		{
			AudioManager.Play(Sound.Punch);
			FieldManager.Instance.ClearTopLair();
			_isDeadEnd = false;
			_isHintShowing = false;
			iTween.Stop(_gameObject);
		}

		public void Reset()
		{
			RemainHints = HintsOnStart;
			_isDeadEnd = false;
			_isHintShowing = false;
			iTween.Stop(_gameObject);
		}
	}
}

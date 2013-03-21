using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Android
{
	public class SwipeDetector : MonoBehaviourBase
	{
		private TouchPhase? _touchPhase;
		private Vector2 _beginPositioin;
		private bool _wasMove;

		public static SwipeDetector Instance { get; private set; }

		public event EventHandler<SwipeEventArgs> OnSwipe;

		public void Awake()
		{
			Instance = this;
		}

		public void Update()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					_beginPositioin = touch.position;
					_wasMove = false;
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					_wasMove = true;
				}
				else if (touch.phase == TouchPhase.Ended && _wasMove)
				{
					if (OnSwipe != null)
						OnSwipe(this, new SwipeEventArgs(_beginPositioin, touch.position));
				}				
			}
		}
	}
}

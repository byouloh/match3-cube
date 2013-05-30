//using Assets.Scripts.Common;
//using UnityEngine;

//namespace Assets.Scripts.Levels
//{
//	public class WiggleBehaviour : LevelBehaviour
//	{
//		public static WiggleBehaviour Instance { get; private set; }

//		private int _curInd = -1;

//		private TimeEvent _nextTurnEvent;
//		public Transform Cubes;
//		public float Period;	//In seconds
//		public string PathName;

//		public void Awake()
//		{
//			Instance = this;
//		}

//		public void Update()
//		{
//			if (IsActive && _nextTurnEvent.PopIsOccurred())
//			{
//				NextTurn();
//			}
//		}

//		public override void OnRun()
//		{
//			_nextTurnEvent = new TimeEvent(Period);
//			NextTurn();
//		}

//		public override void OnStop()
//		{
//			iTween.Stop(Cubes.gameObject);
//			iTween.RotateTo(Cubes.gameObject, new Vector3(0, 0, 0), 0.5f);
//		}

//		private void NextTurn()
//		{
//			Vector3[] points = iTweenPath.GetPath(PathName);
//			_curInd = (_curInd + 1)%points.Length;
//			iTween.RotateBy(Cubes.gameObject, iTween.Hash(
//				iT.RotateBy.easetype, iTween.EaseType.linear,
//				iT.RotateBy.amount, points[_curInd],
//				iT.RotateBy.time, Period - 0.2f));
//		}
//	}
//}

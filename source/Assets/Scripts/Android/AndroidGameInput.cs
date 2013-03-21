using Assets.Scripts.Common;

namespace Assets.Scripts.Android
{
	public class AndroidGameInput : MonoBehaviourBase
	{
		private CubeItem _firstCube;
		private CubeItem _secondCube;

		public void Start()
		{
			SwipeDetector.Instance.OnSwipe += OnSwipe;
		}

		public void Update()
		{
			if (_firstCube != null)
			{
				FieldManager.Instance.OnCubeSelect(_firstCube);
				_firstCube = null;
			}
			else if (_secondCube != null)
			{
				FieldManager.Instance.OnCubeSelect(_secondCube);
				_secondCube = null;
			}
		}

		private void OnSwipe(object sender, SwipeEventArgs e)
		{
			CubeItem firstCube = FieldManager.Instance.GetCubeItem(e.BeginPosition);
			CubeItem secondCube = FieldManager.Instance.GetCubeItem(e.EndPosition);
			if (firstCube != null && secondCube != null && firstCube != secondCube)
			{
				_firstCube = firstCube;
				_secondCube = secondCube;
			}
		}
	}
}

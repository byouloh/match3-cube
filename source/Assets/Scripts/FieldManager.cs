using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.Levels;
using Assets.Scripts.ToastManagement;
using Assets.Scripts.Tweens;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
	public class FieldManager : MonoBehaviourBase
	{
		private const string LEVEL_TOAST_STYLE = "CubeScoreStyleKey";

		private CubeItem _firstSelectedCube;
		private CubeItem _secondSelectedCube;
		private CubeItem[,] _field;
		private CubeItem _selectedCube;
		private bool _isFirsLaunch = true;
		private bool _isNewGameStarted;

		public Transform CubesHoster;
		public int X;
		public int Y;
		public float CubeArea = 1.3f;
		public Material QuestionMaterial;
		public Material MoleMaterial;
		public Transform[] Stars;

		public static FieldManager Instance { get; private set; }

		public void Awake()
		{
			Random.seed = DateTime.Now.Millisecond;
			Instance = this;

			ToastManager.RegisterStyle(LEVEL_TOAST_STYLE,
									   new ToastStyle
									   {
										   Effect = Effect.Bubble,
										   Duration = 1f
									   });
			GameEvents.SnakeMoved.Subscribe(OnSnakeMoved);
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);
		}

		public void Update()
		{
			if (_isFirsLaunch)
			{
				GameEvents.StartNewGame.Publish(GameEventArgs.Empty);
				_isFirsLaunch = false;
				return;
			}

			if (_isNewGameStarted)
			{
				CheckOnMatches();
				_isNewGameStarted = false;
				return;
			}

			if (_selectedCube != null)
			{
				SelectCube(_selectedCube);
				_selectedCube = null;
			}
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			StartGame();
		}

		private void StartGame()
		{
			_firstSelectedCube = null;
			_secondSelectedCube = null;
			ClearField();
			BuildField();
			_isNewGameStarted = true;
		}

		#region Build functions

		private void BuildField()
		{
			_field = new CubeItem[X, Y];
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
						CreateCube(x, y);
		}

		private void ClearField()
		{
			if (_field == null)
				return;

			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					RemoveCube(_field[x, y]);
		}

		private void CreateCube(int x, int y)
		{
			Transform starPrefab = Stars[Random.Range(0, Stars.Length)];
			Transform cube = (Transform)Instantiate(starPrefab);
			CubeItem item = cube.GetComponent<CubeItem>();
			item.SetPosition(x, y);
			cube.parent = CubesHoster;
			item.UpdatePosition();
			_field[x, y] = item;
		}

		#endregion

		#region Cube selection

		public void OnCubeSelect(CubeItem cube)
		{
			_selectedCube = cube;
		}

		private void SelectCube(CubeItem cube)
		{
			bool canSelect = CheckCubeSelection(cube);
			if (canSelect)
			{
				if (_firstSelectedCube == null)
				{
					_firstSelectedCube = cube;
					cube.MarkSelected();
				}
				else
				{
					if (_firstSelectedCube == cube)
					{
						cube.MarkUnSelected();
						_firstSelectedCube = null;
					}
					else
					{
						if (!CubeItem.IsNeighbors(_firstSelectedCube, cube))
						{
							_firstSelectedCube.MarkUnSelected();
							cube.MarkSelected();
							_firstSelectedCube = cube;
						}
						else
						{
							_secondSelectedCube = cube;
							cube.MarkSelected();
							OnBothCubesSelected();
						}
					}
				}
			}
		}

		private bool CheckCubeSelection(CubeItem cube)
		{
			if (VirusesBehaviour.Instance.IsCubeInfected(cube.Position))
			{
				TimerManager.Instance.AddPenaltySeconds(5);
				return false;
			}

			if (cube.IsQuestion && _firstSelectedCube == null)
			{
				cube.TransformToDefault();
				return false;
			}

			if (cube.IsMole)
			{
				TimerManager.Instance.AddPenaltySeconds(5);
				return false;
			}

			return true;
		}

		private void OnBothCubesSelected()
		{
			if (CubeItem.IsNeighbors(_firstSelectedCube, _secondSelectedCube))
			{
				CubeItem[,] field = GetTopLair();
				Swap(field,
				     _firstSelectedCube.Position.X, _firstSelectedCube.Position.Y,
				     _secondSelectedCube.Position.X, _secondSelectedCube.Position.Y);
				if (HasMatches(field))
					StartCoroutine(Swap(_firstSelectedCube, _secondSelectedCube));
				else
					FailSwap(_firstSelectedCube, _secondSelectedCube);
			}

			_firstSelectedCube.MarkUnSelected();
			_secondSelectedCube.MarkUnSelected();
			_firstSelectedCube = null;
			_secondSelectedCube = null;
		}

		private void OnSnakeMoved(PositionEventArgs positionEventArgs)
		{
			if (_firstSelectedCube != null &&
				_firstSelectedCube.Position.Equals(positionEventArgs.Position))
			{
				_firstSelectedCube.MarkUnSelected();
				_firstSelectedCube = null;
			}
		}

		#endregion

		#region Swap and Matches

		private void CheckOnMatches()
		{
			List<Match> matches = GetMatches(GetTopLair()).ToList();
			if (matches.Any())
			{
				GameLocker.Lock();
				foreach (CubeItem cubeItem in matches.SelectMany(m => m.Cubes).Distinct())
					RemoveCube(cubeItem);

				AudioManager.Play(Sound.Match);
				StartCoroutine(CompleteCube());
				GameEvents.MatchesRemoved.Publish(new MatchesEventArgs(matches));
			}
			else
			{
				GameLocker.Unlock();
			}
		}

		private void RemoveCube(CubeItem cube)
		{
			iTween.Stop(cube.gameObject);
			cube.transform.SetZ(-1);

			GravityTween.Run(cube.gameObject, 1.5f);
			Destroy(cube.gameObject, 1.5f);

			_field[cube.Position.X, cube.Position.Y] = null;
			GameEvents.CubeRemoved.Publish(new PositionEventArgs(cube.Position));
			GameEvents.StateChanged.Publish(GameEventArgs.Empty);
		}

		private void FailSwap(CubeItem cube1, CubeItem cube2)
		{
			iTween.Stop(cube1.gameObject);
			Vector3 amount1 = cube2.transform.localPosition - cube1.transform.localPosition;
			iTween.MoveBy(cube1.gameObject, amount1, 0.2f);
			iTween.MoveBy(cube1.gameObject,
			              iTween.Hash(iT.MoveBy.amount, -amount1,
			                          iT.MoveBy.delay, 0.2f,
			                          iT.MoveBy.time, 0.2f,
			                          iT.MoveBy.oncomplete, "UpdatePosition"));

			iTween.Stop(cube2.gameObject);
			Vector3 amount2 = cube1.transform.localPosition - cube2.transform.localPosition;
			iTween.MoveBy(cube2.gameObject, amount2, 0.2f);
			iTween.MoveBy(cube2.gameObject,
			              iTween.Hash(iT.MoveBy.amount, -amount2,
			                          iT.MoveBy.delay, 0.2f,
			                          iT.MoveBy.time, 0.2f,
			                          iT.MoveBy.oncomplete, "UpdatePosition"));
		}

		public IEnumerator Swap(CubeItem cube1, CubeItem cube2)
		{
			_field[cube1.Position.X, cube1.Position.Y] = cube2;
			_field[cube2.Position.X, cube2.Position.Y] = cube1;
			Position.Swap(cube1.Position, cube2.Position);

			iTween.Stop(cube1.gameObject);
			iTween.MoveBy(cube1.gameObject,
						  iTween.Hash(iT.MoveBy.amount, cube2.transform.localPosition - cube1.transform.localPosition,
			                          iT.MoveBy.time, 0.5f,
			                          iT.MoveBy.oncomplete, "UpdatePosition"));

			iTween.Stop(cube2.gameObject);
			iTween.MoveBy(cube2.gameObject,
						  iTween.Hash(iT.MoveBy.amount, cube1.transform.localPosition - cube2.transform.localPosition,
			                          iT.MoveBy.time, 0.5f,
			                          iT.MoveBy.oncomplete, "UpdatePosition"));

			yield return new WaitForSeconds(0.3f);
			CheckOnMatches();
		}

		private IEnumerator CompleteCube()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
				{
					if (_field[x, y] == null)
					{
						MoveUpColumn(x, y);
						CreateCube(x, y);
					}
				}

			yield return new WaitForSeconds(0.5f);
			CheckOnMatches();
		}

		private void MoveUpColumn(int x, int y)
		{
			//for (int z = 1; z < Z; z++)
			//{
			//	_field[x, y, z - 1] = _field[x, y, z];
			//	CubeItem cube = _field[x, y, z - 1];
			//	cube.SetPosition(x, y, z - 1);
			//	iTween.MoveBy(cube.gameObject,
			//				  iTween.Hash(iT.MoveBy.amount, new Vector3(0, 0, -CubeArea),
			//							  iT.MoveBy.time, 0.5f,
			//							  iT.MoveBy.oncomplete, "UpdatePosition"));
			//}
			//GameEvents.ColumnMovedUp.Publish(new PositionEventArgs(new Position(x, y)));
		}

		#endregion

		#region Top lair management...

		public IEnumerable<CubeItem> GetTopLairCubes()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					yield return _field[x, y];
		}

		private CubeItem[,] GetTopLair()
		{
			CubeItem[,] lair = new CubeItem[X, Y];
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					lair[x, y] = _field[x, y];
			return lair;
		}

		private IEnumerable<int[]> GetAvailableMoves()
		{
			CubeItem[,] field = GetTopLair();
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
				{
					if (x > 0)
					{
						Swap(field, x, y, x - 1, y);
						if (HasMatches(field))
							yield return new[] { x, y, x - 1, y };
						Swap(field, x, y, x - 1, y);
					}

					if (x < X - 1)
					{
						Swap(field, x, y, x + 1, y);
						if (HasMatches(field))
							yield return new[] { x, y, x + 1, y };
						Swap(field, x, y, x + 1, y);
					}

					if (y > 0)
					{
						Swap(field, x, y, x, y - 1);
						if (HasMatches(field))
							yield return new[] { x, y, x, y - 1 };
						Swap(field, x, y, x, y - 1);
					}

					if (y < Y - 1)
					{
						Swap(field, x, y, x, y + 1);
						if (HasMatches(field))
							yield return new[] { x, y, x, y + 1 };
						Swap(field, x, y, x, y + 1);
					}
				}
		}

		public bool HasAvailableMoves()
		{
			return GetAvailableMoves().Any();
		}

		private void Swap(CubeItem[,] field, int x1, int y1, int x2, int y2)
		{
			CubeItem temp = field[x1, y1];
			field[x1, y1] = field[x2, y2];
			field[x2, y2] = temp;
		}

		private IEnumerable<Match> GetMatches(CubeItem[,] field)
		{
			for (int y = 0; y < Y; y++)
			{
				for (int startX = 0; startX < X; startX++)
				{

					int endX = startX;
					while (endX + 1 < X && field[endX + 1, y].Value == field[startX, y].Value)
						endX++;

					int matchCount = endX - startX + 1;
					if (matchCount >= 3)
					{
						Match match = new Match();
						for (int x = startX; x <= endX; x++)
							match.Add(field[x, y]);
						yield return match;
					}
					startX = endX;
				}
			}

			for (int x = 0; x < X; x++)
			{
				for (int startY = 0; startY < Y; startY++)
				{
					int endY = startY;
					while (endY + 1 < Y && field[x, endY + 1].Value == field[x, startY].Value)
						endY++;

					int matchCount = endY - startY + 1;
					if (matchCount >= 3)
					{
						Match match = new Match();
						for (int y = startY; y <= endY; y++)
							match.Add(field[x, y]);
						yield return match;
					}
					startY = endY;
				}
			}
		}

		public bool HasMatches()
		{
			return HasMatches(GetTopLair());
		}

		private bool HasMatches(CubeItem[,] field)
		{
			return GetMatches(field).Any();
		}

		public void ClearTopLair()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
				{
					RemoveCube(_field[x, y]);
					//_field[x, y].IsQuestionBlocked = true;
				}
			StartCoroutine(CompleteCube());
			GameEvents.TopLairCleared.Publish(GameEventArgs.Empty);
		}

		public void ShowAvailableMove(float duration)
		{
			int[] availableMove = GetAvailableMoves().First();
			iTween.ShakePosition(_field[availableMove[0], availableMove[1]].gameObject,
								 iTween.Hash(iT.ShakePosition.amount, new Vector3(0.1f, 0.1f, 0),
											 iT.ShakePosition.time, duration,
											 iT.ShakePosition.oncomplete, "UpdatePosition"));

			iTween.ShakePosition(_field[availableMove[2], availableMove[3]].gameObject,
								 iTween.Hash(iT.ShakePosition.amount, new Vector3(0.1f, 0.1f, 0),
											 iT.ShakePosition.time, duration,
											 iT.ShakePosition.oncomplete, "UpdatePosition"));
		}

		public Vector2 GetCubePosition(int x, int y)
		{
			return _field[x, y].transform.position;
		}

		public CubeItem GetCube(int x, int y)
		{
			return _field[x, y];
		}

		#endregion

		public void UpdateAllCubesPosition()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					_field[x, y].UpdatePosition();
		}

		public bool Contains(Position position)
		{
			return position.X >= 0 && position.X < X && position.Y >= 0 && position.Y < Y;
		}

		public CubeItem GetCubeItem(Vector2 position)
		{
			return GetTopLairCubes().FirstOrDefault(cubeItem => cubeItem.Contains(position));
		}
	}
}

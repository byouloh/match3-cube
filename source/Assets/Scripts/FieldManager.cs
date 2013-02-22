using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.Levels;
using Assets.Scripts.ToastManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
	public class FieldManager : MonoBehaviourBase
	{
		private const string LEVEL_TOAST_STYLE = "CubeScoreStyleKey";

		private CubeItem _firstSelectedCube;
		private CubeItem _secondSelectedCube;
		private CubeItem[, ,] _field;
		private CubeItem _selectedCube;

		public Transform CubesHoster;
		public Transform CubePrefab;
		public Material[] Materials;
		public int X;
		public int Y;
		public int Z;
		public float CubeArea = 1.3f;
		public GUIStyle ToastStyle;
		public Material QuestionMaterial;
		public Material MoleMaterial;

		public static FieldManager Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
			GameEvents.Initalize();

			ToastManager.RegisterStyle(LEVEL_TOAST_STYLE,
									   new ToastStyle
									   {
										   Effect = Effect.Bubble,
										   Duration = 1f,
										   GUIStyle = ToastStyle
									   });
		}

		public void Start()
		{
			Random.seed = (int)(Time.realtimeSinceStartup * 1000000);
			StartGame();
		}

		public void Update()
		{
			if (_selectedCube != null)
			{
				SelectCube(_selectedCube);
				_selectedCube = null;
			}
		}

		public void StartGame()
		{
			_firstSelectedCube = null;
			_secondSelectedCube = null;
			ClearField();
			BuildField();
			LevelsManager.Reset();
			ScoreManager.Reset();
			TimerManager.Reset();
			TryBoomScript.Instance.Reset();
			CheckOnMatches();
			GameEvents.NewGameStarted.Publish(new GameEventArgs());
		}

		#region Build functions

		private void BuildField()
		{
			_field = new CubeItem[X, Y, Z];
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					for (int z = 0; z < Z; z++)
						CreateCube(x, y, z);
		}

		private void ClearField()
		{
			if (_field == null)
				return;

			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					for (int z = 0; z < Z; z++)
						RemoveCube(x, y, z);
		}

		private void CreateCube(int x, int y, int z)
		{
			Transform cube = (Transform)Instantiate(CubePrefab);
			CubeItem item = cube.GetComponent<CubeItem>();
			item.SetPosition(x, y, z);
			item.Material = NextMaterial();
			cube.parent = CubesHoster;
			item.UpdatePosition();
			_field[x, y, z] = item;
		}

		private Material NextMaterial()
		{
			int value = Random.Range(0, Materials.Length);
			return Materials[value];
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
				if (!IsFirstLevel(cube))
					return;

				if (_firstSelectedCube == null)
				{
					_firstSelectedCube = cube;
					MarkSelected(cube);
				}
				else
				{
					if (_firstSelectedCube == cube)
					{
						MarkUnSelected(cube);
						_firstSelectedCube = null;
					}
					else
					{
						if (!IsNeighbors(_firstSelectedCube, cube))
						{
							MarkUnSelected(_firstSelectedCube);
							MarkSelected(cube);
							_firstSelectedCube = cube;
						}
						else
						{
							_secondSelectedCube = cube;
							MarkSelected(cube);
							OnBothCubesSelected();
						}
					}
				}
			}
		}

		private bool CheckCubeSelection(CubeItem cube)
		{
			if (VirusesBehaviour.Instance.IsCubeInfected(new Position(cube.X, cube.Y)))
			{
				TimerManager.Instance.AddPenaltySeconds(-5);
				return false;
			}

			if (cube.IsQuestion && _firstSelectedCube == null)
			{
				cube.TransformToDefault();
				return false;
			}

			if (cube.IsMole)
			{
				TimerManager.Instance.AddPenaltySeconds(-5);
				return false;
			}

			return true;
		}

		private bool IsFirstLevel(CubeItem cube)
		{
			return cube.Z == 0;
		}

		private void MarkSelected(CubeItem cube)
		{
			iTween.ScaleTo(cube.gameObject, new Vector3(1.0f, 1.0f, 1.5f), 0.5f);
		}

		private void MarkUnSelected(CubeItem cube)
		{
			iTween.ScaleTo(cube.gameObject, new Vector3(1, 1, 1), 0.5f);
		}

		private bool IsNeighbors(CubeItem cube1, CubeItem cube2)
		{
			return cube1.Z == cube2.Z && (Mathf.Abs(cube1.X - cube2.X) + Mathf.Abs(cube1.Y - cube2.Y)) == 1;
		}

		private void OnBothCubesSelected()
		{
			if (IsNeighbors(_firstSelectedCube, _secondSelectedCube))
			{
				CubeItem[,] field = GetTopLair();
				Swap(field, _firstSelectedCube.X, _firstSelectedCube.Y, _secondSelectedCube.X, _secondSelectedCube.Y);
				if (HasMatches(field))
					StartCoroutine(Swap(_firstSelectedCube, _secondSelectedCube));
				else
					FailSwap(_firstSelectedCube, _secondSelectedCube);
			}

			MarkUnSelected(_firstSelectedCube);
			MarkUnSelected(_secondSelectedCube);
			_firstSelectedCube = null;
			_secondSelectedCube = null;
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
					RemoveCube(cubeItem.X, cubeItem.Y);

				AudioManager.Play(Sound.Match);
				StartCoroutine(CompleteCube());
				GameEvents.MatchesRemoved.Publish(new MatchesEventArgs(matches));
			}
			else
			{
				GameLocker.Unlock();
			}
		}

		private void RemoveCube(int x, int y)
		{
			RemoveCube(x, y, 0);
		}

		private void RemoveCube(int x, int y, int z)
		{
			GameObject cube = _field[x, y, z].gameObject;
			Vector3 runAwayDirectioin = new Vector3(
				Random.Range(-0.5f, 0.5f),
				Random.Range(-0.5f, 0.5f),
				Random.Range(-0.5f, -0.1f));

			iTween.Stop(cube);
			iTween.MoveTo(cube, runAwayDirectioin.normalized * 20, 5);
			Destroy(cube, 2);

			_field[x, y, z] = null;
			GameEvents.StateChanged.Publish(new GameEventArgs());
		}

		private void FailSwap(CubeItem cube1, CubeItem cube2)
		{
			iTween.MoveTo(cube1.gameObject, cube2.transform.position, 0.2f);
			iTween.MoveTo(cube1.gameObject,
						  iTween.Hash(iT.MoveTo.position, cube1.transform.position,
									  iT.MoveTo.delay, 0.2f,
									  iT.MoveTo.time, 0.2f,
									  iT.ShakePosition.oncomplete, "UpdatePosition"));

			iTween.MoveTo(cube2.gameObject, cube1.transform.position, 0.2f);
			iTween.MoveTo(cube2.gameObject,
						  iTween.Hash(iT.MoveTo.position, cube2.transform.position,
									  iT.MoveTo.delay, 0.2f,
									  iT.MoveTo.time, 0.2f,
									  iT.ShakePosition.oncomplete, "UpdatePosition"));
		}

		public IEnumerator Swap(CubeItem cube1, CubeItem cube2)
		{
			_field[cube1.X, cube1.Y, 0] = cube2;
			_field[cube2.X, cube2.Y, 0] = cube1;
			CubeItem.SwapPosition(cube1, cube2);

			iTween.Stop(cube1.gameObject);
			iTween.MoveTo(cube1.gameObject,
						  iTween.Hash(iT.MoveTo.position, cube2.transform.position,
									  iT.MoveTo.time, 0.5f,
									  iT.MoveTo.oncomplete, "UpdatePosition"));

			iTween.Stop(cube2.gameObject);
			iTween.MoveTo(cube2.gameObject,
						  iTween.Hash(iT.MoveTo.position, cube1.transform.position,
									  iT.MoveTo.time, 0.5f,
									  iT.MoveTo.oncomplete, "UpdatePosition"));

			yield return new WaitForSeconds(0.3f);
			CheckOnMatches();
		}

		private IEnumerator CompleteCube()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
				{
					if (_field[x, y, 0] == null)
					{
						MoveUpColumn(x, y);
						CreateCube(x, y, Z - 1);
					}
				}

			yield return new WaitForSeconds(0.5f);
			CheckOnMatches();
		}

		private void MoveUpColumn(int x, int y)
		{
			for (int z = 1; z < Z; z++)
			{
				_field[x, y, z - 1] = _field[x, y, z];
				CubeItem cube = _field[x, y, z - 1];
				cube.SetPosition(x, y, z - 1);
				iTween.MoveTo(cube.gameObject,
							  iTween.Hash(iT.MoveTo.position,
										  new Vector3(cube.transform.position.x, cube.transform.position.y,
													  cube.transform.position.z - CubeArea),
										  iT.MoveTo.time, 0.5f,
										  iT.MoveTo.oncomplete, "UpdatePosition"));
			}
			GameEvents.ColumnMovedUp.Publish(new PositionEventArgs(new Position(x, y)));
		}

		#endregion

		#region Top lair management...

		public IEnumerable<CubeItem> GetTopLairCubes()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					yield return _field[x, y, 0];
		}

		private CubeItem[,] GetTopLair()
		{
			CubeItem[,] lair = new CubeItem[X, Y];
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					lair[x, y] = _field[x, y, 0];
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

		private bool HasMatches(CubeItem[,] field)
		{
			return GetMatches(field).Any();
		}

		public void ClearTopLair()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
				{
					RemoveCube(x, y);
					_field[x, y, 1].IsQuestionBlocked = true;
				}
			StartCoroutine(CompleteCube());
			GameEvents.TopLairCleared.Publish(new GameEventArgs());
		}

		public void ShowAvailableMove(float duration)
		{
			int[] availableMove = GetAvailableMoves().First();
			iTween.ShakePosition(_field[availableMove[0], availableMove[1], 0].gameObject,
								 iTween.Hash(iT.ShakePosition.amount, new Vector3(0.1f, 0.1f, 0),
											 iT.ShakePosition.time, duration,
											 iT.ShakePosition.oncomplete, "UpdatePosition"));

			iTween.ShakePosition(_field[availableMove[2], availableMove[3], 0].gameObject,
								 iTween.Hash(iT.ShakePosition.amount, new Vector3(0.1f, 0.1f, 0),
											 iT.ShakePosition.time, duration,
											 iT.ShakePosition.oncomplete, "UpdatePosition"));
		}

		public Vector2 GetCubePosition(int x, int y)
		{
			return _field[x, y, 0].transform.position;
		}

		public CubeItem GetCube(int x, int y)
		{
			return _field[x, y, 0];
		}

		#endregion

		#region Turns

		public void RotateUp()
		{

		}

		public void RotateDown()
		{

		}

		public void RotateLeft()
		{

		}

		public void RotateRight()
		{

		}

		#endregion

		public void UpdateAllCubesPosition()
		{
			for (int x = 0; x < X; x++)
				for (int y = 0; y < Y; y++)
					for (int z = 0; z < Z; z++)
						_field[x, y, z].UpdatePosition();
		}
	}
}

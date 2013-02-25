using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts
{
	public class Analytics : MonoBehaviourBase
	{
		public int GameId;
		public string GUID;
		public string APIKey;

		public void Start()
		{
			Playtomic.Initialize(GameId, GUID, APIKey);
			Playtomic.Log.View();
			Playtomic.Log.ForceSend();

			//Playtomic.Log.Play();

			//StartCoroutine(LoadGameVars());

			//GameEvents.NextLevel.Subscribe(OnNextLevel);
		}

		private void OnNextLevel(GameEventArgs<Level> gameEventArgs)
		{
			Playtomic.Log.LevelCounterMetric("NextLevel", gameEventArgs.Object.Number);
			Playtomic.Log.ForceSend();
		}

		private IEnumerator LoadGameVars()
		{
			yield return StartCoroutine(Playtomic.GameVars.Load());
			var response = Playtomic.GameVars.GetResponse("Load");

			if (response.Success)
			{
				Debug.Log("GameVars are loaded!");

				foreach (var key in response.Data.Keys)
				{
					Debug.Log("GameVar '" + key + "' = '" + response.Data[key]);
				}

				// alternatively
				// response.GetValue("varname");
			}
			else
			{
				Debug.Log("GameVars failed because of " + response.ErrorCode + ": " + response.ErrorDescription);
			}

			yield return 0;
		}
	}
}

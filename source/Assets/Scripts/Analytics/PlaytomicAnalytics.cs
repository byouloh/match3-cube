using Assets.Scripts.Common;
using Assets.Scripts.Events;

namespace Assets.Scripts.Analytics
{
	public class PlaytomicAnalytics : MonoBehaviourBase
	{
		public int GameId;
		public string GameGuid;
		public string ApiKey;

		public void Awake()
		{
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);

			Playtomic.Initialize(GameId, GameGuid, ApiKey);
		}

		public void Start()
		{
			Playtomic.Log.View();
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			Playtomic.Log.Play();
		}
	}
}

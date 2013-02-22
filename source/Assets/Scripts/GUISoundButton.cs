using UnityEngine;

namespace Assets.Scripts
{
	public class GUISoundButton : MonoBehaviour 
	{
		void OnGUI()
		{
			if(GUI.Button(new Rect(0, Screen.height - 80, 50, 40), "Sound"))
			{
				AudioManager.SoundSwitchOnOff();
			}

			if (GUI.Button(new Rect(60, Screen.height - 80, 50, 40), "Music"))
			{
				AudioManager.MusicSwitchOnOff();
			}


		}

	}
}

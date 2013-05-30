using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class ButtonHandler : MonoBehaviourBase
	{
		private static readonly List<ButtonHandler> _buttons = new List<ButtonHandler>(); 

		public event EventHandler Click;

		public void Awake()
		{
			_buttons.Add(this);
		}

		public void OnClick()
		{
			if (Click != null)
				Click(this, EventArgs.Empty);
		}

		public static void BlockButtons()
		{
			foreach (ButtonHandler buttonHandler in _buttons)
			{
				buttonHandler.GetComponent<BoxCollider>().enabled = false;
			}
		}

		public static void UnblockButtons()
		{
			foreach (ButtonHandler buttonHandler in _buttons)
				UnblockButton(buttonHandler);
		}

		public static void UnblockButton(ButtonHandler buttonHandler)
		{
			buttonHandler.GetComponent<BoxCollider>().enabled = true;
		}
	}
}

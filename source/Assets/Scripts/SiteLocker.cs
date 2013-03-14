using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts
{
	public class SiteLocker : MonoBehaviourBase
	{
		public string LockUrl;
		public string TempUrl1;
		public string TempUrl2;

		public void Start()
		{
			if (Application.isWebPlayer)
			{
				string url = Application.absoluteURL;
				if (string.IsNullOrEmpty(url))
				{
					NavigationToLockUrl();
				}
				else
				{
					Uri uri = new Uri(url);
					if (uri.Host.ToLower() != LockUrl.ToLower())
						NavigationToLockUrl();
				}
			}
		}

		private void NavigationToLockUrl()
		{
			//string javaScript = "if(document.location.host != '" + LockUrl + "') { document.location='" + LockUrl + "'; }";
			string javaScript = string.Format("document.location='http://{0}';", LockUrl);
			Application.ExternalEval(javaScript);
		}
	}
}

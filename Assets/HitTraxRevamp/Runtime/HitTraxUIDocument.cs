using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HitTrax.Assets.HitTraxRevamp
{
	public class HitTraxUIDocument : MonoBehaviour
	{
		protected UIDocument mainDocument;
		protected PanelSettings panelSettings;

		protected virtual void OnEnable()
		{
			mainDocument = GetComponent<UIDocument>();
			panelSettings = mainDocument.panelSettings;
		}
		protected virtual void OnDestroy()
		{
			Resources.UnloadAsset(panelSettings.themeStyleSheet);
			Resources.UnloadUnusedAssets();
		}

		//protected T QueryFindElement<T>(string name) where T : VisualElement
		//{
		//	return rootDocument.rootVisualElement.Q<T>(name);
		//}
	}
}
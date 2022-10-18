using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace FeatureA
{
    public class FeatureARuntimeManager
	{
		public static bool IsInitialized;

		// Example Events
		public Action EventA;
		public Action EventB;
		public Action EventC;
		public Action EventD;


		public FeatureARuntimeManager()
		{
			IsInitialized = false;
		}

		public static void Initialize()
		{
			IsInitialized = true;
			UnityEngine.Debug.Log("Init Feature");

			Addressables.InstantiateAsync("Assets/HitTraxRevamp/FeatureA/Assets/FeatureUIManager.prefab")
				.Completed += (operation) =>
				{
					LoadFeatureUI();
				};
			
			// SomeScript1.Init();
			// SomeScript2.Init();
			// SomeScript3.Init();
			// SomeScript4.Init();
		}

		public static void Close()
		{		
			IsInitialized = false;
			UnityEngine.Debug.Log("Close Feature");

			foreach (var handle in FeatureAUIManager.Instance.addressableHandles)
			{
				Addressables.Release(handle);
				Debug.Log("Released" + handle.DebugName);
			}

			// SomeScript1.Close();
			// SomeScript2.Close();
			// SomeScript3.Close();
			// SomeScript4.Close();
		}

		/// <summary>
		/// Loading the base UI
		/// Instead of living here, this would likely be run in another script's Init() function
		/// </summary>
		private static void LoadFeatureUI()
		{
			Addressables.InstantiateAsync("Assets/HitTraxRevamp/FeatureA/Assets/FeatureAUI.prefab")
				.Completed += (operation) =>
				{
					operation.Result.transform.SetParent(HitTrax.Assets.HitTraxRevamp.MainExecutableUI.Instance.transform.parent);
					operation.Result.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					operation.Result.transform.SetAsLastSibling();
					FeatureAUIManager.Instance.addressableHandles.Add(operation);
				};
		
			//UXML_Manager.AddImageToTree("runtime-image-bataroundLogo", "bataround-logo", true);
		}
	}
}

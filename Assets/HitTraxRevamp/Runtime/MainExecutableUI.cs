using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using System.Reflection;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HitTrax.Assets.HitTraxRevamp
{
    public class MainExecutableUI : MonoBehaviourSingleton<MainExecutableUI>
    {
		public Button launchFeatureA;
		public GameObject loadingAnim;
		private AppDomain featureDomain;
		private Assembly currentAssembly;

		protected override void SingletonStarted()
		{
			base.SingletonStarted();
			launchFeatureA.onClick.RemoveAllListeners();
			launchFeatureA.onClick.AddListener(OnFeatureAClick);		
		}

		protected override void SingletonDestroyed()
		{
			base.SingletonDestroyed();

			if (featureDomain != null && currentAssembly != null)
			{
				Debug.Log("Unload");
				UnloadFeature(currentAssembly);
				UnloadDomain(featureDomain);
			}	
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		private void OnFeatureAClick()
		{
			StartCoroutine(Co_OnFeatureAClick());
		}

		private IEnumerator Co_OnFeatureAClick()
		{
			// Done in a coroutine just to show a loading animation for demo purposes
			loadingAnim.SetActive(true);
			yield return new WaitForSeconds(2f);

			Addressables.InitializeAsync().Completed += (operation) =>
			{
				LoadFeatureDLL("Assets/HitTraxRevamp/FeatureA/FeatureA.txt").Completed += (operation) =>
				{
					featureDomain = AppDomain.CreateDomain("FeatureDomain");
					currentAssembly = featureDomain.Load(operation.Result.bytes);
					if (!LoadFeature(currentAssembly))
					{
						Debug.LogError("Failed to load feature bundles");
					}
				};
			};
		}

		private AsyncOperationHandle<TextAsset> LoadFeatureDLL(string path)
		{
			return Addressables.LoadAssetAsync<TextAsset>(path);
		}

		private void UnloadDomain(AppDomain domain)
		{
			AppDomain.Unload(domain);
		}

		private bool LoadFeature(Assembly assembly)
		{
			// Grab the Feature Loader class and call Load() to initialize the feature how we see fit
			var assemblyType = assembly.GetType("FeatureA.FeatureLoader");
			var assemblyObject = Activator.CreateInstance(assemblyType);
			assemblyType.GetMethod("Load").Invoke(assemblyObject, null);
			loadingAnim.SetActive(false);
			return true;
		}

		private bool UnloadFeature(Assembly assembly)
		{		
			// Grab the Feature Loader class and call Unload() to close the feature how we see fit
			var assemblyType = assembly.GetType("FeatureA.FeatureLoader");
			var assemblyObject = Activator.CreateInstance(assemblyType);
			assemblyType.GetMethod("Unload").Invoke(assemblyObject, null);
			return true;
		}
	}
}

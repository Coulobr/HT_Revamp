using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Reflection;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace HitTrax.Assets.HitTraxRevamp
{
    public class TestDocumentController : HitTraxUIDocument
    {
		private const string UxmlDocumentName = "TestUIDoc";
		private const string FeatureA_DLLs = "Assets/HitTraxRevamp/Feature1DLLs/";
		private const string FeatureA_Assets = "Assets/HitTraxRevamp/Feature1DLLs/";

		private VisualElement buttonContainer;
		private VisualElement titleContainer;
		private GroupBox background;
        private DropdownField dropdownField;
		private Button launchButton;

		private Assembly featureA_Assembly;
		private IEnumerator catalogUpdateRoutine = null;

		float updateCheckTimer = 0;
		float updateCheckInterval = 10;

		public string currentDLL = "";
		public string prevDLL = "";

		protected override void OnEnable()
		{
			base.OnEnable();

			//Addressables.InitializeAsync();
			//mainDocument.visualTreeAsset = Resources.Load<VisualTreeAsset>(UxmlDocumentName);
			//LoadDocument();
			//LoadBaseSelectors();
		}

		private void Update()
		{
			updateCheckTimer += Time.deltaTime;
			if (updateCheckTimer > updateCheckInterval)
			{
				updateCheckTimer = 0;

				if (catalogUpdateRoutine == null)
				{
					//Debug.Log("Checking for Updates");
					//catalogUpdateRoutine = Co_UpdateCatalogs();
					//StartCoroutine(catalogUpdateRoutine);
				}
			}

			if (Input.GetKeyDown(KeyCode.F1))
			{
				//ManualDomainReload();
			}
		}

		private void ManualDomainReload()
		{
			switch (currentDLL)
			{
				case "FeatureA_Patch.txt":
					currentDLL = "FeatureA.txt";
					prevDLL = "FeatureA_Patch.txt";
					print("Updated DLL to " + currentDLL + " from " + prevDLL);
					break;
				case "FeatureA.txt":
					currentDLL = "FeatureA_Patch.txt";
					prevDLL = "FeatureA.txt";
					print("Updated DLL to " + currentDLL + " from " + prevDLL);
					break;
				default:
					currentDLL = "FeatureA_Patch.txt";
					prevDLL = "FeatureA.txt";
					print("Updated DLL to " + currentDLL + " from " + prevDLL);
					break;
			}
		}

		private void OnStartFeatureAClicked()
		{
			//if (catalogUpdateRoutine != null)
			//{
			//	Debug.Log("Catalog is still updating");
			//	return;
			//}

			if (currentDLL == "")
			{
				currentDLL = "FeatureA.txt";
			}

			//LoadDll_ManualSwap(currentDLL);
			InitDLL(currentDLL);
		}

		private IEnumerator Co_UpdateCatalogs()
		{
			List<string> catalogsToUpdate = new List<string>();
			AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();

			checkForUpdateHandle.Completed += op =>
			{
				catalogsToUpdate.AddRange(op.Result);
			};

			yield return checkForUpdateHandle;

			if (catalogsToUpdate.Count > 0)
			{
				Debug.Log($"Updating {catalogsToUpdate.Count} catalogs");
				AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, true);
				yield return updateHandle;
			}

			Debug.Log($"Catalogs up to date");

			catalogUpdateRoutine = null;
		}

		void InitDLL(string dllTextAssetName)
		{
			if (featureA_Assembly == null)
			{
				//print("'featureA_Assembly' is null, Loading dll");

				var loadDllTextHandle = Addressables.LoadAssetAsync<TextAsset>(FeatureA_DLLs + dllTextAssetName);
				loadDllTextHandle.Completed += (operation) =>
				{
					featureA_Assembly = Assembly.Load(loadDllTextHandle.Result.bytes);

					// Grab the Feature Loader class and call Run() to initialize the feature how we see fit
					var assemblyType = featureA_Assembly.GetType("FeatureA.FeatureLoader");
					var assemblyObject = Activator.CreateInstance(assemblyType);
					var method = assemblyType.GetMethod("Run").Invoke(assemblyObject, null);
				};
			}
		}

		void LoadDll_ManualSwap(string dllTextAssetName)
		{
			if (featureA_Assembly == null)
			{
				//print("'featureA_Assembly' is null, Loading dll");

				var loadDllTextHandle = Addressables.LoadAssetAsync<TextAsset>(FeatureA_DLLs + dllTextAssetName);
				loadDllTextHandle.Completed += (operation) =>
				{
					featureA_Assembly = Assembly.Load(loadDllTextHandle.Result.bytes);

					// Grab the Feature Loader class and call Run() to initialize the feature how we see fit
					var assemblyType = featureA_Assembly.GetType("FeatureA.FeatureLoader");
					var assemblyObject = Activator.CreateInstance(assemblyType);
					var method = assemblyType.GetMethod("Run").Invoke(assemblyObject, null);
				};
			}
			else if (prevDLL != currentDLL)
			{
				//print($"param name and assembly name are not the same. Param:{dllTextAssetName} | assembly name: {currentDLL}");

				var dllToUnloadHandle = Addressables.LoadAssetAsync<TextAsset>(FeatureA_DLLs + prevDLL);
				dllToUnloadHandle.Completed += (operation) =>
				{
					featureA_Assembly = Assembly.Load(dllToUnloadHandle.Result.bytes);
					var assemblyType = featureA_Assembly.GetType("FeatureA.FeatureLoader");
					var assemblyObject = Activator.CreateInstance(assemblyType);
					var method = assemblyType.GetMethod("Unload").Invoke(assemblyObject, null);

					var loadDllTextHandle = Addressables.LoadAssetAsync<TextAsset>(FeatureA_DLLs + currentDLL);
					loadDllTextHandle.Completed += (operation) =>
					{
						featureA_Assembly = Assembly.Load(loadDllTextHandle.Result.bytes);
						var assemblyType = featureA_Assembly.GetType("FeatureA.FeatureLoader");
						var assemblyObject = Activator.CreateInstance(assemblyType);
						var method = assemblyType.GetMethod("Run").Invoke(assemblyObject, null);
					};
				};		
			}
		}
		
		private void OnDropdownChanged(string newValue)
		{
			switch (newValue)
			{
				case "SkinA":
					LoadSkinA();
					break;
				case "SkinB":
					LoadSkinB();
					break;
			}
		}

		private void LoadDocument()
		{
			buttonContainer = mainDocument.rootVisualElement.Q<VisualElement>("button-container");
			titleContainer = mainDocument.rootVisualElement.Q<VisualElement>("title-container");
			dropdownField = mainDocument.rootVisualElement.Q<DropdownField>("dropdown-field");
			launchButton = buttonContainer.Q<Button>("launch-button");
			background = mainDocument.rootVisualElement.Q<GroupBox>("background");

			if (launchButton != null)
			{
				launchButton.RegisterCallback<ClickEvent>(evnt => OnStartFeatureAClicked());
			}

			if (dropdownField != null)
			{
				dropdownField.choices = new List<string> { "SkinA", "SkinB" };
				dropdownField.RegisterValueChangedCallback(evnt => OnDropdownChanged(evnt.newValue));
			}
		}
		
		private void LoadBaseSelectors()
		{
			if (!buttonContainer.ClassListContains("ht-button"))
			{
				buttonContainer.AddToClassList("ht-button");
				buttonContainer.AddToClassList("unity-button");
			}

			if (!titleContainer.ClassListContains("ht-title-label"))
			{
				titleContainer.AddToClassList("ht-title-label");
				titleContainer.AddToClassList("unity-label");
			}

			if (!dropdownField.ClassListContains("ht-parent-dropdown"))
			{
				dropdownField.AddToClassList("ht-parent-dropdown");
				dropdownField.AddToClassList("unity-base-field");
			}

			if (!background.ClassListContains("ht-background"))
			{
				background.AddToClassList("ht-background");
			}
		}

		private void LoadSkinA()
		{
			panelSettings.themeStyleSheet = Resources.Load<ThemeStyleSheet>("Theme1");
			mainDocument.rootVisualElement.styleSheets.Clear();
			mainDocument.rootVisualElement.styleSheets.Add(panelSettings.themeStyleSheet);
			mainDocument.rootVisualElement.MarkDirtyRepaint();
		}

		private void LoadSkinB()
		{
			panelSettings.themeStyleSheet = Resources.Load<ThemeStyleSheet>("Theme2");
			mainDocument.rootVisualElement.styleSheets.Clear();
			mainDocument.rootVisualElement.styleSheets.Add(panelSettings.themeStyleSheet);
			mainDocument.rootVisualElement.MarkDirtyRepaint();
		}
	}
}

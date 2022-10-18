using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;
using System;

namespace FeatureA
{
    public class FeatureAUI : MonoBehaviour
    {
		public Button continueButton;
		public TMP_Dropdown skinDropdown;
		public TextMeshProUGUI title;
		public Image background;
		public Image backgroundImage;
		public GameObject mainContainer;
		
		
		public GameObject codeUpdate;

		public SkinData skinA;
		public SkinData skinB;

		private void Start()
		{
			skinDropdown.onValueChanged.RemoveAllListeners();
			skinDropdown.onValueChanged.AddListener(OnSkinChanged);
			OnSkinChanged(0);
			codeUpdate.SetActive(true);
		}

		[ContextMenu("ChangeToSkinA")]
		public void ChangeToSkinA()
		{
			var skinALoad = Addressables.LoadAssetAsync<SkinData>("Assets/HitTraxRevamp/FeatureA/Assets/SkinA.asset");
			skinALoad.Completed += (op) => {
				skinA = skinALoad.Result;

				background.color = skinA.backgroundColor;
				backgroundImage.sprite = skinA.backgroundSprite;
				backgroundImage.preserveAspect = true;

				continueButton.targetGraphic.color = skinA.buttonColor;
				continueButton.GetComponentInChildren<TextMeshProUGUI>().color = skinA.textColor;

				title.color = skinA.textColor;

				skinDropdown.captionText.color = skinA.textColor;
				skinDropdown.itemText.color = skinA.textColor;
				skinDropdown.targetGraphic.color = skinA.buttonColor;
				
				HitTrax.Assets.HitTraxRevamp.MainExecutableUI.Instance.Close();
			};
		}

		[ContextMenu("ChangeToSkinB")]
		public void ChangeToSkinB()
		{
			var skinBLoad = Addressables.LoadAssetAsync<SkinData>("Assets/HitTraxRevamp/FeatureA/Assets/SkinB.asset");
			skinBLoad.Completed += (op) =>
			{
				skinB = skinBLoad.Result;

				background.color = skinB.backgroundColor;
				backgroundImage.sprite = skinB.backgroundSprite;
				backgroundImage.preserveAspect = true;

				continueButton.targetGraphic.color = skinB.buttonColor;
				continueButton.GetComponentInChildren<TextMeshProUGUI>().color = skinB.textColor;

				title.color = skinB.textColor;

				skinDropdown.captionText.color = skinB.textColor;
				skinDropdown.itemText.color = skinB.textColor;
				skinDropdown.targetGraphic.color = skinB.buttonColor;

				HitTrax.Assets.HitTraxRevamp.MainExecutableUI.Instance.Close();
			};
		}

		private void OnSkinChanged(int val)
		{
			switch (val)
			{
				default:
				case 0:
					var skinALoad = Addressables.LoadAssetAsync<SkinData>("Assets/HitTraxRevamp/FeatureA/Assets/SkinA.asset");
					skinALoad.Completed += (op) => {
						skinA = skinALoad.Result;

						background.color = skinA.backgroundColor;
						backgroundImage.sprite = skinA.backgroundSprite;
						backgroundImage.preserveAspect = true;

						continueButton.targetGraphic.color = skinA.buttonColor;
						continueButton.GetComponentInChildren<TextMeshProUGUI>().color = skinA.textColor;

						title.color = skinA.textColor;

						skinDropdown.captionText.color = skinA.textColor;
						skinDropdown.itemText.color = skinA.textColor;
						skinDropdown.targetGraphic.color = skinA.buttonColor;
						skinDropdown.template.GetComponent<Image>().color = skinA.buttonColor;
						skinDropdown.itemText.color = skinA.textColor;

						mainContainer.SetActive(true);
						HitTrax.Assets.HitTraxRevamp.MainExecutableUI.Instance.Close();
					};

					FeatureAUIManager.Instance.addressableHandles.Add(skinALoad);
					break;
				case 1:
					var skinBLoad = Addressables.LoadAssetAsync<SkinData>("Assets/HitTraxRevamp/FeatureA/Assets/SkinB.asset");
					skinBLoad.Completed += (op) =>
					{
						skinB = skinBLoad.Result;

						background.color = skinB.backgroundColor;
						backgroundImage.sprite = skinB.backgroundSprite;
						backgroundImage.preserveAspect = true;

						continueButton.targetGraphic.color = skinB.buttonColor;
						continueButton.GetComponentInChildren<TextMeshProUGUI>().color = skinB.textColor;

						title.color = skinB.textColor;

						skinDropdown.captionText.color = skinB.textColor;
						skinDropdown.itemText.color = skinB.textColor;
						skinDropdown.targetGraphic.color = skinB.buttonColor;
						skinDropdown.template.GetComponent<Image>().color = skinB.buttonColor;
						skinDropdown.itemText.color = skinB.textColor;

						mainContainer.SetActive(true);
						HitTrax.Assets.HitTraxRevamp.MainExecutableUI.Instance.Close();
					};

					FeatureAUIManager.Instance.addressableHandles.Add(skinBLoad);
					break;
			}
		}
	}
}

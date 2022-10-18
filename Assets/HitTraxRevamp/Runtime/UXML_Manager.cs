using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
 
namespace HitTrax.Assets.HitTraxRevamp
{
    public class UXML_Manager : MonoBehaviour
	{
		public static UXML_Manager Instance;

		void Awake()
		{

			if (Instance == null)
			{

				Instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(this);
			}
		}

		public UIDocument mainDocument;
		private HitTraxUIDocument activeDocumentController;

		private void OnEnable()
		{
			//LoadDocument<TestDocumentController>();
		}

		public void LoadDocument<T>() where T : HitTraxUIDocument
		{
			if (activeDocumentController != null)
			{
				DestroyImmediate(activeDocumentController);
			}

			activeDocumentController = gameObject.AddComponent<T>();
		}

		public static void AddImageToTree(string className, string elementName, bool bringToFront = true)
		{
			//var image = new Image();
			//Instance.mainDocument.rootVisualElement.Add(image);
			//image.AddToClassList(className);
			//image.name = elementName;
			//if (bringToFront)
			//{
			//	image.BringToFront();
			//}
		}

		public static void RemoveImageFromTree(string elementName)
		{
			var locatedElement = Instance.mainDocument.rootVisualElement.Q<Image>(elementName);
			locatedElement.RemoveFromHierarchy();
		}
	}
}

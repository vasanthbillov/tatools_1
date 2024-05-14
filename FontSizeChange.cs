// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// public class FontSizeChange : MonoBehaviour
// {
// 	private static FFClass _ffClass;
// 	[MenuItem("Tools/IncreaseFontSize",false, 0)]
// 	public static void ShowSetDepthWindow()
// 	{
// 		if (Selection.activeGameObject != null)
// 		{
// 			_ffClass.IncreaseFontSizeForAllChild(Selection.activeGameObject);
// 		}
// 	}
// }
//
// public class FFClass
// {
// 	public void IncreaseFontSizeForAllChild(GameObject obj)
// 	{
// 		IncreaseFontSize(obj);
// 		for (int i = 0; i < obj.transform.childCount; i++)
// 		{
// 			IncreaseFontSizeForAllChild(obj.transform.GetChild(i).gameObject);
// 		}
// 	}
// 	
// 	private void IncreaseFontSize(GameObject obj)
// 	{
// 		UILabel label = obj.GetComponent<UILabel>();
//
// 		if (label != null)
// 		{
// 			label.fontSize = label.fontSize * 3;
// 		}
// 	}
// }
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FontSizeChange : MonoBehaviour
{
	private static FFClass _ffClass;

	[MenuItem("Tools/IncreaseFontSize", false, 0)]
	public static void IncreaseFontSize()
	{
		if (_ffClass == null)
		{
			_ffClass = new FFClass(); // Initialize the FFClass instance
		}

		if (Selection.activeGameObject != null)
		{
			_ffClass.IncreaseFontSizeForAllChild(Selection.activeGameObject);
		}
	}
	
	[MenuItem("Tools/ChangeFontCrispiness", false, 0)]
	public static void ChangeFontCrispiness()
	{
		if (_ffClass == null)
		{
			_ffClass = new FFClass(); // Initialize the FFClass instance
		}

		if (Selection.activeGameObject != null)
		{
			_ffClass.ChangeCrispnessForAllChild(Selection.activeGameObject);
		}
	}
}

public class FFClass
{
	public void IncreaseFontSizeForAllChild(GameObject obj)
	{
		IncreaseFontSize(obj);
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			IncreaseFontSizeForAllChild(obj.transform.GetChild(i).gameObject);
		}
	}

	private void IncreaseFontSize(GameObject obj)
	{
		UILabel label = obj.GetComponent<UILabel>();

		if (label != null)
		{
			label.fontSize = label.fontSize * 5;
		}
	}
	
	public void ChangeCrispnessForAllChild(GameObject obj)
	{
		ChangeCrispness(obj);
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			ChangeCrispnessForAllChild(obj.transform.GetChild(i).gameObject);
		}
	}
	
	private void ChangeCrispness(GameObject obj)
	{
		UILabel label = obj.GetComponent<UILabel>();

		if (label != null)
		{
			label.keepCrispWhenShrunk = UILabel.Crispness.Always;
		}
	}
}

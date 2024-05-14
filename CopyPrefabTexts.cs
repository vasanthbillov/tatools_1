
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public static class CopyPrefabText
{
	static List<string> keyContainer=new List<string>();

	[MenuItem("Assets/Copy Localize Text")]
	public static void CopylocalizeText()
	{

		var selectedPrefabPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
		if (selectedPrefabPath.Contains(".prefab"))
		{
			CopylocalizeText(AssetDatabase.LoadAssetAtPath<GameObject>(selectedPrefabPath));
			
		}
		else
		{
			//Show Error
		}
	}

	private static void CopylocalizeText(GameObject prefab)
	{
		//Debug.Log("Prefab Name :: "+prefab.name + "Number of Children :: "+ prefab.transform.childCount);
		Traverse(prefab);
		string CombinedText="";
		for (int i = 0; i < keyContainer.Count; i++)
		{
			CombinedText = CombinedText + keyContainer[i] + "\n";
		}
		UnityEngine.Debug.Log(CombinedText.ToString());
		GUIUtility.systemCopyBuffer = CombinedText.ToString();
		
	
	}

	static void Traverse(GameObject obj)
	{
		//Debug.Log(obj.name);
		if (obj.GetComponent<SVLocLabel>()==true)
		{
			//Debug.Log(obj.GetComponent<SVLocLabel>().LocKey +"="+ obj.GetComponent<UILabel>().text);
			keyContainer.Add(obj.GetComponent<SVLocLabel>().LocKey + "=" + obj.GetComponent<UILabel>().text);
		}
		foreach (Transform child in obj.transform)
		{
			Traverse(child.gameObject);
		}

	}
}

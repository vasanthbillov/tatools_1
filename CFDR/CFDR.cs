
//Credit: Vasanth Kumar,Concept Artist,Zynga
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ReconnectorLib;

public class CFDR : EditorWindow {

	[MenuItem("zButton/Dependency Reconnector")]
	public static void Connector()
	{
		EditorWindow.GetWindow<CFDR> (true, "~ DEPENDENCY RECONNECTOR ~");
	}
	private static string originalFolder ;
	private static string duplicatedFolder;
	private static string duplicatedFolderPath;
	private static string guID = "guid: ";
	private static string animator = ".controller";
	private static string material = ".mat";
	private static string originalFolderRef ;
	private static string originalFolderPath;
	private static List<string> pathList = new List<string> ();

	void OnGUI (){
		GUILayout.Label ("Select Original Folder In Assets Panel", EditorStyles.boldLabel);
		if (GUILayout.Button ("Select Original Folder", GUILayout.Width (300f))) {
				originalFolderPath = GetSelectedPath();
			if (FindFolderName (originalFolderPath) != null) {
				originalFolder = FindFolderName (originalFolderPath);
				originalFolderRef = originalFolder;
			} else {
				EditorUtility.DisplayDialog ("ERROR", "Select Original Folder Properly", "OK");
				Debug.LogError ("Select Original Folder Properly");
			}
		}
		if (originalFolder != null) {
			originalFolder = EditorGUILayout.TextField ("Original Folder Name", originalFolder);
		}
		GUILayout.Space (20f);
		GUILayout.Label ("Select Duplicated Folder In Assets Panel", EditorStyles.boldLabel);
		if (GUILayout.Button ("Select Duplicated Folder", GUILayout.Width (300f))) {
			duplicatedFolderPath = GetSelectedPath();
			if (FindFolderName (duplicatedFolderPath) != null) {
				duplicatedFolder = FindFolderName (duplicatedFolderPath);
			} else {
				EditorUtility.DisplayDialog ("ERROR", "Select Duplicated Folder Properly", "OK");
				Debug.LogError ("Select Duplicated Folder Properly");
			}
		}
		if (duplicatedFolder != null) {
			duplicatedFolder = EditorGUILayout.TextField("Duplicated Folder Name",duplicatedFolder) ;
		}
		GUILayout.Space (30f);
		if (GUILayout.Button ("RECONNECT DEPENDENCIES", GUILayout.Width (300f))) {
			
			if (originalFolder != null) {
				if (duplicatedFolder != null) {
					if (originalFolderPath != duplicatedFolderPath) {
						ReconnectDependency ();
					} else {
						EditorUtility.DisplayDialog ("ERROR", "Both Folders should be different", "OK");
					}

					EditorGUIUtility.editingTextField = false;
					originalFolder = null;
					duplicatedFolder = null;
				} else {
					EditorUtility.DisplayDialog ("ERROR", "Select Duplicated Folder Properly", "OK");
					Debug.LogError ("Select Duplicated Folder Properly");
				}
			}else {
				EditorUtility.DisplayDialog ("ERROR", "Select Original Folder Properly", "OK");
				Debug.LogError ("Select Original Folder Properly");
			}
		}
		GUILayout.Space (20f);
		if (GUILayout.Button ("RESET", GUILayout.Width (300f))) {
			EditorGUIUtility.editingTextField = false;
			originalFolder = null;
			duplicatedFolder = null;
		}
	}
	private static void ReconnectDependency () {
		bool isReconnected = false;
		string[] guids = FindPrefabs (duplicatedFolderPath);
		for (int k = 0; k < guids.Length; k++) {
			if (originalFolder != duplicatedFolder) {
				string path = GetPath (guids[k]);
				Replacer (path);
				isReconnected = true;
			} else {
				isReconnected = false;
			}
		}
		if (isReconnected) {
			EditorUtility.DisplayDialog ("RESULT", " All the dependencies are reconnected to "+ duplicatedFolder+ " folder", "OK");
			Debug.Log (" ALL THE DEPENDENCIES ARE RECONNECTED TO "+ duplicatedFolder);
			pathList.Clear ();
			AssetDatabase.Refresh ();
		} else {
			EditorUtility.DisplayDialog ("RESULT", "There is no dependency from "+originalFolderRef+" folder", "OK");
			Debug.Log ("There is no dependency from "+originalFolderRef+" Folder");
		}
	}
	public static void Replacer(string path){
		string[] prefabData = Reconnector.ReadUnityObject (path);

		for (int i = 0; i < prefabData.Length; i++) {
			
			if (prefabData [i].Contains (guID)) {
				string oldGUID = GetGUID (prefabData [i]);
				string guidTopath = GetPath (oldGUID);
				if (guidTopath.Contains (originalFolderPath)) {

					if (originalFolderPath != duplicatedFolderPath) {
						string newPath = GetNewPath (guidTopath, originalFolderPath, duplicatedFolderPath);
						if (newPath.Contains (animator)) {
							if (!pathList.Contains (newPath)) {
								pathList.Add (newPath);
								ChangeSubDependency (newPath, guID, originalFolderPath,duplicatedFolderPath);}
						}
						if (newPath.Contains (material)) {
							if (!pathList.Contains (newPath)) {
								pathList.Add (newPath);
								ChangeSubDependency (newPath, guID, originalFolderPath,duplicatedFolderPath);}
						}
						string newGUID = GetNewGUID (newPath);
						prefabData [i] = ChangeDependency (prefabData [i], newGUID, oldGUID);
                        
					} else{ return; } 
				}
			}
		}
		if (originalFolderPath != duplicatedFolderPath) {
            Reconnector.WriteUnityObject (path, prefabData);
		}
	}
	private static void ChangeSubDependency( string newPath1,string guID1,string originalFolderPath1, string duplicatedFolderPath1){
		string[] data = Reconnector.ReadUnityObject (newPath1);
		for (int j = 0; j < data.Length; j++) {
			if (data [j].Contains (guID1)) {
				string oldGUID = GetGUID (data [j]);
				string guidTopath = GetPath (oldGUID);
				if (guidTopath.Contains (originalFolderPath1)) {
					string path = GetNewPath (guidTopath, originalFolderPath1, duplicatedFolderPath1);
					string newGUID = GetNewGUID (path);
					data [j] = ChangeDependency (data [j], newGUID, oldGUID); 
				}
			}
		}
		Reconnector.WriteUnityObject(newPath1, data);
	}
	public static string GetGUID(string data){
		int guIDIndex = data.IndexOf ("g") + 6;
		string GUID = data.Substring (guIDIndex, 32);
		return GUID;
	}
	static string GetNewGUID (string path){
		string newGUID = AssetDatabase.AssetPathToGUID (path);
		return newGUID;
	}
	public static string GetNewPath1(string oldPath, string originFolder, string duplicatefolder){
		oldPath = oldPath.Replace (originFolder, duplicatefolder);

		return oldPath;
	}
	public static string GetNewPath(string oldPath, string originFolder, string duplicatefolder){

		oldPath = oldPath.Replace (originFolder, duplicatefolder);
		return oldPath;

	}
	public static string ChangeDependency(string data, string newGUID, string oldGUID){
		if (data.Contains (oldGUID)) {
			data = data.Replace (oldGUID, newGUID);
		}
		return data;
	}
	public static string[] FindPrefabs( string folderName){
		return AssetDatabase.FindAssets ("t:Prefab", new[] { folderName });
	}
	public static string GetPath( string id){
		return AssetDatabase.GUIDToAssetPath (id); 
	}
	public static string GetSelectedPath(){
		string selectedPath = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (selectedPath == ""){
			selectedPath = null;
		}else if (System.IO.Path.GetExtension(selectedPath) != ""){
			selectedPath = selectedPath.Replace(System.IO.Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		return selectedPath;
	}
	public static string FindFolderName( string selectedPath){
		if (selectedPath != null) {
			string[] words = selectedPath.Split ('/');
			return words [words.Length - 1];
		} else {
			return null;
		}
	}
}

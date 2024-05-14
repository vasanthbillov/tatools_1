using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelMaskAdjuster : MonoBehaviour
{
	public Camera cam;

	public PanelDataVariants[] panelDataVariants;

	private PanelData[] _panelDataList;
	
	public bool canLerp;

	private UIPanel _uiPanel;

	private SortedDictionary<float, PanelData> _panelDataDict;

	private float screenAspect;

	public string varientName;

	private List<float> keyList;
	
    void Start()
    {
	    if (cam == null) cam = NGUITools.FindCameraForLayer(gameObject.layer);
	    screenAspect = cam.aspect;
	
	    if (panelDataVariants.Length <= 1)
	    {
		    _panelDataList = panelDataVariants[0].panelDataList;

		    SetUp(_panelDataList);
	    }
	    // else 
	    // {
		   //  //Initialise(varientName);
	    // }
    }

    public void Initialise(string variantName)
    {
	    
	    if (variantName != "")
	    {
		    for (int i = 0; i < panelDataVariants.Length; i++)
		    {
			    if (panelDataVariants[i].variantName == variantName)
			    {
				    _panelDataList = panelDataVariants[i].panelDataList;
				
				    SetUp(_panelDataList);
			    }
		    }
	    }
    }


    private void SetUp(PanelData[] panelDataList)
    {
	    if (cam == null) cam = NGUITools.FindCameraForLayer(gameObject.layer);
	    
	    screenAspect = cam.aspect;
	    
	    _panelDataDict = new SortedDictionary<float, PanelData>();
	    
	    keyList = new List<float>();
	    
	    _uiPanel = GetComponent<UIPanel>();
	    
	    // Adding panel data into dict
	    for (int i = 0; i < panelDataList.Length; i++)
	    {
		    float aspectKey = GetAspectRatio(i);
		    
		    panelDataList[i].aspect = aspectKey;
		    
		    _panelDataDict.Add(aspectKey,panelDataList[i] );
	    }
	    
	    foreach (KeyValuePair<float, PanelData> pairs in _panelDataDict)
	    {
		    keyList.Add(pairs.Key);
	    }

	    for (int i = 0; i < keyList.Count; i++)
	    {
		    float aspectKey = _panelDataDict[keyList[i]].aspect;
		    
		    if (i == 0 && screenAspect <= aspectKey)
		    {
		        SetData(aspectKey);
		    }
		    else if (i > 0 && i+1 <= keyList.Count - 1)
		    {
			    float nextAspectKey = _panelDataDict[keyList[i + 1]].aspect;
			    if (screenAspect >= aspectKey && screenAspect <= nextAspectKey)
			    {
				    if (canLerp)
				    {
					    float t = GetStandardizedValue(screenAspect,
						    _panelDataDict[keyList[0]].aspect, _panelDataDict[keyList[keyList.Count - 1]].aspect);

					    float positionY = Mathf.Lerp(_panelDataDict[keyList[i]].positionY,
						    _panelDataDict[keyList[i + 1]].positionY, t);

					    Vector2 offset = Vector2.Lerp(_panelDataDict[keyList[i]].offset,
						    _panelDataDict[keyList[i + 1]].offset, t);

					    Vector2 center = Vector2.Lerp(_panelDataDict[keyList[i]].center,
						    _panelDataDict[keyList[i + 1]].center, t);

					    Vector2 size = Vector2.Lerp(_panelDataDict[keyList[i]].size,
						    _panelDataDict[keyList[i + 1]].size, t);

					    _uiPanel.baseClipRegion = new Vector4(center[0], center[1],
						    size[0], size[1]);

					    transform.localPosition = new Vector3(0f, positionY, 0f);

					    _uiPanel.clipOffset = offset;
				    }
				    else
				    {
					    SetData(aspectKey);
				    }
			    }
			    
		    }
		    else if (i == keyList.Count - 1 && screenAspect >= aspectKey)
		    {
		        SetData(aspectKey);
		    }
	    }
    }
    private void SetData(float key)
    {
	    _uiPanel.baseClipRegion = new Vector4(_panelDataDict[key].center[0], _panelDataDict[key].center[1],
		    _panelDataDict[key].size[0], _panelDataDict[key].size[1]);
	    
	    transform.localPosition = new Vector3(0f,_panelDataDict[key].positionY,0f);
	    
	    _uiPanel.clipOffset = _panelDataDict[key].offset;
    }

    private float GetAspectRatio(int i)
    {
	    return (float)System.Math.Round(_panelDataList[i].aspectRatio[0] / _panelDataList[i].aspectRatio[1],3);
    }
    
    private float GetStandardizedValue(float aspect,float minAspect, float maxAspect)
    {
	    return (aspect - minAspect) / (maxAspect - minAspect);
    }
}

[Serializable]
public class PanelDataVariants
{
	public string variantName;
	public PanelData[] panelDataList;
}

[Serializable]public class PanelData
{
	public float positionY;
	
	public Vector2 aspectRatio;
	
	[NonSerialized]public float aspect;
	
	public Vector2 offset;
	
	public Vector2 center;
	
	public Vector2 size;
}

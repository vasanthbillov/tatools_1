using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelClipHeightPredictor : MonoBehaviour
{
	public Camera cam;

	public float initialY;

	public float clipWidth;

	public int power;
	
	public float[] coefs;
	
	private UIPanel _uiPanel;
	
	private UIScrollView _scrollView;
	
	private float _clipOffsetHeight;
	
	private float _dummyClipHeight;
	
    void Start()
    {
	    _uiPanel = GetComponent<UIPanel>();

	    _scrollView = GetComponent<UIScrollView>();
	    
	    _scrollView.enabled = false;
	    
	    if (cam == null) cam = NGUITools.FindCameraForLayer(gameObject.layer);
	    
	    StartCoroutine(delay());
    }

    IEnumerator delay()
    {
	    yield return new WaitForSeconds(0f);
	    Invoke("Initialize1", 0f);
	    
    }
    
    void Initialize1()
    {
	    float aspect = cam.aspect;

	    _dummyClipHeight = PredictValue(coefs,power, aspect);
	    
	    _uiPanel.baseClipRegion = new Vector4(0f, 0f, clipWidth, _dummyClipHeight);
	    
	    _uiPanel.clipOffset = new Vector2(0f, 0f);
	    
	    Invoke("Initialize2", 0f);
	   
    }

    void Initialize2()
    {
	    _clipOffsetHeight = _dummyClipHeight * 0.5f;
	    
	    _uiPanel.clipOffset = new Vector2(0f, -_clipOffsetHeight);
	    
	    transform.localPosition=new Vector3(0f,initialY,0f);
	    
	    _scrollView.enabled = true;
    }

    float PredictValue(float[] coef, int power, float aspectRatio )
    {
	    float output = 0;
	    
	    for (int i = 0; i < coef.Length; i++)
	    {
		    int p = (power - i);
		    
		    output += (coef[i] * (Mathf.Pow(aspectRatio, p)));
	    }
	    
	    return (float)System.Math.Round(output,3);
    }
    
}

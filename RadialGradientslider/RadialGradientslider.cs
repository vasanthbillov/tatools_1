using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class RadialGradientslider : MonoBehaviour
{

    public UI2DSprite uI2DSprite;

    public GameObject glowSprite;

    public float radius = 0.3f;

    Material spriteMaterial;

    Vector2 center;
 
    void Start()
    {
        uI2DSprite = this.gameObject.GetComponent<UI2DSprite>();
        spriteMaterial = uI2DSprite.material;
        spriteMaterial.SetFloat("_RangeValue", uI2DSprite.fillAmount*2);
        center = uI2DSprite.gameObject.transform.position;
    }

    public void OnSliderValueChanged()
    {
        uI2DSprite.enabled = false;
        spriteMaterial.SetFloat("_RangeValue", uI2DSprite.fillAmount*2);
        uI2DSprite.enabled = true;

        float deg = (uI2DSprite.fillAmount * 360) + 90;
        float n_deg = (360+180) - deg;
        float rad = n_deg * Mathf.Deg2Rad;
        float x = center.x+ (radius * Mathf.Cos(rad));
        float y = center.y+ (radius * Mathf.Sin(rad));
        glowSprite.transform.position = new Vector2(x,y);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FindHirarchy : MonoBehaviour
{

    Transform ob;
    string spaceTree = "";
    int count = 0;
    GameObject prevObj;

    void Start()
    {
        ob = GetComponent<Transform>();
        GetHierarchy(ob.gameObject);
        Debug.Log(spaceTree);
    }


    void GetHierarchy(GameObject obj, string indent = "")
    {
        if (count > 0)
        {
            spaceTree += indent + "|__" + obj.name + "\n";
        }
        else{
            spaceTree +=  obj.name + "\n";
        }
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if ((i == 0))
            {
                if (prevObj != null && indent != null)
                {
                    indent = indent.Substring(0, indent.Length-1);
                    indent += " ";
                }
                else if (count > 0)
                {
                    indent += "|";
                }
            }

            if(i == obj.transform.childCount-1)
            {
                prevObj = obj;
            }
            else
            {
                prevObj = null;
            }
            count++;
            GetHierarchy(obj.transform.GetChild(i).gameObject, indent + "   ");
        }
    }
}

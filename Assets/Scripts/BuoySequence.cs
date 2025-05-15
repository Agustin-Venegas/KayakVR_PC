using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newGameScript",menuName = "Data/BuoyScript")]
public class BuoySequence : ScriptableObject
{
    public List<BuoyData> panels;

    public BuoyData SearchById(string id)
    {
        foreach (BuoyData d in panels)
        {
            if (d.id == id)
            {
                return d;
            }
        }

        return null;
    }
}

[System.Serializable]
public class BuoyData
{
    [HideInInspector]
    public string id;
    [HideInInspector]
    public int ind;
    public int displayInd;

    public string title;
    public string footer;

    [Multiline]
    public string text;

    public List<Sprite> image;

    public List<string> nextIds;

    public bool ending = false;

    [HideInInspector]
    public Vector2 pos; //posicion dentro del editor

    /*public BuoyData()
    {
        image = new List<Sprite>();
        nextIds = new List<string>();
    }*/

    //regresa si tiene id en los siguientes
    public bool Contains(string id)
    {
        foreach (string s in nextIds)
        {
            if (id == s) return true;
        }

        return false;
    }

    public bool IsNext(List<string> ids)
    {
        foreach (string s in ids)
        {
            if (id == s)
            {
                return true;
            }
        }

        return false;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeView
{
    public BuoySequence data;
    public TreePanel connect;
    public TreePanel current;
    public MainBuoyEditorWindow editor;

    public List<TreePanel> list;
    List<List<TreePanel>> list2;

    public TreeView(BuoySequence s, MainBuoyEditorWindow e)
    {
        editor = e;
        data = s;
        list = new List<TreePanel>();

        CreatePanels(s);

        /*BuoyData start = data.panels[0];
        foreach (BuoyData d in s.panels) 
        {
            CreatePanelsRecursive(d,0,0);
        };
        */
    }

    //crear paneles recursivamente
    void CreatePanelsRecursive(BuoyData b, int d, int w)
    {
        TreePanel p = new TreePanel(this, b);
        if (list.Contains(p)) return;

        if (b.pos == Vector2.zero)
        {
            b.pos = new Vector2(w, d);
        }

        
        list.Add(p);

        for (int i = 0; i < b.nextIds.Count; i++)
        {
            BuoyData next = data.SearchById(b.nextIds[i]);
            CreatePanelsRecursive(next, d + 1, i);
        }
    }

    public void View()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Draw(i);
        }

        DrawConnections();
        DrawMouseConnection();
    }

    public void SelectOne(BuoyData b)
    {
        editor.SelectOne(b);
    }

    public void Connect(BuoyData f, BuoyData s)
    {
        if (f.id == s.id) return;

        if (!f.nextIds.Contains(s.id))
        {
            f.nextIds.Add(s.id);
        }
        else
        {
            f.nextIds.Remove(s.id);
        }
    }

    //dibujar conexiones de cada panel
    void DrawConnections()
    {
        foreach (TreePanel p in list)
        {
            var nexts = p.data.nextIds;
            var from = p.GetConnectionPosition(false);
            for (int j = nexts.Count - 1; j >= 0; j--)
            {
                string n = nexts[j];
                Vector3 to;
                if (GetOutConnectionPoint(n, out to))
                {
                    Handles.DrawLine(to, from);
                }
                else
                {
                    p.data.nextIds.Remove(nexts[j]);
                }
            }
        }
    }

    //busca el punto de conexion v en el panel id, regresa si existe
    bool GetOutConnectionPoint(string id, out Vector3 v)
    {
        v = new Vector3();

        //buscar el panel que corresponda
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].data.id == id)
            {
                //si hay un panel aqui y es este
                v = list[i].GetConnectionPosition(true);
                return true;
            }
        }
        return false;
    }

    //dibujar conexion hasta el mouse
    void DrawMouseConnection()
    {
        var e = Event.current;
        if (connect != null && current == null)
        {
            Handles.DrawLine(connect.GetConnectionPosition(false), e.mousePosition);
            editor.Repaint();
        }

        if (e.type == EventType.MouseUp)
        {
            connect = null;
        }
    }

    public void Update()
    {
        list.Clear();

        CreatePanels(data);
    }

    public void CreatePanels(BuoySequence b)
    {
        foreach (BuoyData d in b.panels)
        {
            list.Add(new TreePanel(this, d));
        }
    }
}

 
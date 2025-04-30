using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreePanel : IEquatable<TreePanel>
{
    public Vector2 pos;
    public Rect rect = new Rect(0, 0, 150, 70);

    TreeView editor;
    public BuoyData data;

    Rect name = new Rect(5, 20, 75, 18);
    Rect delete = new Rect(125, 2, 20, 14);
    Rect label = new Rect(5, 10, 206, 45);
    Rect label2 = new Rect(5, 30, 206, 45);
    Rect toolbar = new Rect(85, 20, 126, 18);
    Rect toolbar2 = new Rect(85, 40, 126, 18);

    Rect inputPoint = new Rect(50f, -2, 20, 20);
    Rect outPoint = new Rect(50f, 55, 20, 20);

    int ind = 0;

    public TreePanel(TreeView e, BuoyData b)
    {
        editor = e;
        data = b;
    }

    public void Draw(int i = 0)
    {
        data.ind = i;

        if (rect.x == 0 && rect.y == 0)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(rect.width), GUILayout.Height(rect.height));
            EditorGUILayout.Space();
            rect.x = r.x;
            rect.y = r.y;
        }

        GUI.Label(new Rect(rect.x, rect.y, 20, 20), "" + data.ind);
        GUI.Box(rect, "");
        GUI.BeginGroup(rect);
        GUI.TextArea(new Rect(10, 15, 90, 15), data.title);
        if (GUI.Button(delete, "X"))
        {
            DeleteSelf();
        }

        ConnectPoint(inputPoint, "o", true);
        ConnectPoint(outPoint, "X", false);

        GUI.EndGroup();

        Drag();
    }

    public void DeleteSelf()
    {
        foreach (BuoyData d in editor.data.panels)
        {
            d.nextIds.Remove(data.id);
        }

        editor.data.panels.Remove(data);
        editor.list.Remove(this);

        editor.editor.SelectOne(null);
    }

    //si el mouse esta sobre point, conectar este y el otro panel
    //input es si este panel es entrada o salida
    public void ConnectPoint(Rect point, string symbol, bool input)
    {
        var e = Event.current;
        GUI.Label(point, symbol);
        bool drag = e.type == EventType.MouseDrag
            && point.Contains(e.mousePosition)
            && editor.connect == null;

        if (drag && !input)
        {
            editor.connect = this;
        }

        bool connect = e.type == EventType.MouseUp && input && editor.connect != null;
        if (connect)
        {
            editor.Connect(editor.connect.data, data);
        }
    }

    public void Drag()
    {
        var e = Event.current;
        bool drag = e.type == EventType.MouseDrag
            && rect.Contains(e.mousePosition)
            && editor.connect == null;

        if (drag)
        {

            //e.Use();

            editor.SelectOne(data);
            editor.current = this;
            rect.x += e.delta.x;
            rect.y += e.delta.y;

            Rect r = EditorWindow.GetWindow<MainBuoyEditorWindow>().position;

            rect.x = Mathf.Clamp(rect.x, r.width / 2-rect.width/2, r.width - rect.width / 2);
            rect.y = Mathf.Clamp(rect.y, rect.height / 2, r.height - rect.height / 2);

            pos.x = rect.x;
            pos.y = rect.y;

            editor.editor.Repaint();
        }

        if (e.type == EventType.MouseUp) editor.current = null;
    }

    public bool Equals(TreePanel other)
    {
        return data.id == other.data.id;
    }

    //regresa la posicion correspondiente, input si es entrada o salida
    public Vector3 GetConnectionPosition(bool input)
    {
        if (input)
        {
            return new Vector3(rect.x, rect.y, 0) + new Vector3(rect.width/2,0);
        }
        else
        {
            return new Vector3(rect.x, rect.y, 0) + new Vector3(rect.width/2, rect.height);
        }
    }
}
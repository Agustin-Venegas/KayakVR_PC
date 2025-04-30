using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;

[CanEditMultipleObjects]
public class MainBuoyEditorWindow : EditorWindow
{
    //original data
    public BuoySequence originalData;

    //copy
    public List<BuoyData> data;

    TreeView tree;

    Vector2 pos;

    BuoyData selected;

    [MenuItem("Window/BuoyEditor")]
    public static void ShowWindow()
    {
        EditorWindow wind = GetWindow<MainBuoyEditorWindow>();
        wind.minSize = new Vector2(500, 300);
    }

    public void OnGUI()
    {
        if (data == null)
        {
            Debug.Log("No hay datos seleccionados");
            return;
        } else
        {
            //header
            MakeHeader();

            GUILayout.BeginHorizontal();
            MakeEditor();

            MakeTree();

            GUILayout.EndHorizontal();
        }
    }

    void MakeHeader()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Editor");
        EditorGUILayout.LabelField("Asset Name: " + originalData.name);
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        if (GUILayout.Button("+"))
        {
            AddNew();
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    //accion de sumar un dato nuevo en el asset
    void AddNew()
    {
        BuoyData d = new BuoyData();
        d.id = GUID.Generate().ToString();
        d.ind = data.Count;
        data.Add(d);

        if (tree != null) tree.Update();

        Repaint();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    //hacer el arbol
    void MakeTree()
    {
        Rect r = GetWindow<MainBuoyEditorWindow>().position;

        //Rect r2 = EditorGUILayout.GetControlRect(GUILayout.MinWidth(r.width / 2), GUILayout.MinHeight(r.height - 100));

        //GUILayout.BeginArea(r2);
        GUILayout.BeginVertical(GUILayout.Width(r.width / 2));

        if (tree == null) tree = new TreeView(originalData, this);
        tree.View();

        GUILayout.EndVertical();
        //GUILayout.EndArea();
    }

    void MakeEditor()
    {
        Rect r = GetWindow<MainBuoyEditorWindow>().position;
        EditorGUILayout.BeginVertical(GUILayout.MinWidth(100), GUILayout.MaxWidth(r.width / 2 - 50));

        if (selected != null)
        {           
            /*
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(r.width / 2 - 60));
            EditorGUILayout.LabelField("Title: ");
            selected.title = EditorGUILayout.TextField(selected.title);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(r.width / 2 -60));
            EditorGUILayout.LabelField("Text: ");
            selected.text = EditorGUILayout.TextArea(selected.text, GUILayout.MaxWidth(r.width / 2 -50));
            EditorGUILayout.EndHorizontal();
            */
            
            SerializedObject so = new SerializedObject(originalData);
            SerializedProperty sp = so.FindProperty("panels").GetArrayElementAtIndex(selected.ind);
            if (sp != null) EditorGUILayout.PropertyField(sp, true, GUILayout.MaxWidth(r.width / 2 - 50));
            so.ApplyModifiedProperties();
            

        } else
        {
            EditorGUILayout.LabelField("No Buoy Selected");
        }
        EditorGUILayout.EndVertical();
    }

    public void SelectOne(BuoyData s)
    {
        selected = s;
    }

    void MakePanel(int i)
    {
        BuoyData d = data[i];
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();

        d.title = GUILayout.TextArea(d.title,GUILayout.MaxWidth(100));
        d.text = GUILayout.TextArea(d.text, GUILayout.MinWidth(50));
        //d.image = (Sprite)EditorGUILayout.ObjectField(d.image, typeof(Sprite), true, GUILayout.MaxWidth(150));

        if (GUILayout.Button("-"))
        {
            data.RemoveAt(i);
        }

        GUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        if (FindManagerOnScene())
        {
            //asegurarse que haya por lo menos 1 elemento
            if (data.Count < 1) AddNew();

            tree = new TreeView(originalData, this);
        }
        else
        {
            Debug.Log("No BuoyManager Found");
        }
    }

    //encuentra si existee el manager
    bool FindManagerOnScene()
    {
        BuoyManager manager = FindObjectOfType<BuoyManager>();
        if (manager != null)
        {
            originalData = manager.sequence;
            data = originalData.panels;
            return true;
        } else 
        {
            return false;
        }
    }
}

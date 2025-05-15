using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

//sube los datos a un google forms en el URL, en los elementos destinados en entries
public class DataUploader : MonoBehaviour
{
    //IMPORTANTE: Define el software y version del juego, 01o es arg. fuerte, en oculus
    public string Header = "01H";

    //Define el formato y a q elemento HTTP va la info
    public List<pair> Entries;

    public static int index = -1;

    [Serializable]
    public class pair
    {
        [SerializeField]
       public string[] d = { null, null };
    }

    //URL para postear dato
    [SerializeField]
    private string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSc4P9bB3AziCM7SOiRnS4SPbz0BGlhTHeSdb5sfxCpOZINCBw/formResponse";

    //URL para obtener una copia de los datos
    [SerializeField]
    private string getUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRi7Tw_ZfGL0P2s2rDh-oYvfHACKeJtqx6_g3et8AIdC7jjZ18WWH6LKkMEpOlSk5tt1t5p_yc4-7L4/pub?output=csv";

    private void Start()
    {
        StartCoroutine(GetIndex());
    }

    //manda una request a geturl y ve cuantas lineas hay puestas en el drive
    IEnumerator GetIndex()
    {
        UnityWebRequest www = UnityWebRequest.Get(getUrl);

        yield return www.SendWebRequest();
        if (www.error != null) Debug.Log(www.error);

        string res = www.downloadHandler.text;

        string[] lines = res.Split('\n');
        index = lines.Length+1;

        LogManager.Instance.index = Header + index;

        Debug.Log("index = " + index);
    }


    //poner datos en la url segun el formato en entries
    IEnumerator Post(List<string> data)
    {
        Debug.Log("Uploading");
        WWWForm form = new WWWForm();

        for (int i = 0; i < Entries.Count; i++)
        {
            form.AddField(Entries[i].d[0], data[i]);
        }

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        if (www.error != null)
        {
            Debug.Log(www.error);
        }

        //mostrar codigo al final
        UIController.Instance.SetFooter("Codigo: " + Header + index);
    }


    //transforma los datos de logmanager en una lista de strings y trata de subirla
    public void Upload()
    {
        LogManager lm = LogManager.Instance;

        //si no hay suficientes datos, cancelar, entrada a 3 boyas
        if (lm.data.Count < 4) return;

        List<string> d = new List<string>();

        //poner nombre y fecha en formato
        d.Add(Header+index);
        d.Add(lm.shortFilename);

        //llenar los datos con formato
        for (int i = 2; i < Entries.Count; i++)
        {
            pair p = Entries[i];
            foreach (LogData ld in lm.data) //calzar logdata con entry formato
            {
                if (p.d[1] == ld.data[0]) d.Add(ld.data[1].ToString());
            }

            //si no se guardo algun dato, escribir un valor de error
            if (d.Count <= i) d.Add("" + -1);
        }

        StartCoroutine(Post(d));
    }

}

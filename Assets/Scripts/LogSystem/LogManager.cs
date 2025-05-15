using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class LogManager
{
    string path = Application.persistentDataPath;
    string filename;
    public string shortFilename { get; set; }
    public string index { get; set; }

    public List<LogData> data;
    public List<string> stringData;

    private LogManager()
    {
        data = new List<LogData>();

        GenerateFilename();
    }

    void GenerateFilename()
    {
        DateTime t = DateTime.Today;

        int count = 1;

        //comprobar si hay uno llamado parecido
        do
        {
            filename = path + "/" + t.ToString("MM-dd-yyyy") + "_" + count + ".csv";
            shortFilename = t.ToString("MM-dd-yyyy") + "_" + count;
            count++;
        } while (File.Exists(filename));
    }

    private static LogManager instance = new LogManager();
    public static LogManager Instance { get { return instance; } }

    public void WriteToPath()
    {
        StreamWriter writer = new StreamWriter(filename);

        //contar columnas
        int i = 0;
        foreach (LogData s in data) if (s.data.Count > i) i = s.data.Count;

        //escribir header con id y numeros de columnas
        writer.WriteLine(index);
        writer.WriteLine(NumericHeader(i));

        foreach (LogData s in data)
        {
            writer.WriteLine(s.ToString());
        }
        writer.Close();
    }

    //regresa el primero q tenga el primer dato s
    public LogData SearchById(string s)
    {
        foreach (LogData d in data)
        {
            if (d.data[0] == s) return d;
        }

        return null;
    }

    //genera una linea csv contando de 0 hasta i-1
    public string NumericHeader(int i)
    {
        string Header = string.Empty;
        for (int j = 0; j < i - 1; j++) Header += j + ",";
        Header += i - 1;
        return Header;
    }

    //formatear un objeto para escribir en archivo, con una coma
    public static string Format(object o, bool comma = true)
    {
        string r = string.Empty;
        string s = o.ToString();

        r = "\"" + s + "\"";

        /*if (s.Contains(','))
        {
            r = "\"" + s + "\"";
        }
        else
        {
            r = s;
        }
        */

        if (comma) r += ",";

        return r;
    }

    
}

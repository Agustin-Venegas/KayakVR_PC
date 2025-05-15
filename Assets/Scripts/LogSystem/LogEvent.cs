using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//representa una linea de datos csv
[Serializable]
public class LogData
{
    public bool AppendRunTime = true;
    public List<string> data;

    public LogData()
    {
        data = new List<string>();
    }

    public override string ToString()
    {
        string s = string.Empty;

        for (int i = 0; i < data.Count - 1; i++)
        {
            s += LogManager.Format(data[i]);
        }

        s += LogManager.Format(data[data.Count - 1], AppendRunTime);

        if (AppendRunTime)
        {
            s += LogManager.Format(Time.time,false);
        }

        return s;
    }
}

public class LogEvent : MonoBehaviour
{
    //datos a escribir en archivo
    public List<string> data;
    //escribir tiempo en ultima columna
    public bool AppendRuntime = true;

    private LogData logData;

    //genera una linea de datos
    public string RecordDataLine( params object[] d)
    {
        string r = string.Empty;

        for (int i = 0; i<d.Length-1; i++)
        {
            object o = d[i];
            r += LogManager.Format(o);
        }

        r += LogManager.Format(d[d.Length - 1],false);

        r += "\n";

        return r;
    }

    public LogData GenerateRecord(params object[] d)
    {
        LogData r = new LogData();

        foreach (object o in d)
        {
            r.data.Add(o.ToString().Replace(".", ","));
        }

        if (AppendRuntime)
        {
            r.data.Add(UnityEngine.Time.time.ToString("#####.###").Replace(".", ","));
        }

        return r;
    }

    //Evento que genera un registro que se escribira en el archivo
    //con los strings puestos en esta clase
    //si ya existe lo actualiza
    public void RecordLine()
    {
        //si el dato no existe, generar
        if (logData == null)
        {
            logData = GenerateRecord(data.ToArray());
            LogManager.Instance.data.Add(logData);
        } else //si existe, actualizarlo
        {
            LogData temp = GenerateRecord(data.ToArray());
            logData.data = temp.data;
        }
    }

    public string Time { get { return UnityEngine.Time.time.ToString(); } }
}

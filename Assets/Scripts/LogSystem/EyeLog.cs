using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class EyeLog : MonoBehaviour
{
    private BuoyData cl;
    public BuoyData client 
    {
        get 
        { return cl; }
        set 
        {
            cl = value;
            LogData l = LogManager.Instance.SearchById("Time"+(cl.displayInd)+"Read");
            if (l == null)
            {
                log = GenerateLogData();
                LogManager.Instance.data.Add(log);
            } else //si ya existe un registro para esta boya
            {
                log = l;
                totalTime = float.Parse(log.data[1]);
            }
        }
    }
    public LogData log;

    float totalTime = 0;

    float lastTime = 0;

    bool inside = false;

    public float TotalTime { get { return totalTime; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UIPanel"))
        {
            inside = true;
            Debug.Log("Eye Tracking Enter");
            lastTime = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UIPanel"))
        {
            inside = false;
            totalTime += (Time.time - lastTime);
            lastTime = 0;
        }
    }

    public void Reset()
    {
        cl = null;
        log = null;
        if (inside) lastTime = Time.time; else lastTime = 0;
        totalTime = 0;
    }

    public void RecordData()
    {
        if (inside) totalTime += (Time.time - lastTime);

        //regenerar el dato y actualizar
        LogData d = GenerateLogData();

        //obtener valor antiguo, reemplazando la coma por punto :c
        string oldstring = log.data[1].Replace(",", ".");
        float old = float.Parse(oldstring, CultureInfo.InvariantCulture);

        if (totalTime > old)
        {
            log.data = d.data;
        }

        Debug.Log("ReadTime: "+TotalTime);
    }

    //generar una linea de datos
    public LogData GenerateLogData()
    {
        LogData d = new LogData(); 
        d.data.Add("Time"+(cl.displayInd)+"Read");
        string dat = totalTime.ToString("####0.###").Replace(".", ",");
        d.data.Add(dat);
        d.AppendRunTime = false;
        
        return d;
    }
}

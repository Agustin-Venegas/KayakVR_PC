using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//este manager se encarga de avanzar a traves de las boyas, tiene referencias a todas las boyas
public class BuoyManager : MonoBehaviour
{
    public static BuoyManager Instance;

    public BuoySequence sequence;

    int buoy_index = 0; //ultima boya visitada
    public List<BuoyTrigger> buoys;

    //bool ExecuteOnlyOnce = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        buoys[0].gameObject.SetActive(true);
        for (int i = 0; i<buoys.Count; i++)
        {
            buoys[i].AddEvent(i);
        }
    }

    public void Next(int i)
    {
        if (sequence.panels[buoy_index].ending)
        {
            LogManager.Instance.WriteToPath();
        } else
        {
            BuoyData d = sequence.panels[i];

            if (i >= buoy_index)
            {
                buoy_index = i;
            }
            
            //activar siguientes
            for (int j = 0; j < sequence.panels.Count; j++)
            {
                //si esta en los siguientes de la boya tocada
                if (sequence.panels[j].IsNext(d.nextIds))
                {
                    buoys[j].Activate(true);
                }
            }
        }
    }

    public void SetDataIndex(int i)
    {
        UIController.Instance.AssignData(sequence.panels[i]);
        UIController.Instance.Show();
    }

    public void WriteToFile()
    {
        LogManager.Instance.WriteToPath();
        FindAnyObjectByType<DataUploader>().Upload();
    }

    //private void OnApplicationQuit() { WriteToFile();}

    //private void OnApplicationPause(bool pause) { WriteToFile();}
}
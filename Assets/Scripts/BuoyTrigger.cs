using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//este script se encarga de representar cada boya informativa
public class BuoyTrigger : TriggerEvent
{
    public bool hasRay = false; //si tiene un rayo
    public GameObject rayObject;

    int i;

    public void AddEvent(int i)
    {
        this.i = i;

        if (hasRay)
            onTriggerEnter.AddListener(
            () => 
            {
                Debug.Log("EventTriggered");
                BuoyManager.Instance.SetDataIndex(i);
                BuoyManager.Instance.Next(i);
            }
            );
    }

    public void DeactivateTrigger()
    {
        Collider[] c = GetComponentsInChildren<Collider>();

        foreach (Collider col in c)
        {
            if (col.isTrigger)
            {
                col.enabled = false;
            }
        }
    }

    public void ActivateTrigger()
    {
        Collider[] c = GetComponentsInChildren<Collider>();

        foreach (Collider col in c)
        {
            if (col.isTrigger)
            {
                col.enabled = true;
            }
        }
    }

    //que hacer cuando se activa
    public void Activate(bool ray)
    {
        if (ray && hasRay)
        {
            rayObject.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    public void DeactivateUI()
    {
        UIController.Instance.Hide();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Script simple para activar algo al salir y entrar de un trigger
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            onTriggerEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            onTriggerExit.Invoke();
    }
}

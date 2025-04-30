using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

//activa y desactiva los paddles dependiendo si estan conectados o no
public class PaddleController : MonoBehaviour 
{
    
    public GameObject rightpaddle;
    public GameObject leftpaddle;

	void Start ()
    {
        if (XRController.leftHand == null)
        {
            rightpaddle.SetActive(true);
        }

        if (XRController.rightHand == null)
        {
            leftpaddle.SetActive(true);
        }

    }

    void Update ()
    {
        if (XRController.leftHand == null)
        {
            rightpaddle.SetActive(true);
        }

        if (XRController.rightHand == null)
        {
            leftpaddle.SetActive(true);
        }
        
	}    
}

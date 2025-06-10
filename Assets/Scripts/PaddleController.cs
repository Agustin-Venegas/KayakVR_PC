using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

//Controlar remo con mando xbox o PC, con animaciones
//antes, servia para apagar y prender remo segun mano vr conectada
public class PaddleController : MonoBehaviour 
{
    public GameObject rightpaddle;
    public GameObject leftpaddle;

    public Animator animator;

    Vector2 input;

	void Start ()
    {
        /*
        if (XRController.leftHand == null)
        {
            rightpaddle.SetActive(true);
        }

        if (XRController.rightHand == null)
        {
            leftpaddle.SetActive(true);
        }
        */

    }

    public void OnMove(InputValue movementValue)
    {
        input = movementValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        float r = Mathf.Clamp01(input.x);
        float l = Mathf.Clamp01(-input.x);
        float f = input.y;

        animator.SetFloat("Forward", f);
        animator.SetFloat("Right", r);
        animator.SetFloat("Left", l);

        if (f == 0)
        {
            animator.SetFloat("ActualSpeed", 0f);
        } else
        {
            animator.SetFloat("ActualSpeed", 1f);
        }

        /*
        if (input.y < 0)
        {
            
            animator.SetFloat("ActualSpeed", 1f);
        } else if (input.y == 0)
        {
            animator.SetFloat("Forward", 0f);
        } else
        {
            animator.SetFloat("Forward", f);
        }
        */
    }


    void Update ()
    {
        /*if (XRController.leftHand == null)
        {
            rightpaddle.SetActive(true);
        }

        if (XRController.rightHand == null)
        {
            leftpaddle.SetActive(true);
        }
        */
	}    
}

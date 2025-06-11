using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    Quaternion rot;
    public GameObject Camera;

    public float Sensitivity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rot = Camera.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Camera.transform.rotation = rot;
    }

    //recibe evento de player input, mouse o stick derecho
    public void OnLook(InputValue v)
    {
        Vector2 delta = Sensitivity * v.Get<Vector2>();

        Vector3 euler = rot.eulerAngles;

        //clampear entre 60 y 0 y 360 y 300
        euler.x = euler.x - delta.y > 60 && euler.x - delta.y < 300 ? euler.x : euler.x - delta.y;
        euler.y = euler.y + delta.x > 170 && euler.y + delta.x < 260 ? euler.y : euler.y + delta.x;
        euler.z = 0;

        //Mathf.Clamp(rot.eulerAngles.x - delta.y,-70f,25f)

        Debug.Log("Rot:"+rot.eulerAngles.y);

        rot = Quaternion.Euler(euler);
    }
}

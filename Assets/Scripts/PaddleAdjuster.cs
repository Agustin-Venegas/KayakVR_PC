using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//rota el objeto paddle en espacio local, segun los botones

public class PaddleAdjuster : MonoBehaviour
{
    public GameObject paddle;
    bool rotating = false;

    //InputAction

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //cambiar de modo al presionar
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("Activando Ajuste");
            rotating = !rotating;
        }

        if (rotating)
        {
            Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            paddle.transform.localEulerAngles += new Vector3(0, input.x, input.y);
        }
    }
}

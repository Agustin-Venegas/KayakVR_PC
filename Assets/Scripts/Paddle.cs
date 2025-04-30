using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Paddle : MonoBehaviour {

    Vector3 _waterLevel;
    Vector3 lastPos; //ultima diferencia de posicion global kayak remo
    public AudioClip paddleEffect;
    public AudioSource fuenteAudio;
    public GameObject splashEffect;

    [Range(0, 0.1f)]
    public float RotationBias = 0.05f;
    bool underWater = false;

    // Use this for initialization
    void Start ()
    {
        _waterLevel = KayakController.Singleton.transform.position;
	}

    private void FixedUpdate()
    {
        //distancia espacio global centro de kayak a remo
        Vector3 relativePos = transform.position - KayakController.Singleton.transform.position;
        
        //inverso de movido el remo el ultimo cuadro, positivo empuja el kayak hacia adelante
        Vector3 movedPos = (lastPos - relativePos);

        float magnitude = movedPos.magnitude;

        //si el remo esta sumerjido
        if (transform.position.y < _waterLevel.y)
        {
            //efectos una vez, cuando entra
            if (fuenteAudio != null && !underWater)
            {
                fuenteAudio.clip = paddleEffect;
                fuenteAudio.Play();
                underWater = true;

                float splash = Mathf.Clamp(magnitude, 0.03f, 0.2f);
                Instantiate(splashEffect, transform.position, Quaternion.identity).transform.localScale = new Vector3(splash, splash, splash);
            }

            if (lastPos != Vector3.zero)
            {
                //empuje en el plano horizontal
                Vector3 thrust = movedPos;
                thrust.y = 0;

                //multiplicar dependiendo de cuan profundo va el remo
                thrust *= (-relativePos.y)*0.8f;

                //transformar empuje a espacio local para amortigura en X
                Vector3 nThrust = KayakController.Singleton.transform.InverseTransformDirection(thrust);
                nThrust.x *= 0.5f; //disminuir cantidad de empuje izquierda-derecha
                thrust = KayakController.Singleton.transform.TransformDirection(nThrust);

                //indicar el lado al que va
                int side = 0;

                //distancia del kayak al remo en espacio local de kayak
                Vector3 localpos = KayakController.Singleton.transform.InverseTransformPoint(transform.position);

                //ver si debe girar a la derecha o a la izquierda
                //si es a la derecha
                if (localpos.x > 0)
                {
                    //girar a la izquierda
                    side = -1;
                } else
                {
                    side = 1;
                }

                //fuerza rotativa extra, dependiendo de la distancia en x del kayak al remo, mientras mas lejos, mas rota
                float rotative = Mathf.Clamp(Mathf.Abs(localpos.x) / 3f, 0.5f, 3);

                KayakController.Singleton.AddThrust(thrust, rotative, side);
            }
        }
        else
        {
            //cuando sale del agua
            if (fuenteAudio == null)
            {
                fuenteAudio = GetComponent<AudioSource>();
            }
            underWater = false;
        }

        lastPos = relativePos;
    }
}

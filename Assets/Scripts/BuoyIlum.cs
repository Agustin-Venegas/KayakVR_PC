using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyIlum : MonoBehaviour {

    public Material mat;
    public float speed = 1;
    private float lastpeed;

    void Update()
    {
        float emission = Mathf.PingPong(Time.time * speed, 1.0f);
        Color baseColor = Color.yellow;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    public void Stop()
    {
        mat.SetColor("_EmissionColor", Color.black);
        lastpeed = speed;
        speed = 0;
    }

    public void Init()
    {
        speed = lastpeed;
    }
}

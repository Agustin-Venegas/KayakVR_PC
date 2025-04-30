using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//elimina el objeto despues de un tiempo
public class KillTimer : MonoBehaviour
{
    public float timer = 2f;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 )
        {
            Destroy(gameObject);
        }
    }
}

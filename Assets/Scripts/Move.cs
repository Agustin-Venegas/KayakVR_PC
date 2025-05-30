using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 vector;
    public float speed;

    public void Update()
    {
        transform.position += transform.right * speed;
    }
}

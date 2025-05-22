using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KayakController : MonoBehaviour {

    public float speed = 5.0f;

    public float friction = 10;
    public float torque = 10;

    private static KayakController _singleton;
    Vector3 dir;
    Quaternion lookRot;

    [SerializeField]
    Rigidbody _rb;

    Quaternion _initPos;

    public static KayakController Singleton
    {
        get
        {
            if (_singleton==null)
            {
                _singleton = FindObjectOfType<KayakController>();
            }
            return _singleton;
        }
    }

    public float GetAngle()
    {
        return Quaternion.Angle(_initPos, transform.rotation);
    }

    public Vector3 GetSpeed()
    {
        return _rb.velocity;
    }

    private void Start()
    {
        _initPos = transform.rotation;
        _rb.inertiaTensorRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        //desaceleracion
        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, Time.deltaTime * friction);
    }

    public void AddThrust(Vector3 thrust, float rotative, int side)
    {
        thrust.y = 0;

        if (_rb.velocity.magnitude < 4)
        {
            _rb.AddForce(speed * (thrust), ForceMode.Acceleration);
        }

        //Vector3 torq = new Vector3(0, thrust.magnitude * side * (torque - _rb.velocity.magnitude),0);
        //Vector3 torq = new Vector3(0, thrust.magnitude*rotative * side * (torque - _rb.velocity.magnitude),0);
        //Vector3 torqFinal = new Vector3(0, (_rb.velocity - thrust).magnitude * side * torque + rotative * side * torque, 0);

        //fisica de kayak, cuando empujo de un lado, girar hacia el otro. Si esta frenando un lado, girar hacia ese lado
        //torque en movimiento
        //float dirForce = Mathf.Clamp((90 - Vector3.Angle(thrust,transform.forward))/90f,-1f, 1f); //positivo cuando el remo mueve hacia adelante, negativo cuando va hacia atras
        //float secondtorque = Mathf.Clamp(0.25f - thrust.magnitude, 0,1) * rotative * side ; // * mag

        Vector3 torqFinal = new Vector3(0, ((_rb.velocity - thrust).magnitude * side * torque * rotative),0);
        _rb.AddRelativeTorque(-torqFinal, ForceMode.Acceleration);
    }
}
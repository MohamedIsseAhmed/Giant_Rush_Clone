using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusRotation : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float turnAngel = 10f;
    void Update()
    {
        //transform.rotation *= Quaternion.Euler(0, turnAngel, 0);
        transform.rotation *= Quaternion.AngleAxis(turnAngel*turnSpeed*Time.deltaTime, Vector3.up);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishBlock : MonoBehaviour
{
    [SerializeField] private float shakeIntensity, duration,scaleEndValue,tweenTime;
    [SerializeField] private Transform blockParent;
    [SerializeField] private Transform rayStartPoint;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private LayerMask layerMask;
    private bool hasacaollidedWithObject = false;
    private void OnTriggerEnter(Collider other)
    {
        hasacaollidedWithObject = true;
        blockParent.DOScaleZ(scaleEndValue, tweenTime);
        CamerController.ShakeMainCmera(shakeIntensity, duration);
    }
    private void Update()
    {
        //Ray ray=new Ray(transform.position*rayDistance, Vector3.up); 
        //RaycastHit hit;
        //if(Physics.Raycast(ray,out hit,100, layerMask, QueryTriggerInteraction.Collide) && hasacaollidedWithObject)
        //{
            
        //    print(hit.transform.name);
        //}
        //Debug.DrawLine(ray.origin, ray.origin+ray.direction * rayDistance, Color.red);
    }
}

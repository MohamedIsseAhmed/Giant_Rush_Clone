using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CamerController : MonoBehaviour
{
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float slerpSpeed;
    [SerializeField] private float movbeSpeed=7f;
    [SerializeField] private float taregtY;
    [SerializeField] private float currentY;
    [SerializeField] private float taregtZ;
    [SerializeField] private float currentZ;
    [SerializeField] private Transform taregt;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtual;
    private CinemachineTransposer cinemachineTransposer;
    private CinemachineBasicMultiChannelPerlin channelPerlinNoise;
    private float shakeTimer;
    private float shakeTotalTimer;
    private float startingIntensity;
    public static CamerController instance;
    private Action StopBoxCastingAction;
    private void Awake()
    {
        instance = this;
        cinemachineTransposer = cinemachineVirtual.GetCinemachineComponent<CinemachineTransposer>();
        channelPerlinNoise = cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineTransposer.m_FollowOffset.y = currentY;
        cinemachineTransposer.m_FollowOffset.z = currentZ;
    }
    private void Start()
    {
     
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;    
            //channelPerlinNoise.m_AmplitudeGain = 0;
            channelPerlinNoise.m_AmplitudeGain=Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTotalTimer));
            if (shakeTimer <= 0)
            {
                StopBoxCastingAction.Invoke();
            }
        }
    }
    void LateUpdate()
    {
        FollowTarget();
    }
    private void FollowTarget()
    {
      
        currentY =Mathf.Lerp(currentY,taregtY,lerpSpeed*Time.deltaTime);
        if (currentY <= taregtY)
        {
            currentY = taregtY;
        }
     
        currentZ = Mathf.Lerp(currentZ, taregtZ, lerpSpeed * Time.deltaTime);
        if (currentZ >= taregtZ)
        {
            currentZ = taregtZ;
            
        }
        followOffset.x = 0;
        followOffset.z = currentZ;
        followOffset.y = currentY;
        Vector3 targetPosition = taregt.position + followOffset;
        Vector3 pos = transform.position;
        pos.x = 0;
        targetPosition.x = transform.position.x;
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, movbeSpeed * Time.deltaTime);
        cinemachineTransposer.m_FollowOffset = followOffset;
        transform.position = pos;
    }
    public void ShakeCamera(float intesnsity,float timer,Action _StopBoxCastingAction)
    {
        this.StopBoxCastingAction = _StopBoxCastingAction;
        startingIntensity = intesnsity;
        shakeTimer = timer;
        shakeTotalTimer= timer;
        channelPerlinNoise.m_AmplitudeGain = intesnsity;

    }
}

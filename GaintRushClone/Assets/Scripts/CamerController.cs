using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;
public class CamerController : MonoBehaviour
{
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private Vector3 mainCamfollowOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float slerpSpeed;
    [SerializeField] private float movbeSpeed=7f;
    [SerializeField] private float taregtY;
    [SerializeField] private float currentY;
    [SerializeField] private float taregtZ;
    [SerializeField] private float taregtZOnFollowingBoss;
    [SerializeField] private float currentZ;
    [SerializeField] private float maxDeltaAngel;
    [SerializeField] private Transform taregt;
    [SerializeField] private Transform CameraPointOnFighting;
    [SerializeField] private Transform CameraPointOnLarKick;
    [SerializeField] private Transform boss;
    [SerializeField] private Transform onlastTargetPoint;
    [SerializeField] private IsOnMainCamera isOnMainCamera;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtual;
    private CinemachineTransposer cinemachineTransposer;
    private CinemachineBasicMultiChannelPerlin channelPerlinNoise;
    private float shakeTimer;
    private float shakeTotalTimer;
    private float startingIntensity;
    public static CamerController instance;
    private Action StopBoxCastingAction;
    private bool hasFightingStarted = false;
    private bool gokickingPoint = false;
        
    [SerializeField] private float timerTest = 10;
    [SerializeField] private bool isFollowingBoss = false;
    private Camera mainCamera;
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
        mainCamera = Camera.main;
        taregt.GetComponent<Player>().OnKickingBoss += CamerController_OnKickingBoss;
       
        BossManager.instance.OnEnemyDie += Ýnstance_OnEnemyDie;
    }
    private void Ýnstance_OnEnemyDie(object sender, EventArgs e)
    {
        isFollowingBoss = true;
        cinemachineVirtual.m_Follow = boss;
    }
    private void CamerController_OnKickingBoss(object sender, EventArgs e)
    {
        gokickingPoint = true;
    }
    private void OnDisable()
    {
        taregt.GetComponent<Player>().OnKickingBoss -= CamerController_OnKickingBoss;

        BossManager.instance.OnEnemyDie -= Ýnstance_OnEnemyDie;
    }


    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;    
           
            channelPerlinNoise.m_AmplitudeGain=Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTotalTimer));
            if (shakeTimer <= 0)
            {
                StopBoxCastingAction?.Invoke();
            }
        }
       
    }
    void LateUpdate()
    {
        if (isFollowingBoss && isOnMainCamera==IsOnMainCamera.True)
        {
            
            cinemachineVirtual.enabled = false;
            mainCamera.GetComponent<CinemachineBrain>().enabled = false;
            Vector3 camPos = Camera.main.transform.position;
            Vector3 targetPos = boss.GetChild(1).position+ mainCamfollowOffset;
            mainCamera.transform.position = Vector3.Lerp(camPos, targetPos, lerpSpeed * Time.deltaTime);
        }
        if (GameManager.instance.IsFightingStarted() &&!gokickingPoint && !isFollowingBoss)
        {
          
            cinemachineVirtual.m_Follow = null;
            hasFightingStarted = true;
            transform.position = Vector3.Lerp(transform.position, CameraPointOnFighting.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, CameraPointOnFighting.rotation, maxDeltaAngel * Time.deltaTime);
          
        }
        else if (gokickingPoint)
        {
            cinemachineVirtual.m_Follow = null;
            transform.position = Vector3.Lerp(transform.position, CameraPointOnLarKick.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, CameraPointOnLarKick.rotation, maxDeltaAngel * Time.deltaTime);
        }
        else
        {
            FollowTarget();
        }
    
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
       
        cinemachineTransposer.m_FollowOffset = followOffset;
       
    }
    public void ShakeCamera(float intesnsity,float timer,Action _StopBoxCastingAction)
    {
        this.StopBoxCastingAction = _StopBoxCastingAction;
        startingIntensity = intesnsity;
        shakeTimer = timer;
        shakeTotalTimer= timer;
        channelPerlinNoise.m_AmplitudeGain = intesnsity;
       
    }
    private void OnShake(float duration, float strength)
    {
       
        mainCamera.transform.DOShakeRotation(duration, strength);
    }

    public static void ShakeMainCmera(float duration, float strength) => instance.OnShake(duration, strength);
}
public enum IsOnMainCamera
{
    True,
    False
}

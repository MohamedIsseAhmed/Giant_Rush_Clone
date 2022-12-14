using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isGameOver = false;
    private bool startFighting = false;
    private Vector3 fightingPoint;
    private Quaternion fightingRotation;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    private void Start()
    {
        ScaleUpAnChangeColor.instance.OnDİedEvent += İnstance_OnDİedEvent;
        FightinpointTriger.instance.StartFightingEvent += İnstance_StartFightingEvent;
    }

    private void İnstance_StartFightingEvent(Vector3 _fightingPoint,Quaternion _fightingRotation)
    {
        startFighting = true;
        fightingPoint = _fightingPoint;
        fightingRotation = _fightingRotation;
      
    }
    private void OnDisable()
    {
        ScaleUpAnChangeColor.instance.OnDİedEvent -= İnstance_OnDİedEvent;
        FightinpointTriger.instance.StartFightingEvent -= İnstance_StartFightingEvent;
    }
    private void İnstance_OnDİedEvent(object sender, System.EventArgs e)
    { 
        isGameOver = true;
    }

    public Quaternion GetFightinPointRotation()
    {
        return fightingRotation;
    }
    public Vector3 GetFightinPoint ()
    {
        return fightingPoint;
    }
    public bool IsFightingStarted()
    {
        return startFighting;
    }
    public bool IsGameOverOver()
    {
        return isGameOver;
    }
}

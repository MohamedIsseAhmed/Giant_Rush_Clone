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
        ScaleUpAnChangeColor.instance.OnD�edEvent += �nstance_OnD�edEvent;
        FightinpointTriger.instance.StartFightingEvent += �nstance_StartFightingEvent;
    }

    private void �nstance_StartFightingEvent(Vector3 _fightingPoint,Quaternion _fightingRotation)
    {
        startFighting = true;
        fightingPoint = _fightingPoint;
        fightingRotation = _fightingRotation;
      
    }
    private void OnDisable()
    {
        ScaleUpAnChangeColor.instance.OnD�edEvent -= �nstance_OnD�edEvent;
        FightinpointTriger.instance.StartFightingEvent -= �nstance_StartFightingEvent;
    }
    private void �nstance_OnD�edEvent(object sender, System.EventArgs e)
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

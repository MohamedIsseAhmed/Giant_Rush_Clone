using System;
using UnityEngine;

public class FightinpointTriger : MonoBehaviour
{
    public static FightinpointTriger instance;
    [SerializeField] private Transform fightingPoint;
    public event Action<Vector3,Quaternion> StartFightingEvent;

    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
          
            StartFightingEvent.Invoke(fightingPoint.position, fightingPoint.rotation);
        }
    }
}

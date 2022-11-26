using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObstacleRotaion : MonoBehaviour
{
    [SerializeField] private float twinRotaionSpeed = 1;
    [SerializeField] private float angelToRotateAround;
    [SerializeField] private float startingAngel;

    [SerializeField] private bool shouldStopTweening = false;
    private int loopCount = -1;
   
    private void Start()
    {
        StopTweeningButton.instance.StopTweeningEvent += Ýnstance_StopTweeningEvent;
        Vector3 endValue=new Vector3(0,0,angelToRotateAround);
        if (!shouldStopTweening)
        {
            transform.DORotate(endValue, twinRotaionSpeed).SetLoops(loopCount, LoopType.Yoyo).OnUpdate(() =>
            {
                if (shouldStopTweening)
                {
                    loopCount = 1;
                    Vector3 endValue = new Vector3(0, 0, startingAngel);
                    transform.DORotate(endValue, twinRotaionSpeed);
                }
               
            });
        }
        

    }

    private void Ýnstance_StopTweeningEvent(object sender, System.EventArgs e)
    {
        shouldStopTweening = true;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.2f;
    [SerializeField] private float slowMotionLength = 2;

    public static SlowMotion instance;
    private bool shouldApplySlowMotion = false;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
       
        if (shouldApplySlowMotion)
        {
            Time.timeScale+= (1 / slowMotionLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        }

    }
    public void ApplySlowMotion()
    {
        shouldApplySlowMotion = true;
        Time.timeScale= slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale*0.02f;
    }
}

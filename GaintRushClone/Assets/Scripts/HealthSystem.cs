using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HealthSystem : MonoBehaviour
{
    [SerializeField] private HealthBarOwner Healthowner;
    [SerializeField] private Image healthBarPlayer;
    [SerializeField] private Image healthBoss;
    [SerializeField] private Transform parentRed;
    [SerializeField] private Transform parentGreen;

    [SerializeField] private float healthXSxaleTarget = 3.9f;
    [SerializeField] private float healtTohIncrease = 1f;
    [SerializeField] private float tweenScaleXTime = 0.25f;
    [SerializeField] private float parentTween = 0.25f;
    public static HealthSystem instance;
 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        FightinpointTriger.instance.StartFightingEvent += Ýnstance_StartFightingEvent;
    }

    private void Ýnstance_StartFightingEvent(Vector3 arg1, Quaternion arg2)
    {
        parentRed.gameObject.SetActive(true);
        parentRed.DOScale(Vector3.one, parentTween);
        parentGreen.gameObject.SetActive(true);
        parentGreen.DOScale(Vector3.one, parentTween);
    }

    public void IncreaseHealthBar(IDamageable damageable)
    {
        if(damageable is Player)
        {
            float targetx = healthBarPlayer.rectTransform.localScale.x + healtTohIncrease;
            healthBarPlayer.rectTransform.DOScaleX(targetx, tweenScaleXTime);
        }
        else if(damageable is BossManager)
        {
            
            float targetx = healthBoss.rectTransform.localScale.x + healtTohIncrease;
            healthBoss.rectTransform.DOScaleX(targetx, tweenScaleXTime);
        }
      
    }
}
public enum HealthBarOwner
{
    Player,
    Boss
}
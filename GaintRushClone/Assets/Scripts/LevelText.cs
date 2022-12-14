using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private Image backGround;
     private TextMeshPro plusMinusText;
    [SerializeField] private Transform level?ncrasedShower;
    [SerializeField] private float waitTime;
    private ScaleUpAnChangeColor scaleUp;
    private int levelNumber = 1;
    
    private void Awake()
    {
        scaleUp = GetComponent<ScaleUpAnChangeColor>();
        plusMinusText = level?ncrasedShower.GetComponent<TextMeshPro>();
    }
    private void Start()
    {
        FightinpointTriger.instance.StartFightingEvent += ?nstance_StartFightingEvent;
        scaleUp.OnColidedWithStickmanIncreaseLevelNumber += ScaleUp_OnColidedWithStickmanIncreaseLevelNumber;
        UpdateLevelText(1);
    }

    private void ?nstance_StartFightingEvent(Vector3 arg1, Quaternion arg2)
    {
        levelText.gameObject.SetActive(false);
        backGround.gameObject.SetActive(false);
        plusMinusText.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        FightinpointTriger.instance.StartFightingEvent -= ?nstance_StartFightingEvent;
        scaleUp.OnColidedWithStickmanIncreaseLevelNumber -= ScaleUp_OnColidedWithStickmanIncreaseLevelNumber;
    }
    private void ScaleUp_OnColidedWithStickmanIncreaseLevelNumber(object sender, int levelNumberMinusOrPlus)
    {
        UpdateLevelText(levelNumberMinusOrPlus);
    }
    private void UpdateLevelText(int levelNumberMinusOrPlus)
    {
        bool isPositive = levelNumberMinusOrPlus > 0;
        levelNumber= isPositive ? levelNumber+1 : levelNumber-1;
        StartCoroutine(AnimateLevelText(isPositive));
        
        levelText.SetText("Lv." + levelNumber);
    }
    IEnumerator AnimateLevelText(bool isPositve)
    {
        level?ncrasedShower.gameObject.SetActive(true);

        plusMinusText.text = isPositve ? "+1" : "-1";

        yield return new WaitForSeconds(waitTime);
        level?ncrasedShower.gameObject.SetActive(false);

    }
    public int GetLevelNumber()
    {
        return levelNumber;
    }
}

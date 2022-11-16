using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PunchWall : MonoBehaviour
{
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private int levelNumber = 1;

    private void Start()
    {
        levelText.text = levelNumber.ToString();
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }
}

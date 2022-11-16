using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWall : MonoBehaviour
{
    [SerializeField] private WallColoType wallColoType;

    [SerializeField] private Color red;
    [SerializeField] private Color green;
    [SerializeField] private Color yellow;
    [SerializeField] private Color blue;
    private void OnTriggerEnter(Collider other)
    {
        ChabgeColor(other);
    }

    private void ChabgeColor(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScaleUpAnChangeColor player = other.GetComponent<ScaleUpAnChangeColor>();
            switch (wallColoType)
            {
                case WallColoType.Red:

                    player.SetTargetTagAndColor("red", red);
                    break;
                case WallColoType.Green:
                    player.SetTargetTagAndColor("green", green);
                    break;
                case WallColoType.Blue:
                    player.SetTargetTagAndColor("blue", blue);
                    break;
                case WallColoType.Yellow:
                    player.SetTargetTagAndColor("yellow", yellow);
                    break;

            }
        }
    }
}
public enum WallColoType
{
    Red,
    Green,
    Blue,
    Yellow
}

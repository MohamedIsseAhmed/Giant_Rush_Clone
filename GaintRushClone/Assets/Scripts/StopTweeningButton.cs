using System;
using UnityEngine;

public class StopTweeningButton : MonoBehaviour
{
    [SerializeField] private Material greenMaterial;

    public event EventHandler StopTweeningEvent;
    private MeshRenderer meshRenderer;
    public static StopTweeningButton instance;
    private void Awake()
    {
        instance = this;
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("collided w/Player");
            meshRenderer.material.color = greenMaterial.color;
            StopTweeningEvent.Invoke(this, EventArgs.Empty);
        }
    }

}

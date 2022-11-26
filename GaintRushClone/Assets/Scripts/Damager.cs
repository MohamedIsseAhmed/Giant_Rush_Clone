using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private bool hasHitted = false;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.transform.GetComponent<IDamageable>();
        if (damageable != null && !hasHitted)
        {
            hasHitted = true;
            HealthSystem.instance.IncreaseHealthBar(damageable);
            damageable.TakeDame(1);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        hasHitted = false;
    }
}

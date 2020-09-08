using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public void Hit(float amount)
    {
        GetComponent<Health>().health -= amount;
        if(GetComponent<Health>().health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

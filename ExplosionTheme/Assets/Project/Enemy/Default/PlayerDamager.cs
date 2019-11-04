using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    [SerializeField] public float DamageAmount;
    [SerializeField] private float KnockbackAmount;

    public void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            target.GetComponent<Player>().takeDamage(DamageAmount);
        }
    }
}

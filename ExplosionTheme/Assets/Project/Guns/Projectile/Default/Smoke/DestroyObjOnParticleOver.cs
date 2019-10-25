using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjOnParticleOver : MonoBehaviour
{
    public ParticleSystem mySystem;

    private void FixedUpdate()
    {
        if (mySystem.IsAlive(false) == false)
        {
            Destroy(gameObject);
        }
    }
}

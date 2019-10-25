using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    [SerializeField]protected float slowDownSpeed = .01f;
    [SerializeField] protected float fuseTime = 2f;

    private float fuseTimer = 0f;

    protected void Update()
    {
        calculateSlowDown(Time.deltaTime);
        checkTime();
    }

    private void calculateSlowDown(float timePassed)
    {
        float amountToSlow = timePassed;

        myBody.velocity = myBody.velocity - (myBody.velocity.normalized * (amountToSlow* slowDownSpeed));
    }

    private void checkTime()
    {
        fuseTimer += Time.deltaTime;
        if (fuseTimer >= fuseTime)
        {
            explode();
        }
    }
}

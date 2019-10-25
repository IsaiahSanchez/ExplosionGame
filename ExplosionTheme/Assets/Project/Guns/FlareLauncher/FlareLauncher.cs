using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareLauncher : Gun
{
    private float AccuracyModifier = 0; //closer to 0 the better

    private float bufferInBetweenShotMax = .38f;
    private float bufferTimer = .38f;

    public override void Update()
    {
        base.Update();

        bufferTimer -= Time.deltaTime;
        if (bufferTimer <= 0)
        {
            AccuracyModifier = 0;
        }

    }


    protected override void shootBullet()
    {
        //plan aim
        float rotationModifier = Random.Range(-AccuracyModifier,AccuracyModifier);

        //shoot bullet
        justFired = Instantiate(Bullet, ShotSpawnLocation.position, Quaternion.Euler(0, 0, ShotSpawnLocation.rotation.eulerAngles.z + rotationModifier));
        justFired.transform.parent = null;
        justFired.GetComponent<Projectile>().StartMoving(justFired.transform.right);

        //set timer to keep chain going
        bufferTimer = bufferInBetweenShotMax;
        AccuracyModifier += 3;
        if (AccuracyModifier > 15)
        {
            AccuracyModifier = 15;
        }
    }

}

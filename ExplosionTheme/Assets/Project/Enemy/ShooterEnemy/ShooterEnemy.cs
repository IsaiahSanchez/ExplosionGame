using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField]private Transform hand;

    private Gun CurrentGunRef;
    [SerializeField] private List<Gun> gunsToChoose = new List<Gun>();

    private bool isStopped = false;
    [SerializeField] private float HoverDistanceFromPlayer = 10f;

    private float timer = 4f;
    [SerializeField]private float MaxTimeInBetweenShots = 4f;

    protected override void Awake()
    {
        base.Awake();
        //pick gun out and equip
        if (gunsToChoose.Count > 0)
        {
            CurrentGunRef = Instantiate(gunsToChoose[Random.Range(0, gunsToChoose.Count)], hand);
            CurrentGunRef.infiniteAmmo = true;
            CurrentGunRef.isPlayerGun = false;
            CurrentGunRef.gameObject.layer = 12;
        }
        
    }

    protected override void FixedUpdate()
    {
        if (isSpawnedIn == true)
        {
            handleShooterMovement();
            handleRepelForce();

            aimAtPlayer();

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                pullTrigger();
                timer = MaxTimeInBetweenShots;
            }
        }
    }

    private void handleShooterMovement()
    {
        if (playerRef != null)
        {
            mybody.velocity = currentAim * movementSpeed;
            if (isStopped == false)
            {

                float tempDistance = (playerRef.transform.position - transform.position).magnitude;
                //check if need to move up
                if ((tempDistance > HoverDistanceFromPlayer))
                {
                    //move towards enemy if not within a certain distance
                    currentAim = (playerRef.transform.position - transform.position).normalized;

                }
                else if (tempDistance < HoverDistanceFromPlayer)
                {
                    // and move out if within
                    currentAim = -(playerRef.transform.position - transform.position).normalized;
                }

            }

            //check if is within tolerance to stop moving
            if ((playerRef.transform.position - transform.position).magnitude > (HoverDistanceFromPlayer - 0.75f) &&
                (playerRef.transform.position - transform.position).magnitude < (HoverDistanceFromPlayer + 0.75f))
            {
                if (isStopped == false)
                {
                    mybody.velocity = new Vector2(0, 0);
                    currentAim = Vector2.zero;
                }
                isStopped = true;
            }
            else
            {
                isStopped = false;
            }
        }
    }

    private void aimAtPlayer()
    {
        //gunLocation.right = temp - new Vector2(transform.position.x, transform.position.y);
        if (playerRef != null)
        {
            hand.right = playerRef.transform.position - transform.position;
        }
    }

    private void pullTrigger()
    {
        CurrentGunRef.Fire();
    }

    //aim at player always
    //fire every so often and have a chance to choose not to
}

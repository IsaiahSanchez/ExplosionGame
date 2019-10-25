using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool isPlayerGun = true;

    [SerializeField]protected AudioClip gunShotSound;

    [SerializeField]protected float timeInBetweenShot = .25f;
    private float timeTilNextShot = 1f;
    [SerializeField]protected GameObject Bullet;
    [SerializeField]protected Transform ShotSpawnLocation;
    private bool canShoot = true;

    [SerializeField]public bool infiniteAmmo = false;
    public int currentAmmo = 5;
    public int maxAmmo;

    protected GameObject justFired;

    public void Awake()
    {
        maxAmmo = currentAmmo;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        timeTilNextShot = 0;
    }

    // Update is called once per frame
    public virtual void Update()
    { 
        if (canShoot == false)
        {
            timeTilNextShot -= Time.deltaTime;
            if (timeTilNextShot <= 0)
            {
                canShoot = true;
            }
        }
    }

    public virtual void Fire()
    {
        if (checkForEmpty())
        {
            canShoot = false;
        }

        if (canShoot == true)
        {
            //play sound
            AudioManager.instance.PlaySound("GunShot");

            //shoot
            shootBullet();
            if (isPlayerGun == true)
            {
                justFired.layer = 9;
            }
            else
            {
                justFired.layer = 10;
            }

            canShoot = false;
            timeTilNextShot = timeInBetweenShot;

            if (infiniteAmmo == false)
            {
                currentAmmo -= 1;
                checkForEmpty();
            }
        }
    }

    protected virtual void shootBullet()
    {
        justFired = Instantiate(Bullet, ShotSpawnLocation.position, ShotSpawnLocation.rotation);
        justFired.transform.parent = null;
        justFired.GetComponent<Projectile>().StartMoving(justFired.transform.right);
    }

    public bool checkForEmpty()
    {
        if (currentAmmo <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

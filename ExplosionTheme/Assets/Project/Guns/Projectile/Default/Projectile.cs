using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField]protected float speed = 10f;
    [SerializeField]private float ExplosionSize;
    [SerializeField] private GameObject smokeObj;

    public bool isPlayerProjectile;
    private bool canTriggerHit = true;

    protected Rigidbody2D myBody;

    private void Awake()
    {
        Instantiate(smokeObj, transform.position, transform.rotation);
    }

    protected virtual void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public virtual void StartMoving(Vector2 Direction)
    {
        myBody = GetComponent<Rigidbody2D>();
        transform.right = Direction;
        myBody.velocity = transform.right * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D target)
    {
        explode();
    }

    protected void explode()
    {
        canTriggerHit = false;
        ExplosionManager.instance.spawnExplosion(transform.position, ExplosionSize);
        Destroy(gameObject);
    }
}

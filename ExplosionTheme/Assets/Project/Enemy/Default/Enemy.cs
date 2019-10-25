using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> comrades = new List<Enemy>();

    [SerializeField]private int DamageAmount = 10;
    [SerializeField]protected float movementSpeed = 10f;
    [SerializeField]private float health = 10f;

    public AudioClip enemyHurt;
    public AudioClip enemyDead;

    private bool canBeDamaged = true;
    private float invulnerableTime = 0.05f;

    protected Player playerRef;
    protected Rigidbody2D mybody;

    protected Vector2 currentAim = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        comrades.Add(this);
        canBeDamaged = true;
        mybody = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(checkForPlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        handleMovement();
        handleRepelForce();
    }

    private void handleMovement()
    {
        if (playerRef != null)
        {
            Vector2 directionToAim = playerRef.transform.position - transform.position;
            directionToAim.Normalize();
            currentAim = currentAim.normalized + directionToAim;
            mybody.velocity = currentAim * movementSpeed;
        }
    }

    protected virtual void handleRepelForce()
    {
        float weight = 1f;
        foreach (Enemy current in comrades)
        {
            if (current != this)
            {
                if (!(current == null))
                {
                    Vector2 distance = transform.position - current.transform.position;
                    if (distance.magnitude < 1)
                    {
                        // ((mybody.velocity.normalized + distance.normalized) / 2)
                        currentAim += distance.normalized;
                        mybody.velocity = currentAim * movementSpeed;
                    }
                }
            }
        }
    }

    public void takeDamage(float amount)
    {
        if (canBeDamaged == true)
        {
            //play sound
            AudioManager.instance.PlaySound("EnemyHurt");

            //take damage
            health -= amount;
            if (health <= 0)
            {
                //play sound
               AudioManager.instance.PlaySound("EnemyDeath");

                //check if dead
                comrades.Remove(this);
                Destroy(gameObject);
            }
            //reset
            canBeDamaged = false;
            StartCoroutine(DamageTimer());

        }
    }

    private IEnumerator DamageTimer()
    {
        yield return new WaitForSeconds(invulnerableTime);
        canBeDamaged = true;
    }

    public void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            target.GetComponent<Player>().takeDamage(DamageAmount);
        }
    }

    private IEnumerator checkForPlayer()
    {
        if (playerRef == null)
        {
            playerRef = Player.instance;
            yield return new WaitForEndOfFrame();
            StartCoroutine(checkForPlayer());
        }

    }

}

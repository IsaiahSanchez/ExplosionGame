using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> comrades = new List<Enemy>();

    [SerializeField]private int DamageAmount = 10;
    [SerializeField]protected float movementSpeed = 10f;
    [SerializeField]private float health = 10f;
    [SerializeField]private PlayerDamager DamageBox;
    [SerializeField]private SpriteRenderer mySprite;
    [SerializeField]private GameObject spawnInAnimation;
    [SerializeField] private float sizeOfSpawnX = 1 , sizeOfSpawnY = 1;


    private GameObject spawnInAnimationRef;
    private bool canBeDamaged = true;
    private float invulnerableTime = 0.05f;

    protected Player playerRef;
    protected Rigidbody2D mybody;

    protected Vector2 currentAim = new Vector2(0, 0);
    protected bool isSpawnedIn = false;

    protected virtual void Awake()
    {
        mySprite.gameObject.SetActive(false);
        mybody = gameObject.GetComponent<Rigidbody2D>();
        comrades.Add(this);
        StartCoroutine(checkForPlayer());
        StartCoroutine(spawnIn());
    }

    private IEnumerator spawnIn()
    {
        GetComponent<Collider2D>().enabled = false;
        DamageBox.GetComponent<Collider2D>().enabled = false;
        //spawn animation
        spawnInAnimationRef = Instantiate(spawnInAnimation, transform.position, Quaternion.identity);
        spawnInAnimationRef.transform.localScale = new Vector2(sizeOfSpawnX, sizeOfSpawnY);
        AudioManager.instance.PlaySound("EnemySpawnIn");
        

        yield return new WaitForSeconds(2f);
        //after waiting 
        canBeDamaged = true;
        mySprite.gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = true;
        DamageBox.GetComponent<Collider2D>().enabled = true;
        DamageBox.DamageAmount = DamageAmount;
        isSpawnedIn = true;

        yield return new WaitForSeconds(.25f);

        Destroy(spawnInAnimationRef);

    }

    protected virtual void FixedUpdate()
    {
        if (isSpawnedIn == true)
        {
            handleMovement();
            handleRepelForce();
        }
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
        foreach (Enemy current in comrades)
        {
            if (current != this)
            {
                if (!(current == null))
                {
                    Vector2 distance = transform.position - current.transform.position;
                    if (distance.magnitude < 1)
                    {
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

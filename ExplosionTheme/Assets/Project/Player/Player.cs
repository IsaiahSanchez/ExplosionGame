using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //init / helper objects
    Rigidbody2D myBody;
    [SerializeField]GameObject selectedGun;
    GameObject gunRef;
    [SerializeField] Transform gunLocation;

    [SerializeField] GameObject mySpriteObject;
    public AudioClip PlayerHurtSound;

    //serialized variables
    [SerializeField] float playerMovementSpeed = 10f;
    [SerializeField]
    GameObject testGun, testGun2, testGun3, testGun4, defaultGun;
    bool infiniteAmmo = false;

    //regular variables
    private float health = 100;
    public float maxHealth = 50f;
    private bool canBeDamaged = true;
    private float invulnerableTime = .1f;
    private bool isHoldingTrigger = false;

    public static Player instance;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        myBody = gameObject.GetComponent<Rigidbody2D>();
        ChangeGun(selectedGun);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check input
        CheckInputs();
        if (isHoldingTrigger == true)
        {
            FireGun();
        }

        checkForOutOfAmmo();

        if (gunRef == null)
        {
            ChangeGun(defaultGun);
        }


        Vector2 temp = getMouseInWorldCoords();
        gunLocation.right = temp - new Vector2(transform.position.x, transform.position.y);
    }

    private void FixedUpdate()
    {
        //move
        myBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerMovementSpeed, Input.GetAxisRaw("Vertical") * playerMovementSpeed*.9f);
    }

    private void checkForOutOfAmmo()
    {
        if (gunRef.GetComponent<Gun>().checkForEmpty())
        {
            ChangeGun(default);
        }
    }

    public void ChangeGun(GameObject newGun)
    {
        if (newGun != null)
        {
            selectedGun = newGun;
        }
        else
        {
            selectedGun = defaultGun;
        }

        GameObject.Destroy(gunRef);
        gunRef = Instantiate(selectedGun, gunLocation);
        gunRef.GetComponent<Gun>().isPlayerGun = true;
        updateAmmoBar();
    }

    private void FireGun()
    {
        getMouseInWorldCoords();
        gunRef.GetComponent<Gun>().Fire();
        updateAmmoBar();

        Debug.Log("Player Fire!");
    }

    private void updateAmmoBar()
    {
        if (gunRef != null)
        {
            Gun currentGun = gunRef.GetComponent<Gun>();

            if (GameManager.instance != null)
            {
                GameManager.instance.playerAmmoHasChanged(currentGun.currentAmmo, currentGun.maxAmmo);
            }
        }
    }

    private Vector2 getMouseInWorldCoords()
    {
        Camera view = Camera.main;
        Vector2 temp = view.ScreenToWorldPoint(Input.mousePosition);
        return temp;
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireGun();
            isHoldingTrigger = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isHoldingTrigger = false;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeGun(testGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeGun(testGun2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeGun(testGun3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeGun(testGun4);
        }

        
    }

    public void takeDamage(float amount)
    {
        if (canBeDamaged == true)
        {
            //take damage
            health -= amount;
            GameManager.instance.playerHealthChanged(health, maxHealth);
            if (health <= 0)
            {
                //if < 0 == dead
                Debug.Log("he's ded jim");
                GameManager.instance.GameOver();
                Destroy(gameObject);
            }
            //play sound
            AudioManager.instance.PlaySound("PlayerHurt");

            //reset
            canBeDamaged = false;
            StartCoroutine(DamageTimer());
            StartCoroutine(FlashFromDamage());
        }
    }

    private IEnumerator DamageTimer()
    {
        yield return new WaitForSeconds(invulnerableTime);
        canBeDamaged = true;
    }

    private IEnumerator FlashFromDamage()
    {
        mySpriteObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.3f);
        mySpriteObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}

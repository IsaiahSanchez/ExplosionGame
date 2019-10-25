using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float TimeExplosionLasts = 2f;
    [SerializeField] private float damageModifier = 2f;
    private float timeDamageLasts = .05f;
    private CircleCollider2D myCircle;

    [SerializeField] private GameObject explosionParticles;
    private GameObject particlesRef;

    // Start is called before the first frame update
    void Start()
    {
        myCircle = GetComponent<CircleCollider2D>();
        StartCoroutine(waitToDelete());
        StartCoroutine(timeDamageIsActive());
        particlesRef = Instantiate(explosionParticles, transform);
    }

    private IEnumerator waitToDelete()
    {
        yield return new WaitForSeconds(TimeExplosionLasts);
        Destroy(particlesRef);
        Destroy(gameObject);
    }

    private IEnumerator timeDamageIsActive()
    { 
        yield return new WaitForSeconds(timeDamageLasts);
        myCircle.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        
        if (target.tag == "Player")
        {
            target.GetComponent<Player>().takeDamage(transform.localScale.x*damageModifier);

        }

        if (target.tag == "Enemy")
        {
            target.GetComponent<Enemy>().takeDamage(transform.localScale.x * damageModifier);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance;

    [SerializeField] private GameObject explosion;
    public AudioClip explosionSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            float randomX = Random.Range(-13f,13f);
            float randomY = Random.Range(-7f, 7f);
            float randomSize = Random.Range(1f, 4f);
            spawnExplosion(new Vector2(randomX, randomY), randomSize);
        }
    }

    public void spawnExplosion(Vector2 Location, float Size)
    {
        if (explosion != null)
        {
            AudioManager.instance.PlaySound("Explosion");
            Vector3 minExplosion = new Vector3(.005f, .005f, .005f);
            CameraShake.instance.addShake(minExplosion.x *  (Size*Size), minExplosion.y* (Size * Size), minExplosion.z * (Size * Size), .1f);

            GameObject temp = Instantiate(explosion, Location, Quaternion.identity);
            temp.transform.localScale = new Vector2(1,1)*Size;
        }
    }
}

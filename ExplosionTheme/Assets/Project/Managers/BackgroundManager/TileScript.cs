using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private bool spawnedIn = true;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (spawnedIn == true)
        { 
            spawnOut();
        }
    }

    public void spawnIn()
    {
        GetComponent<Animator>().ResetTrigger("spawnIn");
        GetComponent<Animator>().SetTrigger("SpawnIn");
        BackgroundManager.instance.unSpawnedObjects.Remove(gameObject);
        BackgroundManager.instance.spawnedInObjects.Add(gameObject);
        spawnedIn = true;
        AudioManager.instance.PlaySound("TileSpawnIn");
    }

    public void spawnOut()
    {
        GetComponent<Animator>().ResetTrigger("spawnOut");
        GetComponent<Animator>().SetTrigger("SpawnOut");
        BackgroundManager.instance.spawnedInObjects.Remove(gameObject);
        BackgroundManager.instance.unSpawnedObjects.Add(gameObject);
        spawnedIn = false;
    }
}

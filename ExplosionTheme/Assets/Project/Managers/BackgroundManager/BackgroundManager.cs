using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    [SerializeField] private int maxX, maxY;
    [SerializeField] private GameObject tileToSpawn;
    [SerializeField] private float timeInBetweenSpawns;

    public List<GameObject> spawnedInObjects = new List<GameObject>();
    public List<GameObject> unSpawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int XIndex = 0; XIndex < maxX; XIndex++)
        {
            for (int YIndex = 0; YIndex < maxY; YIndex++)
            {
                GameObject temp = Instantiate(tileToSpawn, new Vector2(transform.position.x + XIndex, transform.position.y + YIndex),Quaternion.identity,transform);
                spawnedInObjects.Add(temp);
                temp.SetActive(false);
            }
        }

        foreach (GameObject obj in spawnedInObjects)
        {
            StartCoroutine(delayBeforeActive(Random.Range(.1f, 1f), obj));
        }
        //start respawning if we can
        StartCoroutine(respawningSquares());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            debugBackgroundClear();
        }
    }

    private void debugBackgroundClear()
    {
        while (spawnedInObjects.Count > 0)
        {
            spawnedInObjects[0].GetComponent<Animator>().SetTrigger("SpawnOut");
            unSpawnedObjects.Add(spawnedInObjects[0]);
            spawnedInObjects.RemoveAt(0);
        }
    }

    private IEnumerator delayBeforeActive(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(true);
    }

    private IEnumerator respawningSquares()
    {
        yield return new WaitForSeconds(timeInBetweenSpawns);
        if (unSpawnedObjects.Count > 0)
        {
            int rand = 0;
            //respawn 1 object randomly
            if (unSpawnedObjects.Count > 1)
            {
                rand = Random.Range(0, unSpawnedObjects.Count);
            }
            unSpawnedObjects[rand].GetComponent<TileScript>().spawnIn();
        }

        StartCoroutine(respawningSquares());
    }
}

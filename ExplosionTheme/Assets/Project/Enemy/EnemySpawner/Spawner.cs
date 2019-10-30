using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static List<Spawner> spawners = new List<Spawner>();

    //list
    [SerializeField] private List<Enemy> whatToSpawn = new List<Enemy>();

    [SerializeField] Transform farRight, upTop;

    //chance to pause
    [SerializeField] private float ChanceToSkipOutOfOneHundred = 20f;

    public bool isSpawning = false;

    //delays
    private float startDelay = 1f;
    private float DelayInBetweenMiniWaves = 5f;
    private float naturalSpawnTime = .5f;

    //gates to let delay finish
    private bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        spawners.Add(this);
    }

    public void StartWave()
    {
        StartCoroutine(Delay(startDelay + 2));
        StartCoroutine(WaveSpawning());
        isSpawning = true;
    }

    private IEnumerator WaveSpawning()
    {
        yield return new WaitForSeconds(naturalSpawnTime);
        if (canSpawn == true)
        {
            spawnNextEnemy();
            float rand = Random.Range(0, 100);
            //Debug.Log(rand);
            if (rand <= ChanceToSkipOutOfOneHundred)
            {
                Debug.Log("skipped");
                StartCoroutine(Delay(DelayInBetweenMiniWaves));
            }
        }

        if (!(whatToSpawn.Count <= 0))
        {
            StartCoroutine(WaveSpawning());
        }
        else
        {
            isSpawning = false;
        }

    }

    private IEnumerator Delay(float amount)
    {
        canSpawn = false;
        yield return new WaitForSeconds(amount);
        canSpawn = true;
    }

    private void spawnNextEnemy()
    {
        if (whatToSpawn.Count == 0)
        {

        }
        else
        {
            Instantiate(whatToSpawn[0], getNewSpawnPosition(), transform.rotation);
            whatToSpawn.RemoveAt(0);
        }
    }

    private Vector2 getNewSpawnPosition()
    {
        //pick random location based on x and y
        float xPos = Random.Range(transform.position.x,farRight.position.x);
        float yPos = Random.Range(transform.position.y,upTop.position.y);

        return new Vector2(xPos, yPos);
    } 

    //setters
    public void AddNewEnemy(Enemy nextEnemy)
    {
        whatToSpawn.Add(nextEnemy);
    }

    public void setStartDelay(float delay)
    {
        startDelay = delay;
    }

    public void setMiniWaveDelay(float delay)
    {
        DelayInBetweenMiniWaves = delay;
    }

}

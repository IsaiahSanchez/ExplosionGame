using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private List<Enemy> MasterList = new List<Enemy>();
    [SerializeField] private List<Enemy> ListOfEnemiesToSpawn = new List<Enemy>();
    [SerializeField]private Text roundNumberText;
    [SerializeField]private Text playerHealthText;

    private int round = 0;
    private bool isListening = false;
    public bool hasStartedGame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Update()
    {
        if (hasStartedGame == true)
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                Spawner.spawners.Clear();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                StartNewRound();
            }

            if (isListening == true)
            {
                bool roundIsDone = true;
                foreach (Spawner spawner in Spawner.spawners)
                {
                    if (spawner.isSpawning == true)
                    {
                        roundIsDone = false;
                    }
                }
                if (Enemy.comrades.Count != 0)
                {
                    roundIsDone = false;
                }

                Debug.Log(roundIsDone);
                if (roundIsDone == true)
                {
                    isListening = false;
                    roundIsDone = false;
                    StartNewRound();

                }
            }
        }
    }

    public void StartNewRound()
    {
        round++;
        
        GameManager.instance.RoundHasChanged(round);

        //generate List of Enemies for this round
        GenerateEnemyList();

        //distribute enemies to the spawners
        DistributeEnemiesToSpawners();

        //start the round for each
        StartRound();


        hasStartedGame = true;
        StartListening();
    }

    private void GenerateEnemyList()
    {
        int numberOfEnemies = 0;
        //determine number of enemies based on round number
        //(x-3)^2-3
        numberOfEnemies = ((round - 3) * (round - 3)) +8;
        if (numberOfEnemies > 0)
        {
            for (int index = 0; index < numberOfEnemies; index++) 
            {
                int temp = Random.Range(0, ListOfEnemiesToSpawn.Count);
                MasterList.Add(ListOfEnemiesToSpawn[temp]);
            }
        }
        else
        {
            Debug.Log("Error had 0 enemies in the spawning");
        }

    }

    private void DistributeEnemiesToSpawners()
    {
        int currentIndex = 0;
        while (MasterList.Count > 0)
        {
            //decide if we are depositing in this spawner
            if (!(Random.Range(0, 100) < 35))
            {
                //if yes then deposit 
                Spawner.spawners[currentIndex].AddNewEnemy(MasterList[0]);
                MasterList.RemoveAt(0);
            }

            //increment the thing and loop
            currentIndex++;
            if (currentIndex >= Spawner.spawners.Count)
            {
                currentIndex = 0;
            }
        }
    }

    private void StartRound()
    {
        foreach (Spawner spawner in Spawner.spawners)
        {
            spawner.setMiniWaveDelay(Random.Range(2f, 7f));
            spawner.setStartDelay(Random.Range(0.5f, 5f));
            spawner.StartWave();
        }
    }

    private void StartListening()
    {
        isListening = true;
    }

}


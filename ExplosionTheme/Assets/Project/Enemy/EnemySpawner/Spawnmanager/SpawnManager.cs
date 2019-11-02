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

    public int round = 0;
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

        if (round >= 4)
        {
            //generate List of Enemies for this round
            GenerateEnemyList();
            //distribute enemies to the spawners
            DistributeEnemiesToSpawners();
        }
        else
        {
            switch (round)
            {
                case 1:
                    generateTutorialRoundEnemyList(12, 0);
                    List<int> temp = new List<int>();
                    temp.Add(0); temp.Add(1);
                    distributeTutorialEnemies(temp);
                    break;
                case 2:
                    generateTutorialRoundEnemyList(10, 2);
                    List<int> temp1 = new List<int>();
                    temp1.Add(2); temp1.Add(3);
                    distributeTutorialEnemies(temp1);
                    break;
                case 3:
                    generateTutorialRoundEnemyList(8, 3);
                    List<int> temp2 = new List<int>();
                    temp2.Add(0); temp2.Add(1); temp2.Add(2); temp2.Add(3);
                    distributeTutorialEnemies(temp2);
                    break;
                default:
                    break;
            }
        }
        //start the round for each
        StartRound();


        hasStartedGame = true;
        StartListening();
    }

    private void generateTutorialRoundEnemyList(int numberOfEnemies, int enemyType)
    {
        for (int index = 0; index < numberOfEnemies; index++)
        {
            MasterList.Add(ListOfEnemiesToSpawn[enemyType]);
        }
    }

    private void distributeTutorialEnemies(List<int> indexOfSpawnersToDistributeTo)
    {
        while(MasterList.Count>0)
        { 
            Spawner.spawners[indexOfSpawnersToDistributeTo[Random.Range(0,indexOfSpawnersToDistributeTo.Count)]].AddNewEnemy(MasterList[0]);
            MasterList.RemoveAt(0);
        }
    }

    private void GenerateEnemyList()
    {
        int numberOfEnemies = 0;
        //determine number of enemies based on round number
        //(x-3)^2-3
        numberOfEnemies = (round - 3)*3;
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private Transform top, Right;
    [SerializeField] GameObject whatToSpawn;
    [SerializeField] List<ScripWeapon> typesOfWeapons = new List<ScripWeapon>();

    private GameObject weaponRef = null;

    //temporary
    private float timeInBetweenPowerups = 10f;

    // Start is called before the first frame update
    void Start()
    {
        //spawnNewPowerup();
        StartCoroutine(CountSpawing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //temporary 
    private IEnumerator CountSpawing()
    {
        spawnNewPowerup();
        yield return new WaitForSeconds(timeInBetweenPowerups);

        StartCoroutine(CountSpawing());
    }

    private void spawnNewPowerup()
    {
        Vector2 WhereToSpawn = new Vector2(0, 0);
        WhereToSpawn.x = Random.Range(-Right.position.x, Right.position.x);
        WhereToSpawn.y = Random.Range(-top.position.y, top.position.y);

        if (weaponRef != null)
        {
            weaponRef.GetComponent<WeaponPickup>().Delete();
        }
        weaponRef = Instantiate(whatToSpawn, WhereToSpawn,transform.rotation);

        if (SpawnManager.instance != null)
        {
            if (SpawnManager.instance.round >= 4)
            {
                weaponRef.GetComponent<WeaponPickup>().setWeapon(typesOfWeapons[Random.Range(0, typesOfWeapons.Count)]);
            }
            else
            {
                switch (SpawnManager.instance.round)
                {
                    case 1:
                        weaponRef.GetComponent<WeaponPickup>().setWeapon(typesOfWeapons[0]);
                        break;
                    case 2:
                        weaponRef.GetComponent<WeaponPickup>().setWeapon(typesOfWeapons[1]);
                        break;
                    case 3:
                        weaponRef.GetComponent<WeaponPickup>().setWeapon(typesOfWeapons[2]);
                        break;
                    default:
                        weaponRef.GetComponent<WeaponPickup>().setWeapon(typesOfWeapons[0]);
                        break;
                }
            }
        }
    }


}

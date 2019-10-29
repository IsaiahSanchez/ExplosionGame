using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    [SerializeField]private ScripWeapon weaponChoice;
    private float GraphicScale = 2f;
    private GameObject spawnedWeapon;
    [SerializeField] private GameObject Graphic;

    private void Start()
    {
        Graphic.GetComponent<SpriteRenderer>().sprite = null;
        spawnedWeapon = Instantiate(weaponChoice.WeaponRef, transform.position, transform.rotation);
        spawnedWeapon.transform.localScale = new Vector3(spawnedWeapon.transform.localScale.x * GraphicScale, spawnedWeapon.transform.localScale.y * GraphicScale, 1);
        spawnedWeapon.transform.parent = Graphic.gameObject.transform;
    }

    public void setWeapon(ScripWeapon weaponChosen)
    {
        weaponChoice = weaponChosen;
    }

    public void Delete()
    {
        Destroy(spawnedWeapon);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            if (weaponChoice != null)
            {
                target.GetComponent<Player>().ChangeGun(weaponChoice.WeaponRef);
                AudioManager.instance.PlaySound("WeaponCollect");
                Delete();
            }

        }
    }
}

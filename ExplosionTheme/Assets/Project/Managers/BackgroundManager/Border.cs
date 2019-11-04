using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private GameObject borderTile;

    // Start is called before the first frame update
    void Start()
    {
        //top
        for (int index = 0; index < 14; index++)
        {
            GameObject Tile = Instantiate(borderTile, new Vector2(transform.position.x, transform.position.y + index),Quaternion.identity);
            Tile.GetComponentInChildren<SpriteRenderer>().color = new Vector4(1, 0, 0, 1);
        }
    }
}

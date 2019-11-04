using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSquisher : MonoBehaviour
{
    private bool isMoving = false;

    [SerializeField] private float maxSizeModifier = .125f;
    [SerializeField] private float normalSize = 1f;
    [SerializeField] private float minSizeModifier = .5f;
    [SerializeField]private float speedToChange = .1f;
    private float currentSize = 1f;

    private Rigidbody2D myBody;

    // Start is called before the first frame update
    void Start()
    {
        currentSize = normalSize;
        myBody = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myBody.velocity.magnitude > 0)
        {
            if (isMoving != true)
            {
                isMoving = true;
                GetComponentInParent<Animator>().SetTrigger("startMoving");
            }
        }
        else
        {
            if (isMoving != false)
            {
                isMoving = false;
                GetComponentInParent<Animator>().SetTrigger("isNotMoving");
            }
        }

        //if (isMoving == true)
        //{
        //    squishMore(Time.deltaTime);
        //}
        //else
        //{
        //    springBack(Time.deltaTime);
        //}
        //transform.localScale = new Vector2(1, currentSize);

        transform.right = myBody.velocity.normalized; 
    }

    private void squishMore(float time)
    {
        if (currentSize > normalSize - minSizeModifier)
        {
            currentSize -= (speedToChange * time);
        }
        else
        {
            currentSize = normalSize - minSizeModifier;
        }
    }

    private void springBack(float time)
    {
        if (currentSize < normalSize + maxSizeModifier)
        {
            currentSize += (speedToChange * time);
        }
        else
        {
            currentSize = normalSize;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 startingLocation;
    private float duration = 0f;
    private float nextXModifier = 0f;
    private float nextYModifier = 0f;
    private float nextRotationModifier = 0f;


    private bool shouldShake = false;

    private float xIntensity = 0;
    private float yIntensity = 0;
    private float rotationIntensity = 0;

    private float maxXIntensity = 2f;
    private float maxYIntensity = 2f;
    private float maxRotationIntensity = 1.5f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (shouldShake == true)
            {
                duration -= Time.deltaTime;
                //setNextLocation
                ShakeX();
                ShakeY();
                rotateShake();

                //move
                transform.position = new Vector3(startingLocation.x + nextXModifier, startingLocation.y + nextYModifier, startingLocation.z);
                transform.eulerAngles = new Vector3(0, 0, nextRotationModifier);


                //test if should move back and set should shake to false
                if (duration <= 0)
                {
                    resetShake();

                }
            }
            else
            {
                startingLocation = transform.position;
            }
        }
    }


    public void addShake(float XIntensity, float YIntensity, float RotationIntensity, float Duration)
    {
        xIntensity += XIntensity;
        if (xIntensity > maxXIntensity)
        {
            xIntensity = maxXIntensity;
        }

        yIntensity += YIntensity;
        if (yIntensity > maxYIntensity)
        {
            yIntensity = maxYIntensity;
        }

        rotationIntensity += RotationIntensity;
        if (rotationIntensity > maxRotationIntensity)
        {
            rotationIntensity = maxRotationIntensity;
        }
        duration = Duration;

        shouldShake = true;
    }

    private void resetShake()
    {
        shouldShake = false;
        transform.position = startingLocation;
        transform.rotation = Quaternion.identity;

        xIntensity = 0;
        yIntensity = 0;
        rotationIntensity = 0;
        duration = 0;
    }

    private void ShakeX()
    {
        nextXModifier = Random.Range(-1, 1) * xIntensity;
    }

    private void ShakeY()
    {
        nextYModifier = Random.Range(-1, 1) * yIntensity;
    }

    private void rotateShake()
    {
        nextRotationModifier = Random.Range(-1, 1) * rotationIntensity;
    }

}

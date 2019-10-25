﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator fadeToBlackAnim;

    [SerializeField]GameObject gameOverPanel, pauseMenu;

    [SerializeField]Text RoundText, RoundNumberText, ammoText;
    [SerializeField]Slider playerHealthBar, ammoBar;
    private float RoundTextOpacity = 0f, fadeInModifier = 1f;

    private float RoundOneStartTime = 2f;

    public bool isTestMode = false;
    private bool PlayerSelectedSomethingAlready = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        SceneFader.instance.FadeToClear();

        if (!isTestMode)
        {
            StartCoroutine(waitForRoundStart());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerAmmoHasChanged(float ammo, float ammoMax)
    {
        ammoBar.value = ammo / ammoMax;
        ammoText.text = ammo.ToString();
    }

    public void playerHealthChanged(float health, float maxHealth)
    {
        playerHealthBar.value = health / maxHealth;
    }

    private IEnumerator waitForRoundStart()
    {
        yield return new WaitForSeconds(RoundOneStartTime);
        SpawnManager.instance.StartNewRound();
    }

    public void MainMenu()
    {
        
            PlayerSelectedSomethingAlready = true;
            StartCoroutine(coMainMenu());
        
    }
    private IEnumerator coMainMenu()
    {
        //set gamespeed to full
        Time.timeScale = 1f;
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);

        //go to main menu
        ClearBeforeReset();
        SceneManager.LoadScene(0);

    }

    public void reloadGame()
    {
        
            PlayerSelectedSomethingAlready = true;
            StartCoroutine(coReloadGame());
        
    }
    private IEnumerator coReloadGame()
    {
        //set gamespeed to full
        Time.timeScale = 1f;
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);

        ClearBeforeReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void pauseGame()
    {
        //set gamespeed to 0
        Time.timeScale = 0;
        //set panel to active
    }

    public void GameOver()
    {
        StartCoroutine(coGameOver());
    }
    private IEnumerator coGameOver()
    {
        yield return new WaitForSeconds(1.35f);

        SceneFader.instance.FadeToBlack();
        yield return new WaitForSeconds(.7f);
        gameOverPanel.SetActive(true);
        SceneFader.instance.FadeToClear();
        yield return new WaitForSeconds(.7f);
        //set gamespeed to 0
        //Time.timeScale = 0;
    }

    public void RoundHasChanged(int roundNumber)
    {
        StartCoroutine(coFadeInRoundNum());
        RoundNumberText.text = roundNumber.ToString();
    }
    private IEnumerator coFadeInRoundNum()
    {
        //fade in
        StartCoroutine(roundNumberFadeIn());
        //wait time
        yield return new WaitForSeconds(2.5f);
        //fade out
        StartCoroutine(roundNumberFadeOut());
    }

    private IEnumerator roundNumberFadeIn()
    {
        yield return new WaitForEndOfFrame();
        RoundTextOpacity += Time.deltaTime * fadeInModifier;
        //set both opacity
        RoundText.color = new Vector4(0, 0, 0, RoundTextOpacity);
        RoundNumberText.color = new Vector4(0, 0, 0, RoundTextOpacity);
        if (RoundTextOpacity >= 1f)
        {
            RoundTextOpacity = 1f;
            RoundText.color = new Vector4(0, 0, 0, RoundTextOpacity);
            RoundNumberText.color = new Vector4(0, 0, 0, RoundTextOpacity);
        }
        else
        {
            StartCoroutine(roundNumberFadeIn());
        }
    }

    private IEnumerator roundNumberFadeOut()
    {
        yield return new WaitForEndOfFrame();
        RoundTextOpacity -= Time.deltaTime * fadeInModifier;
        //set both opacity
        RoundText.color = new Vector4(0, 0, 0, RoundTextOpacity);
        RoundNumberText.color = new Vector4(0, 0, 0, RoundTextOpacity);
        if (RoundTextOpacity <= 0)
        {
            RoundTextOpacity = 0;
            RoundText.color = new Vector4(0, 0, 0, RoundTextOpacity);
            RoundNumberText.color = new Vector4(0, 0, 0, RoundTextOpacity);
        }
        else
        {
            StartCoroutine(roundNumberFadeOut());
        }
    }

    private void ClearBeforeReset()
    {
        Spawner.spawners.Clear();
        Enemy.comrades.Clear();
        SpawnManager.instance.hasStartedGame = false;
    }
}

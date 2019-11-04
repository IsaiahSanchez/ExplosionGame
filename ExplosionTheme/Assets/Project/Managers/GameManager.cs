using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator fadeToBlackAnim;

    [SerializeField]private GameObject gameOverPanel, pauseMenu;

    [SerializeField] private Text RoundText, RoundNumberText, ammoText;
    [SerializeField] private Slider playerHealthBar, ammoBar;
    [SerializeField] private GameObject cursorGraphic;
    private float RoundTextOpacity = 0f, fadeInModifier = 1f;

    private float RoundOneStartTime = 2f;
    private GameObject currentCursor;

    public bool isTestMode = false;
    private bool PlayerSelectedSomethingAlready = false;
    private bool canPause = false;

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

        canPause = false;
        SceneFader.instance.FadeToClear();

        if (!isTestMode)
        {
            StartCoroutine(waitForRoundStart());
        }
        currentCursor = Instantiate(cursorGraphic, Player.instance.getMouseInWorldCoords(), Quaternion.identity);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
            currentCursor.transform.position = Player.instance.getMouseInWorldCoords();
        

        if (Input.GetKeyDown(KeyCode.P) && canPause == true)
        {
            pauseGame();
        }
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
        canPause = true;
    }

    public void MousedOver()
    {
        AudioManager.instance.PlaySound("MenuButtonOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySound("MenuButtonClick");    
        PlayerSelectedSomethingAlready = true;
        StartCoroutine(coMainMenu());
        
    }
    private IEnumerator coMainMenu()
    {
        //set gamespeed to full
        Time.timeScale = 1f;
        canPause = false;
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);

        //go to main menu
        ClearBeforeReset();
        Destroy(MusicSingleton.Instance.gameObject);
        SceneManager.LoadScene(0);

    }

    public void reloadGame()
    {
        AudioManager.instance.PlaySound("MenuButtonClick");
        PlayerSelectedSomethingAlready = true;
        StartCoroutine(coReloadGame());
        Cursor.visible = false;

    }
    private IEnumerator coReloadGame()
    {
        //set gamespeed to full
        Time.timeScale = 1f;
        canPause = false;
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);

        ClearBeforeReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void pauseGame()
    {
        AudioManager.instance.PlaySound("MenuButtonClick");
        //set gamespeed to 0
        Time.timeScale = 0;
        //set panel to active
        pauseMenu.SetActive(true);
        Cursor.visible = true;
    }

    public void continuePlaying()
    {
        AudioManager.instance.PlaySound("MenuButtonClick");
        //set gamespeed to 0
        Time.timeScale = 1;
        //set panel to active
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void GameOver()
    {
        Cursor.visible = true;
        canPause = false;
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
        AudioManager.instance.PlaySound("RoundChange");
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
        RoundText.color = new Vector4(RoundText.color.r, RoundText.color.g, RoundText.color.b, RoundTextOpacity);
        RoundNumberText.color = new Vector4(RoundNumberText.color.r, RoundNumberText.color.g, RoundNumberText.color.b, RoundTextOpacity);
        if (RoundTextOpacity >= 1f)
        {
            RoundTextOpacity = 1f;
            RoundText.color = new Vector4(RoundText.color.r, RoundText.color.g, RoundText.color.b, RoundTextOpacity);
            RoundNumberText.color = new Vector4(RoundNumberText.color.r, RoundNumberText.color.g, RoundNumberText.color.b, RoundTextOpacity);
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
        RoundText.color = new Vector4(RoundText.color.r, RoundText.color.g, RoundText.color.b, RoundTextOpacity);
        RoundNumberText.color = new Vector4(RoundNumberText.color.r, RoundNumberText.color.g, RoundNumberText.color.b, RoundTextOpacity);
        if (RoundTextOpacity <= 0)
        {
            RoundTextOpacity = 0;
            RoundText.color = new Vector4(RoundText.color.r, RoundText.color.g, RoundText.color.b, RoundTextOpacity);
            RoundNumberText.color = new Vector4(RoundNumberText.color.r, RoundNumberText.color.g, RoundNumberText.color.b, RoundTextOpacity);
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

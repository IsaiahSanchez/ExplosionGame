using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void Start()
    {
        StartCoroutine(waitToFade());
    }

    private IEnumerator waitToFade()
    {
        yield return new WaitForSeconds(.1f);
        SceneFader.instance.FadeToClear();
    }

    public void MousedOver()
    {
        AudioManager.instance.PlaySound("MenuButtonOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        AudioManager.instance.PlaySound("MenuButtonClick");
        StartCoroutine(coStartGame());
    }
    private IEnumerator coStartGame()
    {
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }
}

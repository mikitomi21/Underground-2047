using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }
    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadSceneAsync("Level1");
        StartCoroutine(StartGame("Level1"));
    }

    public void OnExitToDesktopButtonPressed()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

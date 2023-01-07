using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS };

public class GameManager : MonoBehaviour
{

    public GameState currentGameState = GameState.GS_PAUSEMENU;

    public static GameManager instance;
    public Canvas inGameCanvas;
    public Canvas optionsCanvas;

    public TMP_Text scoreText;
    public TMP_Text enemyText;
    public TMP_Text timeText;
    public TMP_Text highScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text quality;
    private int score = 0;
    private int enemyDead = 0;

    public Image[] keysTab;
    public Image[] livesTab;

    private int keysFound = 0;
    private int lives = 3;

    private float timer = 0;

    public Canvas pauseMenuCanvas;
    public Canvas levelCompleted;

    const string keyHighScore = "HighScoreLevel1";

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void AddDeads(int points)
    {
        enemyDead += points;
        enemyText.text = enemyDead.ToString();
    }

    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        inGameCanvas.enabled = true;

        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        levelCompleted.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);

        if (newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene=SceneManager.GetActiveScene();
            if(currentScene.name == "Level1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if(highScore < score)
                {
                    PlayerPrefs.SetInt(keyHighScore, score);
                    highScore = score;
                }
                currentScoreText.text = "Your score = " + score.ToString();
                highScoreText.text = "The best score = " + highScore.ToString();
            }
        }

    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    void Awake()
    {
 
        quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        optionsCanvas.enabled = false;
        instance = this;
        scoreText.text = score.ToString();
        enemyText.text = enemyDead.ToString();
        for (int i = 0; i < 3; i++)
        {
            keysTab[i].color= Color.grey;
        }

        for (int i = 0; i < 3; i++)
        {
            livesTab[i].color = Color.white;
        }
        livesTab[3].color = Color.grey;
        timeText.text = string.Format("{0:00}:{1:00}", (int)timer / 60, (int)timer % 60);

        InGame();

        if(!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
    }

    public void AddKeys()
    {
        keysTab[keysFound].color = Color.white;
        keysFound++;
    }

    public void StoleLives()
    {
        lives--;
        livesTab[lives].color = Color.grey;
    }

    public void AddLives()
    {
        livesTab[lives].color = Color.white;
        lives++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            

        }

        if(currentGameState != GameState.GS_PAUSEMENU) timer += Time.deltaTime;

        timeText.text = string.Format("{0:00}:{1:00}", (int)timer/60,(int)timer%60);


    }

    public void OnResumeButtonClick()
    {
        InGame();
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnOptionsButtonClick()
    {
        Options();
    }
    public void OnPlusClick()
    {
        QualitySettings.IncreaseLevel();
        quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void OnMinusClick()
    {
        QualitySettings.DecreaseLevel();
        quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }


    public void OnReturnButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public int getKeys()
    {
        return keysFound;
    }

    public int getLives()
    {
        return lives;
    }
}

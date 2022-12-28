using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //HUD
    public TMP_Text scoreText;
    public TMP_Text enemyText;
    public TMP_Text timerText;

    //LevelCompletedCanvas
    public TMP_Text highScoreText;
    public TMP_Text lvlCompletedScoreText;

    private int score = 0;
    private int enemiesKilled = 0;
    public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS }
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;

    public int keysFound = 0;
    public Image[] keysTab;

    public int lives = 3;
    public Image[] livesTab;

    public float timer = 0;

    const string keyHighScore = "HighScoreLevel1";

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnReturnToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
    public void AddKill()
    {
        enemiesKilled++;
        enemyText.text = enemiesKilled.ToString();
    }
    public void AddLives()
    {
        livesTab[lives].enabled = true;
        lives++;
    }
    public void SubtractLives()
    {
        lives--;
        livesTab[lives].enabled = false;
        if (lives <= 0)
            GameOver();
    }
    public void AddKeys()
    {
        keysTab[keysFound].color = Color.magenta;
        keysFound++;
    }

    public void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Level1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore < score)
                {
                    PlayerPrefs.SetInt(keyHighScore, score);
                    highScore = score;
                }
                lvlCompletedScoreText.text = "Your score: " + score;
                highScoreText.text = "The best score: " + highScore;
            }
        }
        if (currentGameState == GameState.GS_GAME)
            inGameCanvas.enabled = true;
        currentGameState = newGameState;
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    void Awake()
    {
        instance = this;
        scoreText.text = score.ToString();
        enemyText.text = enemiesKilled.ToString();
        timerText.text = string.Format("{0:00}:{1:00}", (int)timer / 60, (int)timer%60);
        for(int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.grey;
        }
        for (int i = lives; i < livesTab.Length; i++)
        {
            livesTab[i].enabled = false;
        }

        InGame();

        if (!PlayerPrefs.HasKey(keyHighScore))
            PlayerPrefs.SetInt(keyHighScore, 0);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameState.GS_GAME)
            timer += Time.deltaTime;
        timerText.text = string.Format("{0:00}:{1:00}", (int)timer/60, (int)timer%60);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentGameState == GameState.GS_GAME)
                PauseMenu();
            else if(currentGameState == GameState.GS_PAUSEMENU)
                InGame();
                
        }
    }
}

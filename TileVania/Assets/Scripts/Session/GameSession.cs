using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
  
    [SerializeField] float playerLives = 3;
    [SerializeField] float gold = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI goldText;
    public float PlayerLives => playerLives;

    void Awake()
    {
        int numberOfGamesession = FindObjectsOfType<GameSession>().Length;
        if (numberOfGamesession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        livesText.text = playerLives.ToString(); 
        goldText.text = gold.ToString();

    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        gold += pointsToAdd;
        goldText.text = gold.ToString();
    }

    private void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesText;
    private GameSession gameSession;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        SetLivesText(gameSession.PlayerLives);
    }

    private void Update()
    {
        SetLivesText(gameSession.PlayerLives);
    }

    public void SetLivesText(float amount)
    {
        livesText.text = amount.ToString();
    }
}

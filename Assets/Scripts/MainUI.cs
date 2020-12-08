using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;

    private void Start()
    {
        Player.player.addScore += OnScoreAdd; // Подписка на событие
    }

    public void OnScoreAdd() => scoreText.text = Player.player._score.ToString();
}

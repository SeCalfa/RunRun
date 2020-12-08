using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI : MonoBehaviour
{
    [SerializeField] private Text currentScoreText = null;
    [SerializeField] private Text maxScoreText = null;
    [SerializeField] private Image crown1 = null;
    [SerializeField] private Image crown2 = null;

    private void Start()
    {
        Player.player.addScore += OnScoreAdd; // Event subscription
    }

    public void OnScoreAdd()
    {
        currentScoreText.text = Player.player._score.ToString();

        if (Player.player._score > PlayerPrefs.GetInt("MaxScore"))
        {

        }
    }
}

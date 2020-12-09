using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI : MonoBehaviour
{
    [SerializeField] private Text currentScoreText = null;
    [SerializeField] private Text maxScoreText = null;
    [SerializeField] private Text tapToPlay = null;
    [SerializeField] private Image levelBarFG = null;
    [SerializeField] private Image fade = null;

    private float _barOffset = 0;
    private float _fadeAlpha = 0;

    private void Start()
    {
        StartCoroutine(FadeDisappear());
        Player.player.addScore += OnScoreAdd; // Event subscription
        Player.player.firstTap += FirstTap; // Event subscription
        maxScoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
        _barOffset = 1 / (float)PlayerPrefs.GetInt("MaxScore");
    }

    private void OnScoreAdd()
    {
        currentScoreText.text = Player.player._score.ToString();

        if (Player.player._score < PlayerPrefs.GetInt("MaxScore"))
        {
            StartCoroutine(FillingBG());
        }
    }

    private void FirstTap() => tapToPlay.gameObject.SetActive(false);

    private IEnumerator FillingBG()
    {
        int num = 0;
        Reset:
        num++;
        levelBarFG.fillAmount += (_barOffset / 20f);
        yield return new WaitForSeconds(_barOffset / 20f);
        if (num < 20)
            goto Reset;
    }

    private IEnumerator FadeDisappear()
    {
        _fadeAlpha = 1;

        Reset:
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, _fadeAlpha);
        _fadeAlpha -= 0.04f;
        yield return new WaitForSeconds(0.04f);
        if (_fadeAlpha >= 0)
            goto Reset;

        Player.player.canMove = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public UnityEvent onReset;

    public static GameManager instance;
    public GameObject readyPanel;
    public Text ScoreText;
    public Text BestScoreText;
    public Text MessageText;

    public bool isRoundActive = false;
    private int score = 0;

    public ShooterRotater shooterRotater;
    public CamFollow cam;

    void Awake()
    {
        instance = this;
        UpdateUI();
    }

    void Start()
    {
        StartCoroutine("RoundRoutine");
    }
    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    void UpdateBestScore()
    {
        if (GetBestScore() < score) 
            PlayerPrefs.SetInt("BestScore", score);
    }

    int GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;
    }

    void UpdateUI()
    {
        ScoreText.text = "Score: " + score;
        BestScoreText.text = "Best Score: " + GetBestScore();
    }

    //ball이 파괴되었을때 호출됨
    public void OnBallDestroy()
    {
        UpdateUI();
        isRoundActive = false;
    }

    public void Reset()
    {
        score = 0;
        UpdateUI();
        //라운드 재시작
        StartCoroutine("RoundRoutine");
    }

    IEnumerator RoundRoutine()
    {
        //Ready
        onReset.Invoke();
        readyPanel.SetActive(true);
        cam.SetTarget(shooterRotater.transform, CamFollow.State.Idle);
        shooterRotater.enabled = false;
        isRoundActive = false;

        MessageText.text = "Ready...";
        yield return new WaitForSeconds(3f);
        //Play
        isRoundActive = true;
        readyPanel.SetActive(false);
        shooterRotater.enabled = true;
        cam.SetTarget(shooterRotater.transform, CamFollow.State.Ready);
        while(isRoundActive == true)//공이 땅에 닿을때 까지 무한루프
        {
            yield return null;
        }
        //End
        readyPanel.SetActive(true);
        shooterRotater.enabled = false;

        MessageText.text = "Wait For Next Round...";

        yield return new WaitForSeconds(3f);
        Reset();
    }
}

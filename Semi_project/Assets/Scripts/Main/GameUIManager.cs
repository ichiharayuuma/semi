using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI lifeNum;
    
    [SerializeField] private GameObject resultUI;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI life;
    [SerializeField] private TextMeshProUGUI totalScore;

    [Header("class")]
    [SerializeField] private PointManager pointManager;

    [Header("Scene")]
    [SerializeField] private string GameSceneName = "GameScene";
    [SerializeField] private string TitleSceneName = "Title";

    void Start()
    {
        if(timer == null)
        {
            Debug.LogAssertion("Timer is not setted!");
        }
        else
        {
            timer.text = string.Empty;
        }

        inGameUI.SetActive(true);
        resultUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(pointManager.IsInGame())
        {
            ShowTime();
            ShowLife();
        }
    }

    private void ShowTime() // ゲーム中にタイムを表示する
    {
        var time = pointManager.GetCurrentTime();

        if(time < 0)
        {
            return;
        }

        timer.text = time.ToString("f2");
    }

    private void ShowLife()
    {
        lifeNum.text = pointManager.GetCurrentLife().ToString();
    }

    public void ShowResult()
    {
        inGameUI.SetActive(false);
        resultUI.SetActive(true);

        //Delay(0.5f, () =>
        //{
        //    SetResultTime();
        //});
        //Delay(0.5f, () =>
        //{
        //    SetResultLife();
        //});
        //Delay(0.5f, () =>
        //{
        //    SetTotalScore();
        //});

        DOVirtual.DelayedCall(0.5f, () =>
        {
            SetResultTime();
        });
        DOVirtual.DelayedCall(0.8f, () =>
        {
            SetResultLife();
        });
        DOVirtual.DelayedCall(0.8f, () =>
        {
            SetTotalScore();
        });
    }

    private void SetResultTime()
    {
        var resultTime = pointManager.GetCurrentTime();
        var min = resultTime / 60;
        var sec = resultTime % 60;
        time.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    private void SetResultLife()
    {
        life.text = pointManager.GetResultLife().ToString();
    }

    private void SetTotalScore()
    {
        totalScore.text = pointManager.GetTotalScore().ToString();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void OnToStart()
    {
        SceneManager.LoadScene(TitleSceneName);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

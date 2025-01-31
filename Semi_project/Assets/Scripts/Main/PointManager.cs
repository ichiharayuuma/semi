using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private int startLifeNum = 5;

    private float time;
    private bool isStart;
    private float start_time;
    private float end_time;

    private int life = 0;

    private int timeScore = 0;
    private int lifeScore = 0;
    private int totalScore = 0;
    
    void Start()
    {
        time = -1;
        isStart = false;

        life = startLifeNum;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStart)
        {
            time = Time.time - start_time;
        }
    }

    public void StartTimer()
    {
        start_time = Time.time;
        isStart = true;
    }

    public void StopTimer()
    {
        end_time = Time.time;
        isStart = false;

        time = end_time - start_time;
        timeScore = 600 - (int)time;
        lifeScore = life * 50;
        totalScore = timeScore + lifeScore;
    }

    public void TakeDamage()
    {
        life--;
        if(life <= 0)
        {

        }
    }

    public float GetCurrentTime()
    {
        return time;
    }

    public int GetCurrentLife()
    {
        return life;
    }

    public bool IsInGame()
    {
        return isStart;
    }

    public int GetResultTime()
    {
        return timeScore;
    }

    public int GetResultLife()
    {
        return lifeScore;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}

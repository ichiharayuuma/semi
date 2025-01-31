using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    [SerializeField] private PointManager point_manager;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameUIManager uiManager;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            point_manager.StopTimer();
            player.Result();
            uiManager.ShowResult();

            // Time.timeScale = 0.1f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{
    [SerializeField] private PointManager point_manager;
    [SerializeField] private PlayerController player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            point_manager.StartTimer();
            player.OnStart();
        }
    }
}

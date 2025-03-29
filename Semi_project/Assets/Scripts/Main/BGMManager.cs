using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource IngameBGM;
    [SerializeField] private AudioSource resultBGM;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ResultAudio()
    {
        IngameBGM.Stop();
        resultBGM.Play();
    }
}

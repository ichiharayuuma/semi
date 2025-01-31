using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnStart()
    {
        SceneManager.LoadScene(gameSceneName);
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
